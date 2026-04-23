using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
namespace CalcLib
{
    public class CalcEngine
    {
        private int GetPriority(string op)// Метод распределения приоитетов
        {
            switch (op)
            {
                case "+": case "-": return 1;
                case "*": case "/": return 2;
                case "^": return 3; 
                default:
                    
                    if (IsFunction(op)) return 4;// Если это функция (sin, sqrt), даем ей высокий приоритет
                    return 0;
            }
        }
        public double Calculate(string expression, Dictionary<string, double> variables)
        {            
            var tokens = Tokenize(expression);// Токенизация

            var postfix = ToPostfix(tokens); // Алгоритм Дейкстры - превращаем обычную запись в постфиксную

            return Evaluate(postfix, variables);// Вычисление
        }
        private bool IsFunction(string token)
        {
            return new List<string> { "sin", "cos", "tg", "abs", "log", "ln", "pow", "sqrt", "√" }.Contains(token.ToLower());
        }

        private List<string> ToPostfix(List<string> tokens)
        {
            var output = new List<string>();
            var stack = new Stack<string>();

            foreach (var token in tokens)
            {
                if (string.IsNullOrWhiteSpace(token)) continue;

                if (double.TryParse(token, out _) || IsVariable(token))
                {
                    output.Add(token); // Числа и переменные — сразу в выход
                }
                else if (IsFunction(token) || token == "(")
                {
                    stack.Push(token);
                }
                else if (token == ")")
                {
                    while (stack.Count > 0 && stack.Peek() != "(")
                        output.Add(stack.Pop());

                    if (stack.Count > 0) stack.Pop(); // Удаляем саму "("

                    // Если перед скобкой была функция (sin, cos...), её тоже в выход
                    if (stack.Count > 0 && IsFunction(stack.Peek()))
                        output.Add(stack.Pop());
                }
                else // Если это оператор (+, -, *, /, ^)
                {
                    while (stack.Count > 0 && GetPriority(stack.Peek()) >= GetPriority(token) && stack.Peek() != "(")
                    {
                        output.Add(stack.Pop());
                    }
                    stack.Push(token);
                }
            }

            while (stack.Count > 0)
            {
                string op = stack.Pop();
                if (op != "(") output.Add(op); // Добавляем только операторы, не скобки
            }

            return output;
        }


        private List<string> Tokenize(string expr)
        {
            // Убираем все пробелы перед делением на токены
            string cleanExpr = expr.Replace(" ", "");
            var pattern = @"(\d+[\.,]?\d*)|([a-zA-Z]+)|(\+|-|\*|/|\^|\(|\)|,|√)";
            var matches = Regex.Matches(cleanExpr, pattern);

            var tokens = new List<string>();
            foreach (Match m in matches)
            {
                string val = m.Value.Trim(); // Чистим каждый токен
                if (!string.IsNullOrWhiteSpace(val))
                    tokens.Add(val);
            }
            return tokens;
        }


        private double Evaluate(List<string> postfix, Dictionary<string, double> variables)
        {
            var stack = new Stack<double>();

            foreach (var token in postfix)
            {
                if (string.IsNullOrWhiteSpace(token)) continue; // Пропускаем пустоту

                if (double.TryParse(token, out double number))
                {
                    stack.Push(number);
                }
                else if (variables.ContainsKey(token))
                {
                    stack.Push(variables[token]);
                }
                else if (variables.ContainsKey(token.Trim())) // Добавь .Trim()
                {
                    stack.Push(variables[token.Trim()]);
                }
                else if (!double.TryParse(token, out _) && !IsFunction(token) && !"+-*/^√()".Contains(token))
                {
                    throw new CalcException($"Неизвестная переменная или символ: '{token}'");
                }

                else if (IsFunction(token))
                {
                    if (stack.Count < 1) continue; // Или throw, если хочешь строгую проверку
                    var val = stack.Pop();
                    stack.Push(ApplyFunction(token, val));
                }
                else if ("+-*/^√".Contains(token)) // СТРОГАЯ ПРОВЕРКА: только если это оператор
                {
                    if (stack.Count < 2) throw new CalcException("Недостаточно данных для операции " + token);
                    var b = stack.Pop();
                    var a = stack.Pop();
                    stack.Push(ExecuteOperation(token, a, b));
                }
                // Всё остальное (запятые, скобки, мусор) просто игнорируем
            }

            return stack.Count > 0 ? stack.Pop() : 0;
        }

        private bool IsVariable(string token)// Проверка переменная ли это
        {
            return !double.TryParse(token, out _) &&
            !"+-*/^(),".Contains(token) && // Добавь запятую сюда
            !IsFunction(token);
        }

        private double ApplyFunction(string func, double a) // Расчёт функций
        {
            switch (func.ToLower())
            {
                case "√":
                case "sqrt": 
                    return Math.Sqrt(a);
                case "sin": 
                    return Math.Sin(a);
                case "cos": 
                    return Math.Cos(a);
                case "tg": 
                    return Math.Tan(a);
                case "abs":
                    return Math.Abs(a);
                case "ln":
                    return Math.Log(a);
                case "log":
                    return Math.Log10(a);
                default: 
                    throw new Exception("Неизвестная функция");
            }
        }

        private double ExecuteOperation(string op, double a, double b) // Метод для арифметических операций над 2 числами
        {
            switch (op)
            {
                case "+": return a + b;
                case "-": return a - b;
                case "*": return a * b;
                case "/":
                    if (b == 0) 
                        throw new CalcException("Деление на ноль!");
                    return a / b;
                case "^":
                    return Math.Pow(a, b);
                default: 
                    throw new CalcException("Неизвестный оператор: " + op);
            }
        }
    }
}
