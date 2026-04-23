using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib
{
    public enum CalcErrorCode
    {
        Unknown,
        DivisionByZero,
        InvalidVariable,
        InvalidFunction,
        InvalidOperator,
        SyntaxError,
        MathDomainError,    // логарифм от отрицательного, корень из отрицательного
        StackUnderflow,     // не хватает операндов
        ResultNaN,
        ResultInfinity
    }
    public class CalcException : Exception
    {
        public CalcErrorCode ErrorCode { get; }
        public string Token { get; }          // проблемный токен (если есть)
        public int Position { get; }          // позиция в выражении (если известно)
        public string Suggestion { get; }     // подсказка по исправлению

        public CalcException(string message, CalcErrorCode code = CalcErrorCode.Unknown)
            : base(message)
        {
            ErrorCode = code;
        }

        public CalcException(string message, CalcErrorCode code, string token)
            : this(message, code)
        {
            Token = token;
        }

        public CalcException(string message, CalcErrorCode code, int position)
            : this(message, code)
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
    }
}


    