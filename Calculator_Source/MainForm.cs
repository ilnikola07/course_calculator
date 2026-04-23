using Course_calculator;
using CalcLib;

namespace course_calculator
{
    public partial class MainForm : Form
    {
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
            FormHistory history = new FormHistory();
            history.Show();
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
                txtExpression.Text = txtExpression.Text.Remove(txtExpression.Text.Length - 1);// Убираем ровно один символ с конца

                if (txtExpression.Text.EndsWith(" "))// Если после удаления остался пробел (от оператора), удаляем и его
                {
                    txtExpression.Text = txtExpression.Text.TrimEnd();
                }
            }
        }

        private void BtnOperator_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                string op = btn.Text;


                if (op == "√") // Логика пробелов для красоты 
                {
                    txtExpression.Text += " √ ";// Так как корень пишется перед числом
                }
                else
                {
                    txtExpression.Text += " " + op + " ";// Остальные операторы ставятся между числами
                }
                txtExpression.Focus(); // Возвращаем курсор в конец
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
                double result = engine.Calculate(txtExpression.Text, vars);
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

                // Собираем переменные из таблицы
                foreach (DataGridViewRow row in dataGridViewPer.Rows)
                {
                    // Пропускаем пустые или неполные строки
                    if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                    {
                        string name = row.Cells[0].Value.ToString().Trim();
                        string valStr = row.Cells[1].Value.ToString().Replace('.', ','); // заменяем точку на запятую для Double.Parse

                        if (!string.IsNullOrEmpty(name) && double.TryParse(valStr, out double value))
                        {
                            vars[name] = value;
                        }
                    }
                }

                double result = engine.Calculate(txtExpression.Text, vars);
                txtResult.Text = result.ToString();

                // Сюда потом добавим сохранение в историю:
                // history.Add(txtExpression.Text, result); 
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
    }
}
