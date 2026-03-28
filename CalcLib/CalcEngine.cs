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
            // Создаём список с названиями функций, берём строку token и переводим в нижний 
            // регистр (чтобы прочиталось), а .Contains(...) — ищет результат в созданном списке
        }

        private List<string> ToPostfix(List<string> tokens)
        {
            var output = new List<string>();
            var stack = new Stack<string>();

            foreach (var token in tokens)
            {
                if (double.TryParse(token, out _) || IsVariable(token))
                {
                    output.Add(token); // Числа и переменные сразу в результат
                }
                else if (IsFunction(token) || token == "(")
                {
                    stack.Push(token);
                }
                else if (token == ")")
                {
                    while (stack.Count > 0 && stack.Peek() != "(")
                        output.Add(stack.Pop());
                    stack.Pop(); // Создаём открывающую скобку
                }
                else // Иначе если это оператор
                {
                    while (stack.Count > 0 && GetPriority(stack.Peek()) >= GetPriority(token))
                        output.Add(stack.Pop());
                    stack.Push(token);
                }
            }
            while (stack.Count > 0) 
                output.Add(stack.Pop());
            return output;
        }


        private List<string> Tokenize(string expr)
        {
            // Регулярное выражение для чисел, функций, переменных и операторов
            var pattern = @"(\d+\.?\d*)|([a-zA-Z]+)|(\+|-|\*|/|\^|\(|\)|,|√)";
            var matches = Regex.Matches(expr.Replace(" ", ""), pattern);
            var tokens = new List<string>();
            foreach (Match m in matches) 
                tokens.Add(m.Value);
            return tokens;
        }


        private double Evaluate(List<string> postfix, Dictionary<string, double> variables)
        {
            var stack = new Stack<double>();

            foreach (var token in postfix)
            {
                if (double.TryParse(token, out double number))
                {
                    stack.Push(number);
                }
                else if (variables.ContainsKey(token))
                {
                    stack.Push(variables[token]); // Подставляем значение переменной
                }
                else if (IsFunction(token))
                {
                    var val = stack.Pop();
                    stack.Push(ApplyFunction(token, val)); // Считаем sin, cos и подобные
                }
                else
                {
                    var b = stack.Pop();
                    var a = stack.Pop();
                    stack.Push(ExecuteOperation(token, a, b)); // Считаем +, -, *, /
                }
            }
            return stack.Pop();
        }

        private bool IsVariable(string token)// Проверка переменная ли это
        {
            //Токен считается переменной, если он
            return !double.TryParse(token, out _) && // не число
                   !"+-*/^()".Contains(token) && // не оператор
                   !IsFunction(token); // не математическая функция
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
