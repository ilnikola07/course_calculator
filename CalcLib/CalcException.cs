using System;
using System.Diagnostics.CodeAnalysis;
namespace CalcLib
{
    /// <summary>
    /// Коды ошибок для калькулятора
    /// </summary>
    public enum CalcErrorCode // Перечисление возможных проблем
    {
        Unknown,
        DivisionByZero,
        InvalidVariable,
        InvalidFunction,
        InvalidOperator,
        SyntaxError,
        MathDomainError,    // Логарифм от отрицательного, корень из отрицательного
        StackUnderflow,     // Не хватает операндов
        ResultNaN,
        ResultInfinity,
        UnbalancedParentheses
    }

    /// <summary>
    /// Исключение, возникающее при ошибках вычислений
    /// </summary>
    /// 
    [ExcludeFromCodeCoverage] // Исключить из отчёта о покрытии тестами
    public class CalcException : Exception
    {
        public CalcErrorCode ErrorCode { get; }
        public string Token { get; }
        public int Position { get; }

        public CalcException(string message, CalcErrorCode code = CalcErrorCode.Unknown)
            : base(message)  // Принимает текст ошибки (message) и code
        {
            ErrorCode = code;
        }

        public CalcException(string message, CalcErrorCode code, string token)
            : this(message, code) // Создает ошибку и запоминает конкретное ошибочное слово или символ (token)
        {
            Token = token;
        }

        public CalcException(string message, CalcErrorCode code, int position)
            : this(message, code) // Создает ошибку и запоминает индекс (номер символа) в строке, где была найдена проблема
        {
            Position = position;
        }

        public static CalcException DivisionByZero() =>
            new CalcException("Деление на ноль!", CalcErrorCode.DivisionByZero);

        public static CalcException InvalidVariable(string name) =>
            new CalcException($"Неизвестная переменная: '{name}'",
                             CalcErrorCode.InvalidVariable, name);

        public static CalcException InvalidFunction(string name) =>
            new CalcException($"Неизвестная функция: '{name}'",
                             CalcErrorCode.InvalidFunction, name);

        public static CalcException MathDomainError(string operation, double value) =>
            new CalcException($"Недопустимое значение для {operation}: {value}",
                             CalcErrorCode.MathDomainError);

        public static CalcException SyntaxError(string details) =>
            new CalcException($"Синтаксическая ошибка: {details}",
                             CalcErrorCode.SyntaxError);

        public static CalcException UnbalancedParentheses() =>
            new CalcException("Несбалансированные скобки в выражении",
                             CalcErrorCode.UnbalancedParentheses);

        public static CalcException StackUnderflow(string operation) =>
            new CalcException($"Недостаточно операндов для операции: {operation}",
                             CalcErrorCode.StackUnderflow);
    }
}