using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace CalcLib
{
    public class CalcEngine
    {
        private static readonly CultureInfo _culture = CultureInfo.InvariantCulture; // Использовать точку как разделитель в числах
        private int GetPriority(string op) // Выдача приоритета операциям
        {
            switch (op)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                case "^":
                    return 3;
                default:
                    if (IsFunction(op))
                        return 4;
                    return 0;
            }
        }

        /// <summary>
        /// Является ли токен функцией
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool IsFunction(string token)
        {
            var functions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) // С игнорированием регистра
            {
                "sin", "cos", "tan", "tg", "abs", "log", "ln", "sqrt", "√" // Коллекция (множество) разрешенных функций
            };
            return functions.Contains(token);
        }

        /// <summary>
        /// Является ли переменной 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
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
            if (string.IsNullOrWhiteSpace(expr)) // Если вдруг ничего нет
                throw CalcException.SyntaxError("Пустое выражение");

            string cleanExpr = expr.Replace(" ", ""); // Удаляет пробелы, если вдруг есть в строке

            var pattern = @"(?<number>\d+[.,]?\d*|[.,]\d+)|" + // Текущие паттерны, этот для цифр
                         @"(?<variable>[a-zA-Zа-яА-ЯёЁ][a-zA-Zа-яА-ЯёЁ0-9_]*)|" + // Для функций
                         @"(?<operator>\+\+|--|\+|-|\*|/|\^|√)|" + // Для операторов
                         @"(?<paren>[()])|" + // Для скобок
                         @"(?<comma>,)"; // Для запятой

            var matches = Regex.Matches(cleanExpr, pattern); // Просматривает всю строку и находит все, что подошли под правила выше
            var tokens = new List<string>();

            foreach (Match m in matches) // Проходит по всем и добавляет в список токенов
            {
                string val = m.Value.Trim(); // На всякий случай удалить невидимые символы
                if (!string.IsNullOrWhiteSpace(val))
                    tokens.Add(val);
            }
            
            tokens = ProcessUnaryMinus(tokens); // Обработка унарного минуса (замена "-" на "u-")
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

                if (token == "-") // Унарный минус если:
                {                  
                    bool isUnary = i == 0 || // В начале выражения
                    result.Count == 0 || // После другого оператора
                    IsOperator(result.Last()) || // Стоит сразу после другого оператора
                    result.Last() == "("; // После открывающей скобки

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

        /// <summary>
        /// Является ли оператором
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool IsOperator(string token)
        {
            return token is "+" or "-" or "*" or "/" or "^" or "u-"; 
        }

        /// <summary>
        /// Преобразует инфиксную запись в постфиксную (ОПН)
        /// </summary>
        private List<string> ToPostfix(List<string> tokens)
        {
            for (int i = 0; i < tokens.Count - 1; i++) // Ошибка, если нет знака между токенами
            {
                string current = tokens[i];
                string next = tokens[i + 1];                
                if ((current == ")" || double.TryParse(current, _culture, out _) || IsVariable(current)) &&
                    (next == "(" || IsFunction(next) || double.TryParse(next, _culture, out _) || IsVariable(next)))
                {
                    throw CalcException.SyntaxError("Пропущен оператор между выражениями");
                }
            }
            var output = new List<string>(); // Очередь для итогового выражения
            var stack = new Stack<string>(); // Временный склад для знаков и функций

            foreach (var token in tokens)
            {
                if (string.IsNullOrWhiteSpace(token)) // Если вдруг ничего
                    continue;
                                
                if (double.TryParse(token, _culture, out _) || IsVariable(token)) // Числа и переменные — сразу в выход
                {
                    output.Add(token);
                }
                
                else if (IsFunction(token) || token == "(") // Функции и открывающая скобка — в стек
                {
                    stack.Push(token);
                }
                
                else if (token == ",") // Запятая — разделитель аргументов 
                {                    
                    while (stack.Count > 0 && stack.Peek() != "(") // Выгружаем всё до ближайшей "("
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
                
                else if (token == ")") // Закрывающая скобка
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
                        output.Add(top); // Переносим знаки из стека в результат
                    }
                    if (!foundOpen)
                        throw CalcException.UnbalancedParentheses(); // Иначе ошибка несбалансированные скобки
                                       
                    if (stack.Count > 0 && IsFunction(stack.Peek())) // Если перед скобкой была функция — выгружаем её
                        output.Add(stack.Pop());
                }                
                else if (IsOperator(token) || token == "√") // Операторы
                {                    
                    int tokenPriority = token == "u-" ? 5 : GetPriority(token); // Для унарного минуса — особый приоритет

                    while (stack.Count > 0 && stack.Peek() != "(")
                    {
                        string top = stack.Peek();
                        int topPriority = top == "u-" ? 5 : GetPriority(top);

                        if (topPriority >= tokenPriority)
                            output.Add(stack.Pop());
                        else
                            break;
                    }
                    stack.Push(token); // Текущий знак ложится в стек
                }
                else
                {
                    throw CalcException.SyntaxError($"Неизвестный токен: '{token}'");
                }
            }            
            while (stack.Count > 0) // Выгружаем остаток стека
            {
                string op = stack.Pop();
                if (op == "(")
                    throw CalcException.UnbalancedParentheses();
                output.Add(op); // Если вдруг забыли закрыть )
            }

            return output;
        }

        /// <summary>
        /// Вычисляет значение постфиксного выражения
        /// </summary>
        private double Evaluate(List<string> postfix, Dictionary<string, double> variables)
        {
            if (postfix == null || postfix.Count == 0) // Если вдруг подавлось с ошибкой - выбросить исключение
                throw CalcException.SyntaxError("Пустое выражение");

            var stack = new Stack<double>(); // Создаётся копия
            var normalizedVars = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase); // Нужно, чтобы X и x работали одинаково

            if (variables != null) // Передан ли вообще какой-нибудь словарь с переменными
            { 
                foreach (var kvp in variables)
                    normalizedVars[kvp.Key.ToLower()] = kvp.Value; // Это пара Имя переменной — Её значение
            }

            foreach (var token in postfix)
            {
                if (string.IsNullOrWhiteSpace(token)) // Если вдруг пустота - пропустить
                    continue;
                if (double.TryParse(token, _culture, out double number)) // Число - переводим в double и в вершину стека
                {
                    stack.Push(number);
                }                
                else if (IsVariable(token))// Переменная - найти значение в словаре, иначе - ошибка
                {
                    string key = token.ToLower();
                    if (normalizedVars.TryGetValue(key, out double value))
                        stack.Push(value);
                    else
                        throw CalcException.InvalidVariable(token);
                }                
                else if (token == "u-") // Унарный минус
                {
                    if (stack.Count < 1)
                        throw CalcException.StackUnderflow("унарный минус");
                    stack.Push(-stack.Pop());
                }                
                else if (IsFunction(token)) // Функция
                {
                    if (stack.Count < 1)
                        throw CalcException.SyntaxError($"Недостаточно аргументов для функции '{token}'");

                    double arg = stack.Pop(); // Берем одно число (аргумент)
                    double result = ApplyFunction(token, arg);  // Считаем функцию

                    if (double.IsNaN(result)) // Возможны математические ошибки
                        throw CalcException.MathDomainError(token, arg);
                    if (double.IsInfinity(result))
                        throw new CalcException($"Бесконечный результат функции {token}", CalcErrorCode.ResultInfinity);

                    stack.Push(result);
                }                
                else if ("+-*/^√".Contains(token)) // Остальные операторы
                {
                    if (stack.Count < 2)
                        throw CalcException.StackUnderflow(token);

                    double b = stack.Pop(); // Берем ВТОРОЕ число (правое)
                    double a = stack.Pop(); // Берем ПЕРВОЕ число (левое)
                    double result = ExecuteOperation(token, a, b);  // Рассчитываем

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

        /// <summary>
        /// // Расчёт функций
        /// </summary>
        /// <param name="func"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public double ApplyFunction(string func, double a) 
        {
            return func.ToLower() // func.ToLower() переводит, чтобы считалось вне зависимости от регистра
                switch
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

        /// <summary>
        /// Расчёт обычных операций
        /// </summary>
        /// <param name="op"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="CalcException"></exception>
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
        /// Вычисляет значение математического выражения (срабатывает пр нажатии =)
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