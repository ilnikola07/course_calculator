using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace CalcLib
{
    public class CalcEngine
    {
        private static readonly CultureInfo _culture = CultureInfo.InvariantCulture;
        private int GetPriority(string op)
        {
            return op switch
            {
                "+" or "-" => 1,
                "*" or "/" => 2,
                "^" => 3,
                var t when IsFunction(t) => 4,
                _ => 0
            };
        }
        private bool IsFunction(string token)
        {
            var functions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "sin", "cos", "tan", "tg", "abs", "log", "ln", "sqrt", "√"
            };
            return functions.Contains(token);
        }

        private bool IsVariable(string token)
        {
            return !double.TryParse(token, _culture, out _) &&
                   !"+-*/^(),".Contains(token) &&
                   !IsFunction(token) &&
                   !string.IsNullOrWhiteSpace(token) &&
                   Regex.IsMatch(token, @"^[a-zA-Zа-яА-ЯёЁ][a-zA-Zа-яА-ЯёЁ0-9_]*$");
        }

        /// <summary>
        /// Разбивает выражение на токены (числа, операторы, функции, скобки)
        /// </summary>
        private List<string> Tokenize(string expr)
        {
            if (string.IsNullOrWhiteSpace(expr))
                throw CalcException.SyntaxError("Пустое выражение");

            // Убираем пробелы для упрощения парсинга
            string cleanExpr = expr.Replace(" ", "");

            // Расширенный паттерн: поддерживает числа вида .5, 5., 5.5, переменные, операторы
            var pattern = @"(?<number>\d+[.,]?\d*|[.,]\d+)|" +
                         @"(?<variable>[a-zA-Zа-яА-ЯёЁ][a-zA-Zа-яА-ЯёЁ0-9_]*)|" +
                         @"(?<operator>\+\+|--|\+|-|\*|/|\^|√)|" +
                         @"(?<paren>[()])|" +
                         @"(?<comma>,)";

            var matches = Regex.Matches(cleanExpr, pattern);
            var tokens = new List<string>();

            foreach (Match m in matches)
            {
                string val = m.Value.Trim();
                if (!string.IsNullOrWhiteSpace(val))
                    tokens.Add(val);
            }

            // Обработка унарного минуса: заменяем "-" на "u-" в нужных позициях
            tokens = ProcessUnaryMinus(tokens);

            return tokens;
        }

        /// <summary>
        /// Преобразует бинарный минус в унарный там, где это необходимо
        /// </summary>
        private List<string> ProcessUnaryMinus(List<string> tokens)
        {
            var result = new List<string>();

            for (int i = 0; i < tokens.Count; i++)
            {
                string token = tokens[i];

                if (token == "-")
                {
                    // Унарный минус если:
                    // 1. В начале выражения
                    // 2. После открывающей скобки
                    // 3. После другого оператора
                    bool isUnary = i == 0 ||
                                   result.Count == 0 ||
                                   IsOperator(result.Last()) ||
                                   result.Last() == "(";

                    if (isUnary)
                    {
                        result.Add("u-"); // Маркер унарного минуса
                        continue;
                    }
                }
                result.Add(token);
            }

            return result;
        }

        private bool IsOperator(string token)
        {
            return token is "+" or "-" or "*" or "/" or "^" or "u-";
        }

        /// <summary>
        /// Преобразует инфиксную запись в постфиксную (ОПН)
        /// </summary>
        private List<string> ToPostfix(List<string> tokens)
        {
            var output = new List<string>();
            var stack = new Stack<string>();

            foreach (var token in tokens)
            {
                if (string.IsNullOrWhiteSpace(token))
                    continue;

                // Числа и переменные — сразу в выход
                if (double.TryParse(token, _culture, out _) || IsVariable(token))
                {
                    output.Add(token);
                }
                // Функции и открывающая скобка — в стек
                else if (IsFunction(token) || token == "(")
                {
                    stack.Push(token);
                }
                // Запятая — разделитель аргументов функции
                else if (token == ",")
                {
                    // Выгружаем всё до ближайшей "("
                    while (stack.Count > 0 && stack.Peek() != "(")
                    {
                        string top = stack.Pop();
                        if (top == "(")
                        {
                            stack.Push(top);
                            break;
                        }
                        output.Add(top);
                    }

                    if (stack.Count == 0)
                        throw CalcException.SyntaxError("Запятая вне функции");
                }
                // Закрывающая скобка
                else if (token == ")")
                {
                    bool foundOpen = false;

                    while (stack.Count > 0)
                    {
                        string top = stack.Pop();
                        if (top == "(")
                        {
                            foundOpen = true;
                            break;
                        }
                        output.Add(top);
                    }

                    if (!foundOpen)
                        throw CalcException.UnbalancedParentheses();

                    // Если перед скобкой была функция — выгружаем её
                    if (stack.Count > 0 && IsFunction(stack.Peek()))
                        output.Add(stack.Pop());
                }
                // Операторы
                else if (IsOperator(token) || token == "√")
                {
                    // Для унарного минуса — особый приоритет
                    int tokenPriority = token == "u-" ? 5 : GetPriority(token);

                    while (stack.Count > 0 && stack.Peek() != "(")
                    {
                        string top = stack.Peek();
                        int topPriority = top == "u-" ? 5 : GetPriority(top);

                        if (topPriority >= tokenPriority)
                            output.Add(stack.Pop());
                        else
                            break;
                    }
                    stack.Push(token);
                }
                else
                {
                    throw CalcException.SyntaxError($"Неизвестный токен: '{token}'");
                }
            }

            // Выгружаем остаток стека
            while (stack.Count > 0)
            {
                string op = stack.Pop();
                if (op == "(")
                    throw CalcException.UnbalancedParentheses();
                output.Add(op);
            }

            return output;
        }

        /// <summary>
        /// Вычисляет значение постфиксного выражения
        /// </summary>
        private double Evaluate(List<string> postfix, Dictionary<string, double> variables)
        {
            if (postfix == null || postfix.Count == 0)
                throw CalcException.SyntaxError("Пустое выражение");

            var stack = new Stack<double>();
            var normalizedVars = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

            if (variables != null)
            {
                foreach (var kvp in variables)
                    normalizedVars[kvp.Key.ToLower()] = kvp.Value;
            }

            foreach (var token in postfix)
            {
                if (string.IsNullOrWhiteSpace(token))
                    continue;

                // Число
                if (double.TryParse(token, _culture, out double number))
                {
                    stack.Push(number);
                }
                // Переменная
                else if (IsVariable(token))
                {
                    string key = token.ToLower();
                    if (normalizedVars.TryGetValue(key, out double value))
                        stack.Push(value);
                    else
                        throw CalcException.InvalidVariable(token);
                }
                // Унарный минус
                else if (token == "u-")
                {
                    if (stack.Count < 1)
                        throw CalcException.StackUnderflow("унарный минус");
                    stack.Push(-stack.Pop());
                }
                // Функция
                else if (IsFunction(token))
                {
                    if (stack.Count < 1)
                        throw CalcException.SyntaxError($"Недостаточно аргументов для функции '{token}'");

                    double arg = stack.Pop();
                    double result = ApplyFunction(token, arg);

                    if (double.IsNaN(result))
                        throw CalcException.MathDomainError(token, arg);
                    if (double.IsInfinity(result))
                        throw new CalcException($"Бесконечный результат функции {token}", CalcErrorCode.ResultInfinity);

                    stack.Push(result);
                }
                // Бинарный оператор
                else if ("+-*/^√".Contains(token))
                {
                    if (stack.Count < 2)
                        throw CalcException.StackUnderflow(token);

                    double b = stack.Pop();
                    double a = stack.Pop();
                    double result = ExecuteOperation(token, a, b);

                    if (double.IsNaN(result))
                        throw CalcException.MathDomainError("операция", result);
                    if (double.IsInfinity(result))
                        throw new CalcException($"Бесконечный результат операции {token}", CalcErrorCode.ResultInfinity);

                    stack.Push(result);
                }
                else
                {
                    throw CalcException.SyntaxError($"Неизвестный токен в выражении: '{token}'");
                }
            }

            if (stack.Count != 1)
                throw CalcException.SyntaxError("Ошибка в структуре выражения");

            return stack.Pop();
        }

        public double ApplyFunction(string func, double a)
        {
            return func.ToLower() switch
            {
                "sqrt" or "√" => a < 0
                    ? throw CalcException.MathDomainError("квадратного корня", a)
                    : Math.Sqrt(a),

                "sin" => Math.Sin(a),
                "cos" => Math.Cos(a),
                "tan" or "tg" => Math.Tan(a),
                "abs" => Math.Abs(a),

                "ln" => a <= 0
                    ? throw CalcException.MathDomainError("натурального логарифма", a)
                    : Math.Log(a, Math.E),

                "log" => a <= 0
                    ? throw CalcException.MathDomainError("десятичного логарифма", a)
                    : Math.Log10(a),

                _ => throw CalcException.InvalidFunction(func)
            };
        }

        private double ExecuteOperation(string op, double a, double b)
        {
            return op switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => b == 0
                    ? throw CalcException.DivisionByZero()
                    : a / b,
                "^" => Math.Pow(a, b),
                "√" => a < 0
                    ? throw CalcException.MathDomainError("корня", a)
                    : Math.Pow(a, 1.0 / b),
                _ => throw new CalcException($"Неизвестный оператор: {op}", CalcErrorCode.InvalidOperator)
            };
        }

        /// <summary>
        /// Вычисляет значение математического выражения
        /// </summary>
        /// <param name="expression">Выражение в инфиксной записи</param>
        /// <param name="variables">Словарь переменных</param>
        /// <returns>Результат вычисления</returns>
        public double Calculate(string expression, Dictionary<string, double> variables = null)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw CalcException.SyntaxError("Пустое выражение");

            var tokens = Tokenize(expression);
            var postfix = ToPostfix(tokens);
            return Evaluate(postfix, variables);
        }

        /// <summary>
        /// Проверяет синтаксис выражения без вычисления
        /// </summary>
        public bool ValidateSyntax(string expression)
        {
            try
            {
                var tokens = Tokenize(expression);
                ToPostfix(tokens);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}