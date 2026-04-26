using CalcLib;
using Course_calculator;
using System;
using System.Collections.Generic;
using System.Globalization;  
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace course_calculator  
{
    public partial class MainForm : Form
    {
        private CalcHistory historyManager = new CalcHistory();
        private Dictionary<string, double> variables = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

        public MainForm()
        {
            InitializeComponent();
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
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
                string expression = historyForm.SelectedExpression;
                txtExpression.Text = expression;
            }
        }

        private void BtnDigit_Click(object sender, EventArgs e)
        {
            if (sender is Button clickedButton)
            {
                string digit = clickedButton.Text;

                if (digit == ",")
                {
                    // Проверяем, чтобы не было двух запятых в одном числе
                    if (string.IsNullOrEmpty(txtExpression.Text))
                    {
                        txtExpression.Text = "0,";
                        txtExpression.Focus();
                        txtExpression.SelectionStart = txtExpression.Text.Length;
                        return;
                    }

                    // Находим последнюю операцию или скобку
                    string lastPart = txtExpression.Text;
                    int lastOp = Math.Max(
                        lastPart.LastIndexOf('+'),
                        Math.Max(lastPart.LastIndexOf('-'),
                        Math.Max(lastPart.LastIndexOf('*'),
                        Math.Max(lastPart.LastIndexOf('/'),
                        Math.Max(lastPart.LastIndexOf('('),
                        Math.Max(lastPart.LastIndexOf('^'),
                        lastPart.LastIndexOf('=')))))));

                    if (lastOp >= 0)
                        lastPart = lastPart.Substring(lastOp + 1);

                    // Если в текущем числе уже есть запятая или точка — не добавляем
                    if (lastPart.Contains(",") || lastPart.Contains("."))
                    {
                        return;
                    }
                }

                txtExpression.Text += digit;  // ← Добавляем как есть (запятую как запятую!)
                txtExpression.Focus();
                txtExpression.SelectionStart = txtExpression.Text.Length;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtExpression.Clear();
            txtResult.Clear();
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            if (txtExpression.Text.Length > 0)
            {
                txtExpression.Text = txtExpression.Text.Substring(0, txtExpression.Text.Length - 1);
                txtExpression.SelectionStart = txtExpression.Text.Length;
            }
        }

        private void BtnOperator_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                string op = btn.Text;
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
                if (parenthesis == ")")
                {
                    int openCount = txtExpression.Text.Count(f => f == '(');
                    int closeCount = txtExpression.Text.Count(f => f == ')');

                    if (closeCount >= openCount)
                    {
                        MessageBox.Show("Ошибка: нет соответствующей открывающей скобки!",
                                        "Ошибка синтаксиса", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                txtExpression.Text += parenthesis;
                txtExpression.Focus();
                txtExpression.SelectionStart = txtExpression.Text.Length;
            }
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtExpression.Text)) return;
            string text = txtExpression.Text;
            int openBrackets = text.Count(c => c == '(');
            int closeBrackets = text.Count(c => c == ')');
            while (openBrackets > closeBrackets)
            {
                text += ")";
                closeBrackets++;
            }
            txtExpression.Text = text;  

            try
            {
                variables.Clear();
                foreach (DataGridViewRow row in dataGridViewPer.Rows)
                {
                    if (row.IsNewRow) continue;
                    var name = row.Cells[0].Value?.ToString()?.Trim();
                    var valStr = row.Cells[1].Value?.ToString()?.Trim();

                    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(valStr)) continue;

                    valStr = valStr.Replace(',', '.');
                    if (double.TryParse(valStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
                        variables[name] = val; 
                }

                var engine = new CalcEngine();
                double result = engine.Calculate(text.Trim(), variables);  
                if (double.IsNaN(result)) throw CalcException.MathDomainError("вычисление", result);
                if (double.IsInfinity(result)) throw new CalcException("Бесконечный результат", CalcErrorCode.ResultInfinity);

                txtResult.Text = result.ToString("G15", CultureInfo.InvariantCulture);
                historyManager.Add(txtExpression.Text, result);  
            }
            catch (CalcException ex)
            {
                MessageBoxIcon icon = MessageBoxIcon.Error;
                if (ex.ErrorCode == CalcErrorCode.DivisionByZero || ex.ErrorCode == CalcErrorCode.MathDomainError)
                    icon = MessageBoxIcon.Warning;

                MessageBox.Show(ex.Message, "Ошибка вычисления", MessageBoxButtons.OK, icon);
                txtResult.Text = "Ошибка";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Непредвиденная ошибка: " + ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FunctionButton(object sender, EventArgs e)
        {
            var button = (Button)sender;
            string func = button.Text.ToLower();

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
            string value = e.FormattedValue.ToString().Trim();

            if (string.IsNullOrEmpty(value)) return;

            if (e.ColumnIndex == 0)
            {
                if (!value.All(char.IsLetter))
                {
                    MessageBox.Show("Имя переменной должно состоять только из букв!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
            else if (e.ColumnIndex == 1)
            {
                string normalized = value.Replace(',', '.');
                if (!double.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                {
                    MessageBox.Show("Значение должно быть числом!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
        }

        private void dataGridViewPer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                foreach (DataGridViewCell cell in dataGridViewPer.SelectedCells)
                {
                    cell.Value = string.Empty;
                }
                e.Handled = true;
            }
        }
    }
}