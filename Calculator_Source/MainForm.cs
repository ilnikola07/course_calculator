using Course_calculator;
using CalcLib;
using System.Linq;

namespace course_calculator
{
    public partial class MainForm : Form
    {
        private CalcHistory historyManager = new CalcHistory();
        private CalcLib.CalcHistory calculatorHistory = new CalcLib.CalcHistory();
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnRef_Click(object sender, EventArgs e)
        {
            FormAbout about = new FormAbout();
            about.Show();
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            FormHistory historyForm = new FormHistory(historyManager.GetAll());
            DialogResult result = historyForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                string expression = historyForm.SelectedExpression; // Получаем выбранное выражение
                txtExpression.Text = expression; // Вставляем в поле ввода главной формы
            }
        }

        private void BtnDigit_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (sender is Button clickedButton)// Определение, какая именно кнопка была нажата
            {
                string digit = clickedButton.Text;
                if (digit == ",") // Логика для запятой (разделителя)
                {
                    if (string.IsNullOrEmpty(txtExpression.Text)) // Проверка, чтобы в поле уже что-то было
                    {
                        txtExpression.Text = "0,";
                        return;
                    }
                    if (txtExpression.Text.EndsWith(","))// Простая проверка: не даем поставить две запятые подряд
                    {
                        return;
                    }

                }
                txtExpression.Text += digit; // Добавляем символ в текстовое поле                
                txtExpression.Focus();// Устанавливаем фокус обратно в поле и переводим курсор в конец
                txtExpression.SelectionStart = txtExpression.Text.Length;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtExpression.Clear(); // Очистить ввод
            txtResult.Clear();     // Очистить результат
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            if (txtExpression.Text.Length > 0)
            {
                // Просто удаляем последний символ
                txtExpression.Text = txtExpression.Text.Remove(txtExpression.Text.Length - 1);

                txtExpression.SelectionStart = txtExpression.Text.Length;
            }
        }

        private void BtnOperator_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                string op = btn.Text;

                // Просто добавляем оператор без пробелов
                txtExpression.Text += op;

                txtExpression.Focus();
                txtExpression.SelectionStart = txtExpression.Text.Length;
            }
        }
        private void BtnParenthesis_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                string parenthesis = btn.Text;
                if (parenthesis == ")") // Если нажата закрывающая скобка
                {
                    int openCount = txtExpression.Text.Count(f => f == '('); // Считаем количество открывающих и закрывающих скобок в строке
                    int closeCount = txtExpression.Text.Count(f => f == ')');

                    if (closeCount >= openCount) // Если закрывающих уже столько же, сколько открытых (или больше)
                    {
                        MessageBox.Show("Ошибка: нет соответствующей открывающей скобки!",
                                        "Ошибка синтаксиса", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Выходим из метода, не добавляя скобку в поле
                    }
                }
                txtExpression.Text += parenthesis;  // Если проверка пройдена или это открывающая скобка — добавляем в поле               
                txtExpression.Focus(); // Возвращаем фокус и курсор в конец
                txtExpression.SelectionStart = txtExpression.Text.Length;
            }
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            string text = txtExpression.Text;
            int openBrackets = text.Split('(').Length - 1; // Считаем количество открытых и закрытых скобок
            int closeBrackets = text.Split(')').Length - 1;
            while (openBrackets > closeBrackets) // Добавляем недостающие закрывающие скобки
            {
                text += ")";
                closeBrackets++;
            }
            txtExpression.Text = text;
            if (string.IsNullOrWhiteSpace(txtExpression.Text))
                return;

            try
            {
                var engine = new CalcEngine();
                var vars = new Dictionary<string, double>(); // Получаем переменные из таблицы (ваша правая панель)
                double result = engine.Calculate(txtExpression.Text.ToLower(), vars);
                txtResult.Text = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                var engine = new CalcEngine();
                var vars = new Dictionary<string, double>();

                foreach (DataGridViewRow row in dataGridViewPer.Rows)
                {
                    if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                    {
                        // .ToLower() делает имя переменной строчным (x)
                        string name = row.Cells[0].Value.ToString().Trim().ToLower();
                        string valStr = row.Cells[1].Value.ToString().Trim().Replace('.', ',');

                        if (!string.IsNullOrEmpty(name) && double.TryParse(valStr, out double value))
                        {
                            vars[name] = value;
                        }
                    }
                }

                // Здесь тоже добавляем .ToLower(), чтобы x/z превратилось в x/z (на всякий случай)
                double result = engine.Calculate(txtExpression.Text.Trim().ToLower(), vars);
                txtResult.Text = result.ToString();

                historyManager.Add(txtExpression.Text, result);
                // СОХРАНЯЕМ В ИСТОРИЮ
                calculatorHistory.Add(txtExpression.Text, result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void FunctionButton(object sender, EventArgs e) // Oтвечает за визуальную вставку функции в поле ввода
        {
            var button = (Button)sender; // Определяем, какая кнопка нажата
            string func = button.Text.ToLower(); // Берем текст кнопки (например, "cos")

            txtExpression.Text += func + "(";
            txtExpression.Focus();
            txtExpression.SelectionStart = txtExpression.Text.Length;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void dataGridViewPer_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Индекс 0 — колонка "Переменная", Индекс 1 — колонка "Значение"
            string value = e.FormattedValue.ToString().Trim();

            if (string.IsNullOrEmpty(value)) return; // Разрешаем пустые ячейки (для удаления)

            if (e.ColumnIndex == 0) // Проверка имени переменной
            {
                // Проверяем, что в имени только буквы (латиница/кириллица)
                if (!value.All(char.IsLetter))
                {
                    MessageBox.Show("Имя переменной должно состоять только из букв!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true; // Отменяем переход, заставляя исправить ввод
                }
            }
            else if (e.ColumnIndex == 1) // Проверка значения
            {
                // Пытаемся преобразовать в число (с учетом запятой/точки)
                if (!double.TryParse(value.Replace('.', ','), out _))
                {
                    MessageBox.Show("Значение должно быть числом!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
        }

        private void dataGridViewPer_KeyDown(object sender, KeyEventArgs e)
        {
            // Проверяем, нажата ли клавиша Delete
            if (e.KeyCode == Keys.Delete)
            {
                foreach (DataGridViewCell cell in dataGridViewPer.SelectedCells)
                {
                    // Очищаем значение в выделенных ячейках
                    cell.Value = string.Empty;
                }
                e.Handled = true; // Сообщаем системе, что нажатие обработано
            }
        }
    }
}
