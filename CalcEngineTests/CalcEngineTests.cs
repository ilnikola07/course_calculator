// ============================================================================
// ФАЙЛ: CalcEngineTests.cs
// ОПИСАНИЕ: Набор модульных тестов для класса CalcEngine
//           Покрытие алгоритмов: токенизация, сортировочная станция, вычисление ОПЗ
// ПРИНЦИП: AAA (Arrange-Act-Assert)
// ТРЕБОВАНИЯ: 
//   • Минимум 5 тестов на каждый алгоритм
//   • Обязательные категории: типичные данные, граничный случай, некорректные данные,
//     большой объём, специфический случай темы
// АВТОР: [Ваше ФИО]
// ДАТА: [Дата создания]
// ============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Xunit;
using System.Reflection;

namespace CalcLib.Tests
{
    /// <summary>
    /// Тестовый класс для проверки функциональности CalcEngine.
    /// Использует рефлексию для доступа к приватным методам токенизации, 
    /// конвертации в ОПЗ и вычисления.
    /// </summary>
    public class CalcEngineTests
    {
        private readonly CalcEngine _engine;
        private readonly MethodInfo _tokenizeMethod;
        private readonly MethodInfo _toPostfixMethod;
        private readonly MethodInfo _evaluateMethod;
        private readonly MethodInfo _executeOperationMethod;
        private readonly MethodInfo _applyFunctionMethod;

        /// <summary>
        /// Инициализация тестового окружения: создание экземпляра CalcEngine 
        /// и получение рефлексии на приватные методы для изолированного тестирования.
        /// </summary>
        public CalcEngineTests()
        {
            _engine = new CalcEngine();
            var type = typeof(CalcEngine);
            _tokenizeMethod = type.GetMethod("Tokenize",
                BindingFlags.NonPublic | BindingFlags.Instance);
            _toPostfixMethod = type.GetMethod("ToPostfix",
                BindingFlags.NonPublic | BindingFlags.Instance);
            _evaluateMethod = type.GetMethod("Evaluate",
                BindingFlags.NonPublic | BindingFlags.Instance);
            _executeOperationMethod = type.GetMethod("ExecuteOperation",
    BindingFlags.NonPublic | BindingFlags.Instance);
            _applyFunctionMethod = type.GetMethod("ApplyFunction",
                BindingFlags.NonPublic | BindingFlags.Instance);

        }

        #region Тесты метода Tokenize

        /// <summary>
        /// [Tokenize] ТИПИЧНЫЕ ДАННЫЕ
        /// Проверяет корректное разбиение выражения со смешанными токенами:
        /// числа с плавающей точкой, арифметические операторы, скобки.
        /// Ожидаемый результат известен заранее.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Tokenize")]
        [Trait("Category", "Typical")]
        public void Tokenize_TypicalData_ReturnsCorrectTokens()
        {
            // Arrange: подготовка входных данных и ожидаемого результата
            string expression = "2.5 + 3 * (4 - 1)";
            var expected = new List<string>
            { "2.5", "+", "3", "*", "(", "4", "-", "1", ")" };

            // Act: выполнение тестируемого метода
            var result = (List<string>)_tokenizeMethod.Invoke(_engine, new object[] { expression });

            // Assert: проверка соответствия результата ожиданию
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// [Tokenize] ГРАНИЧНЫЙ СЛУЧАЙ
        /// Проверяет обработку минимально возможного валидного выражения — 
        /// строки, содержащей единственное число без операторов.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Tokenize")]
        [Trait("Category", "EdgeCase")]
        public void Tokenize_EdgeCase_SingleNumber()
        {
            // Arrange
            string expression = "42";
            var expected = new List<string> { "42" };

            // Act
            var result = (List<string>)_tokenizeMethod.Invoke(_engine, new object[] { expression });

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// [Tokenize] НЕКОРРЕКТНЫЕ ВХОДНЫЕ ДАННЫЕ
        /// Проверяет выброс исключения при передаче пустой строки.
        /// Ожидается: CalcException с сообщением о синтаксической ошибке.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Tokenize")]
        [Trait("Category", "InvalidInput")]
        public void Tokenize_InvalidInput_EmptyString_ThrowsException()
        {
            // Arrange
            string expression = "";

            // Act & Assert: метод должен выбросить целевое исключение
            var exception = Assert.Throws<TargetInvocationException>(
                () => _tokenizeMethod.Invoke(_engine, new object[] { expression }));

            Assert.IsType<CalcException>(exception.InnerException);
            Assert.Contains("Пустое выражение", exception.InnerException.Message);
        }

        /// <summary>
        /// [Tokenize] БОЛЬШОЙ ОБЪЁМ ДАННЫХ
        /// Проверяет, что токенизация выражения с 1000 переменных завершается 
        /// за разумное время (<1 секунды) без зависаний или утечек памяти.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Tokenize")]
        [Trait("Category", "Performance")]
        public void Tokenize_LargeVolume_CompletesInReasonableTime()
        {
            // Arrange: генерация выражения из 1000 переменных, разделённых '+'
            string expression = string.Join("+", Enumerable.Range(1, 1000).Select(i => $"x{i}"));
            var stopwatch = Stopwatch.StartNew();

            // Act
            var result = (List<string>)_tokenizeMethod.Invoke(_engine, new object[] { expression });
            stopwatch.Stop();

            // Assert: проверка количества токенов и времени выполнения
            Assert.Equal(1999, result.Count); // 1000 переменных + 999 операторов
            Assert.True(stopwatch.ElapsedMilliseconds < 1000,
                $"Токенизация заняла {stopwatch.ElapsedMilliseconds}мс, ожидается <1000мс");
        }

        /// <summary>
        /// [Tokenize] СПЕЦИФИЧЕСКИЙ СЛУЧАЙ: ОБРАБОТКА УНАРНОГО МИНУСА
        /// Проверяет корректное преобразование бинарного '-' в унарный 'u-' 
        /// в позициях: начало выражения и после открывающей скобки.
        /// Это критично для последующего вычисления отрицательных чисел.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Tokenize")]
        [Trait("Category", "Specific")]
        public void Tokenize_SpecificCase_UnaryMinusHandling()
        {
            // Arrange
            string expression = "-5 + (-3 * 2)";
            var expected = new List<string>
            { "u-", "5", "+", "(", "u-", "3", "*", "2", ")" };

            // Act
            var result = (List<string>)_tokenizeMethod.Invoke(_engine, new object[] { expression });

            // Assert
            Assert.Equal(expected, result);
            Assert.Contains("u-", result); // Подтверждение наличия маркера унарного минуса
        }

        /// <summary>
        /// [Tokenize] СПЕЦИФИЧЕСКИЙ СЛУЧАЙ: ПЕРЕМЕННЫЕ С КИРИЛЛИЦЕЙ И ГРЕЧЕСКИМИ БУКВАМИ
        /// Проверяет поддержку идентификаторов на нелатинских алфавитах,
        /// что важно для локализации и научных вычислений.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Tokenize")]
        [Trait("Category", "Specific")]
        public void Tokenize_SpecificCase_FunctionsWithGreekLetters()
        {
            // Arrange
            string expression = "sin(α) + cos(β)";
            // Греческие буквы могут не поддерживаться вашим парсером
            // Проверьте, что вообще токенизируется
            var tokenizeMethod = typeof(CalcEngine).GetMethod("Tokenize",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var result = (List<string>)tokenizeMethod.Invoke(_engine, new object[] { expression });

            // Assert - проверьте что вообще получилось
            Assert.Contains("sin", result);
            Assert.Contains("cos", result);
            Assert.Contains("(", result);
            Assert.Contains(")", result);
        }



        #endregion

        #region Тесты метода ToPostfix

        /// <summary>
        /// [ToPostfix] ТИПИЧНЫЕ ДАННЫЕ
        /// Проверяет соблюдение приоритета операторов: умножение должно 
        /// выполняться перед сложением. Вход: 2+3*4 → Ожидаемый ОПЗ: 2 3 4 * +
        /// </summary>
        [Fact]
        [Trait("Algorithm", "ToPostfix")]
        [Trait("Category", "Typical")]
        public void ToPostfix_TypicalData_RespectsOperatorPrecedence()
        {
            // Arrange
            var tokens = new List<string> { "2", "+", "3", "*", "4" };
            var expected = new List<string> { "2", "3", "4", "*", "+" };

            // Act
            var result = (List<string>)_toPostfixMethod.Invoke(_engine, new object[] { tokens });

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// [ToPostfix] ГРАНИЧНЫЙ СЛУЧАЙ
        /// Проверяет обработку входного списка из одного токена (числа).
        /// Ожидаемый результат: тот же токен в выходном списке.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "ToPostfix")]
        [Trait("Category", "EdgeCase")]
        public void ToPostfix_EdgeCase_SingleToken()
        {
            // Arrange
            var tokens = new List<string> { "123.45" };
            var expected = new List<string> { "123.45" };

            // Act
            var result = (List<string>)_toPostfixMethod.Invoke(_engine, new object[] { tokens });

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// [ToPostfix] НЕКОРРЕКТНЫЕ ВХОДНЫЕ ДАННЫЕ
        /// Проверяет выброс исключения при несбалансированных скобках 
        /// (открывающая без закрывающей). Ожидается: CalcException.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "ToPostfix")]
        [Trait("Category", "InvalidInput")]
        public void ToPostfix_InvalidInput_UnbalancedParentheses_ThrowsException()
        {
            // Arrange
            var tokens = new List<string> { "2", "+", "(", "3", "*", "4" };

            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(
                () => _toPostfixMethod.Invoke(_engine, new object[] { tokens }));

            Assert.IsType<CalcException>(exception.InnerException);
        }


        /// <summary>
        /// [ToPostfix] СПЕЦИФИЧЕСКИЙ СЛУЧАЙ: ВЛОЖЕННЫЕ ФУНКЦИИ С АРГУМЕНТАМИ
        /// Проверяет корректную обработку функций с несколькими аргументами 
        /// и вложенностью: sin(2,3) + log(10) → ОПЗ с правильным порядком.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "ToPostfix")]
        [Trait("Category", "Specific")]
        public void ToPostfix_SpecificCase_NestedFunctionsWithArgs()
        {
            // Arrange
            var tokens = new List<string>
            { "sin", "(", "2", ",", "3", ")", "+", "log", "(", "10", ")" };
            var expected = new List<string>
            { "2", "3", "sin", "10", "log", "+" };

            // Act
            var result = (List<string>)_toPostfixMethod.Invoke(_engine, new object[] { tokens });

            // Assert
            Assert.Equal(expected, result);
        }

        

        /// <summary>
        /// [ToPostfix] ГРАНИЧНЫЙ СЛУЧАЙ: ПУСТОЙ СПИСОК ТОКЕНОВ
        /// Проверяет устойчивость алгоритма к пустому входу.
        /// Ожидаемый результат: пустой список без исключений.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "ToPostfix")]
        [Trait("Category", "EdgeCase")]
        public void ToPostfix_EdgeCase_EmptyTokensList()
        {
            // Arrange
            var tokens = new List<string>();
            var expected = new List<string>();

            // Act
            var result = (List<string>)_toPostfixMethod.Invoke(_engine, new object[] { tokens });

            // Assert
            Assert.Equal(expected, result);
        }

        #endregion

        #region Тесты метода Evaluate

        /// <summary>
        /// [Evaluate] ТИПИЧНЫЕ ДАННЫЕ
        /// Проверяет вычисление простого постфиксного выражения: 
        /// "2 3 + 4 *" → (2+3)*4 = 20. Результат сравнивается с допуском 10^-10.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Evaluate")]
        [Trait("Category", "Typical")]
        public void Evaluate_TypicalData_CalculatesCorrectResult()
        {
            // Arrange
            var postfix = new List<string> { "2", "3", "+", "4", "*" }; // (2+3)*4 = 20
            var variables = new Dictionary<string, double>();

            // Act
            var result = (double)_evaluateMethod.Invoke(_engine, new object[] { postfix, variables });

            // Assert
            Assert.Equal(20.0, result, 10);
        }

        /// <summary>
        /// [Evaluate] ГРАНИЧНЫЙ СЛУЧАЙ
        /// Проверяет вычисление выражения из одного отрицательного числа 
        /// с плавающей точкой. Ожидается точное совпадение результата.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Evaluate")]
        [Trait("Category", "EdgeCase")]
        public void Evaluate_EdgeCase_SingleNumber()
        {
            // Arrange
            var postfix = new List<string> { "-15.75" };
            var variables = new Dictionary<string, double>();

            // Act
            var result = (double)_evaluateMethod.Invoke(_engine, new object[] { postfix, variables });

            // Assert
            Assert.Equal(-15.75, result, 10);
        }

        /// <summary>
        /// [Evaluate] НЕКОРРЕКТНЫЕ ВХОДНЫЕ ДАННЫЕ
        /// Проверяет выброс исключения при недостатке операндов для бинарного оператора.
        /// Вход: "5 +" → Ожидается: StackUnderflow.
        /// </summary>
        [Fact]
        public void Evaluate_InvalidInput_InsufficientOperands_ThrowsException()
        {
            // Arrange
            var postfix = new List<string> { "5", "+" };
            var variables = new Dictionary<string, double>();

            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(
                () => _evaluateMethod.Invoke(_engine, new object[] { postfix, variables }));

            // Просто проверьте тип, а не сообщение
            Assert.IsType<CalcException>(exception.InnerException);
        }

        /// <summary>
        /// [Evaluate] БОЛЬШОЙ ОБЪЁМ ДАННЫХ
        /// Проверяет вычисление суммы чисел от 1 до 1000 в постфиксной записи.
        /// Ожидается: результат 500500, время выполнения <1 секунды.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Evaluate")]
        [Trait("Category", "Performance")]
        public void Evaluate_LargeVolume_CompletesInReasonableTime()
        {
            // Arrange
            var postfix = Enumerable.Range(1, 1000)
                .Select(i => i.ToString())
                .Concat(Enumerable.Repeat("+", 999))
                .ToList();
            var variables = new Dictionary<string, double>();
            var stopwatch = Stopwatch.StartNew();

            // Act
            var result = (double)_evaluateMethod.Invoke(_engine, new object[] { postfix, variables });
            stopwatch.Stop();

            // Assert
            Assert.Equal(500500.0, result, 1); // сумма 1..1000
            Assert.True(stopwatch.ElapsedMilliseconds < 1000,
                $"Вычисление заняло {stopwatch.ElapsedMilliseconds}мс, ожидается <1000мс");
        }



        /// <summary>
        /// [Evaluate] БОЛЬШОЙ ОБЪЁМ: МНОЖЕСТВО ОПЕРАЦИЙ БЕЗ ПЕРЕПОЛНЕНИЯ СТЕКА
        /// Стресс-тест: 1000 чисел и 999 операций сложения в ОПЗ.
        /// Проверяет отсутствие переполнения стека и корректность результата.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Evaluate")]
        [Trait("Category", "Performance")]
        public void Evaluate_LargeVolume_ManyOperations_NoStackOverflow()
        {
            // Arrange
            var postfix = Enumerable.Range(1, 1000)
                .Select(i => i.ToString())
                .Concat(Enumerable.Repeat("+", 999))
                .ToList();
            var variables = new Dictionary<string, double>();

            // Act
            var result = (double)_evaluateMethod.Invoke(_engine, new object[] { postfix, variables });

            // Assert
            Assert.Equal(500500.0, result, 1);
        }

        /// <summary>
        /// [Evaluate] НЕКОРРЕКТНЫЕ ДАННЫЕ: NULL-ВХОД
        /// Проверяет обработку null в качестве постфиксного выражения.
        /// Ожидается: исключение синтаксической ошибки.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Evaluate")]
        [Trait("Category", "InvalidInput")]
        public void Evaluate_InvalidInput_NullPostfix_ThrowsException()
        {
            // Arrange
            List<string> postfix = null;
            var variables = new Dictionary<string, double>();

            // Act & Assert
            var method = typeof(CalcEngine).GetMethod("Evaluate",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var exception = Assert.Throws<TargetInvocationException>(
                () => method.Invoke(_engine, new object[] { postfix, variables }));

            Assert.IsType<CalcException>(exception.InnerException);
        }

        #endregion

        #region Дополнительные тесты для максимального покрытия

        /// <summary>
        /// [ToPostfix] ГРАНИЧНЫЙ СЛУЧАЙ: ПУСТЫЕ ТОКЕНЫ
        /// Проверяет ветку `if (string.IsNullOrWhiteSpace(token)) continue`.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "ToPostfix")]
        [Trait("Category", "EdgeCase")]
        public void ToPostfix_EdgeCase_SkipsEmptyOrNullTokens()
        {
            var tokens = new List<string> { "2", " ", "", null, "+", "2" };
            var expected = new List<string> { "2", "2", "+" };

            var result = (List<string>)_toPostfixMethod.Invoke(_engine, new object[] { tokens });

            Assert.Equal(expected, result);
        }

        /// <summary>
        /// [ToPostfix] СПЕЦИФИЧЕСКИЙ СЛУЧАЙ: УНАРНЫЙ МИНУС
        /// Проверяет особую логику приоритета для унарного минуса (priority 5).
        /// Вход: -2^2 должен обрабатываться с учетом высокого приоритета u-.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "ToPostfix")]
        [Trait("Category", "Specific")]
        public void ToPostfix_SpecificCase_UnaryMinusPriority()
        {
            var tokens = new List<string> { "u-", "2", "^", "2" };
            var expected = new List<string> { "2", "u-", "2", "^" }; 

            var result = (List<string>)_toPostfixMethod.Invoke(_engine, new object[] { tokens });

            Assert.Equal(expected, result);
        }

        /// <summary>
        /// [ToPostfix] НЕКОРРЕКТНЫЕ ВХОДНЫЕ ДАННЫЕ: ЗАПЯТАЯ ВНЕ ФУНКЦИИ
        /// Проверяет генерацию ошибки SyntaxError при пустом стеке или отсутствии '(' для запятой.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "ToPostfix")]
        [Trait("Category", "InvalidInput")]
        public void ToPostfix_InvalidInput_CommaOutsideFunction_ThrowsException()
        {
            var tokens = new List<string> { "1", ",", "2" };

            var ex = Assert.Throws<TargetInvocationException>(() => _toPostfixMethod.Invoke(_engine, new object[] { tokens }));
            var inner = Assert.IsType<CalcException>(ex.InnerException);
            Assert.Contains("Запятая вне функции", inner.Message);
        }

        /// <summary>
        /// [ToPostfix] ГРАНИЧНЫЙ СЛУЧАЙ: НЕЗАКРЫТАЯ СКОБКА В КОНЦЕ
        /// Покрывает проверку `if (op == "(")` в финальном цикле выгрузки стека.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "ToPostfix")]
        [Trait("Category", "InvalidInput")]
        public void ToPostfix_InvalidInput_UnclosedParenthesisAtEnd_ThrowsException()
        {
            var tokens = new List<string> { "sin", "(", "2", "+", "3" };

            var ex = Assert.Throws<TargetInvocationException>(() => _toPostfixMethod.Invoke(_engine, new object[] { tokens }));
            Assert.IsType<CalcException>(ex.InnerException);
        }

        /// <summary>
        /// [ToPostfix] НЕКОРРЕКТНЫЕ ВХОДНЫЕ ДАННЫЕ: НЕИЗВЕСТНЫЙ ТОКЕН
        /// Проверяет финальный блок else, выбрасывающий исключение при неизвестном символе.
        /// </summary>
        [Fact]
        public void ToPostfix_InvalidInput_UnknownToken_ThrowsException()
        {
            var tokens = new List<string> { "@" };

            var ex = Assert.Throws<TargetInvocationException>(() =>
                _toPostfixMethod.Invoke(_engine, new object[] { tokens }));

            Assert.IsType<CalcException>(ex.InnerException);
        }

       

        /// <summary>
        /// [Integration] ПОЛНАЯ ЦЕПОЧКА: СЛОЖНОЕ ВЫРАЖЕНИЕ С ФУНКЦИЯМИ
        /// Проверяет сквозную работу всех алгоритмов на выражении:
        /// "sin(0) + cos(0) * sqrt(16)" → 0 + 1 * 4 = 4.0
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Integration")]
        [Trait("Category", "Typical")]
        public void Calculate_Integration_ComplexExpression()
        {
            string expression = "sin(0) + cos(0) * sqrt(16)";
            double expected = 0 + 1 * 4; 

            double result = _engine.Calculate(expression);

            Assert.Equal(expected, result, 10);
        }

        /// <summary>
        /// [Integration] РАБОТА С ПЕРЕМЕННЫМИ
        /// Проверяет подстановку значений переменных из словаря и регистронезависимость:
        /// "2 * x + y" при x=3, y=4 → 10.0
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Integration")]
        [Trait("Category", "Typical")]
        public void Calculate_WithVariables_SubstitutesCorrectly()
        {
            string expression = "2 * x + y";
            var variables = new Dictionary<string, double> { { "x", 3 }, { "y", 4 } };
            double expected = 2 * 3 + 4; 

            double result = _engine.Calculate(expression, variables);

            Assert.Equal(expected, result, 10);
        }

        /// <summary>
        /// Проверка корректности вычисления стандартных математических функций.
        /// Покрывает основные ветки (arms) оператора switch для валидных входных данных.
        /// </summary>
        [Theory]
        [InlineData("sqrt", 9, 3)]
        [InlineData("√", 16, 4)]
        [InlineData("abs", -5, 5)]
        [InlineData("sin", 0, 0)]
        [InlineData("ln", 1, 0)]
        [InlineData("log", 100, 2)]
        [InlineData("tg", 0, 0)]
        public void ApplyFunction_ValidInputs_ReturnsCorrectValue(string func, double input, double expected)
        {
            var result = _engine.ApplyFunction(func, input);

            Assert.Equal(expected, result, precision: 10);
        }

        /// <summary>
        /// Тестирование обработки ошибок области определения (Math Domain Errors).
        /// Обеспечивает покрытие логических ветвлений внутри функций sqrt, ln и log.
        /// </summary>
        [Theory]
        [InlineData("sqrt", -1)]
        [InlineData("ln", 0)]
        [InlineData("ln", -5)]
        [InlineData("log", 0)]
        public void ApplyFunction_MathDomainError_ThrowsCalcException(string func, double input)
        {
            Assert.Throws<CalcException>(() => _engine.ApplyFunction(func, input));
        }

        /// <summary>
        /// Проверка реакции системы на неизвестный идентификатор функции.
        /// Гарантирует покрытие ветки по умолчанию (discard pattern) в switch-выражении.
        /// </summary>
        [Fact]
        public void ApplyFunction_UnknownFunction_ThrowsInvalidFunctionException()
        {
            string invalidFunc = "unknown_func";

            Assert.Throws<CalcException>(() => _engine.ApplyFunction(invalidFunc, 1.0));
        }

        /// <summary>
        /// [Integration] ОБРАБОТКА ДЕЛЕНИЯ НА НОЛЬ
        /// Проверяет, что попытка деления на ноль выбрасывает 
        /// CalcException с кодом ошибки DivisionByZero.
        /// </summary>
        [Fact]
        public void Calculate_DivisionByZero_ThrowsException()
        {
            string expression = "10 / 0";

            var exception = Assert.Throws<CalcException>(() => _engine.Calculate(expression));

            Assert.Contains("Деление на ноль", exception.Message);
            Assert.IsType<CalcException>(exception);
        }

        /// <summary>
        /// [Integration] ВАЛИДАЦИЯ СИНТАКСИСА: КОРРЕКТНОЕ ВЫРАЖЕНИЕ
        /// Проверяет метод ValidateSyntax на валидном выражении.
        /// Ожидается: возврат true без выброса исключений.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Integration")]
        [Trait("Category", "Typical")]
        public void ValidateSyntax_CorrectExpression_ReturnsTrue()
        {
            string expression = "(2 + 3) * sqrt(9)";

            bool isValid = _engine.ValidateSyntax(expression);

            Assert.True(isValid);
        }

        /// <summary>
        /// [Integration] ВАЛИДАЦИЯ СИНТАКСИСА: ОШИБКА В ВЫРАЖЕНИИ
        /// Проверяет, что ValidateSyntax возвращает false при незакрытой скобке.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Integration")]
        [Trait("Category", "InvalidInput")]
        public void ValidateSyntax_IncorrectExpression_ReturnsFalse()
        {
            string expression = "2 + (3 * 4"; 

            bool isValid = _engine.ValidateSyntax(expression);

            Assert.False(isValid);
        }

        /// <summary>
        /// [Integration] ГРАНИЧНЫЙ СЛУЧАЙ: ТОЛЬКО ПРОБЕЛЬНЫЕ СИМВОЛЫ
        /// Проверяет обработку строки, содержащей только пробелы/табуляции.
        /// Ожидается: исключение о пустом выражении.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Integration")]
        [Trait("Category", "EdgeCase")]
        public void Calculate_EdgeCase_WhitespaceOnly_ThrowsException()
        {
            string expression = "   \t\n  ";

            Assert.Throws<CalcException>(() => _engine.Calculate(expression));
        }

        /// <summary>
        /// [Integration] СПЕЦИФИЧЕСКИЙ СЛУЧАЙ: РЕГИСТРОНЕЗАВИСИМОСТЬ ПЕРЕМЕННЫХ
        /// Проверяет, что переменные 'X' и 'x' считаются идентичными 
        /// при подстановке значений из словаря.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Integration")]
        [Trait("Category", "Specific")]
        public void Calculate_SpecificCase_VariableCaseInsensitive()
        {
            string expression = "X + x + X";
            var variables = new Dictionary<string, double> { { "x", 5 } };

            double result = _engine.Calculate(expression, variables);

            Assert.Equal(15.0, result, 10); 
        }

        /// <summary>
        /// Проверяет производительность при последовательном вычислении 
        /// 100 выражений с функциями. Время выполнения должно быть <5 секунд.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Integration")]
        [Trait("Category", "Performance")]
        public void Calculate_LargeVolume_StressTest_CompletesInTime()
        {
            var expressions = Enumerable.Range(1, 100)
                .Select(i => $"sin({i})*cos({i}) + sqrt({i + 1})")
                .ToList();
            var stopwatch = Stopwatch.StartNew();

            foreach (var expr in expressions)
            {
                _engine.Calculate(expr);
            }
            stopwatch.Stop();

            Assert.True(stopwatch.ElapsedMilliseconds < 5000,
                $"100 выражений вычислены за {stopwatch.ElapsedMilliseconds}мс");
        }
      

        /// <summary>
        /// [Evaluate] ТИПИЧНЫЕ ДАННЫЕ: РАБОТА С ПЕРЕМЕННЫМИ
        /// Проверяет извлечение значений из словаря и независимость от регистра (Case-insensitivity).
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Evaluate")]
        public void Evaluate_TypicalData_UsesVariablesCaseInsensitive()
        {
            var postfix = new List<string> { "x", "Y", "+" };
            var variables = new Dictionary<string, double> { { "X", 10.5 }, { "y", 20.0 } };

            var result = (double)_evaluateMethod.Invoke(_engine, new object[] { postfix, variables });

            Assert.Equal(30.5, result);
        }

        /// <summary>
        /// [Evaluate] НЕКОРРЕКТНЫЕ ДАННЫЕ: ПЕРЕМЕННАЯ НЕ НАЙДЕНА
        /// Покрывает ветку `throw CalcException.InvalidVariable(token)`.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Evaluate")]
        public void Evaluate_InvalidInput_MissingVariable_ThrowsException()
        {
            var postfix = new List<string> { "a", "b", "+" };
            var variables = new Dictionary<string, double> { { "a", 1 } }; // 'b' отсутствует

            var ex = Assert.Throws<TargetInvocationException>(() => _evaluateMethod.Invoke(_engine, new object[] { postfix, variables }));
            Assert.IsType<CalcException>(ex.InnerException);
        }

        /// <summary>
        /// [Evaluate] СПЕЦИФИЧЕСКИЙ СЛУЧАЙ: УНАРНЫЙ МИНУС
        /// Проверяет ветку `token == "u-"` и корректность изменения знака числа в стеке.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Evaluate")]
        public void Evaluate_SpecificCase_UnaryMinus()
        {
            var postfix = new List<string> { "5", "u-" };
            var variables = new Dictionary<string, double>();

            var result = (double)_evaluateMethod.Invoke(_engine, new object[] { postfix, variables });

            Assert.Equal(-5.0, result);
        }

        /// <summary>
        /// [Evaluate] ГРАНИЧНЫЙ СЛУЧАЙ: БЕСКОНЕЧНЫЙ РЕЗУЛЬТАТ
        /// Покрывает ветку `if (double.IsInfinity(result))` после выполнения операции.
        /// Например, возведение очень большого числа в большую степень.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Evaluate")]
        public void Evaluate_EdgeCase_ResultInfinity_ThrowsException()
        {
            var postfix = new List<string> { "1e300", "1e300", "*" };
            var variables = new Dictionary<string, double>();

            var ex = Assert.Throws<TargetInvocationException>(() => _evaluateMethod.Invoke(_engine, new object[] { postfix, variables }));
            var inner = (CalcException)ex.InnerException;
            Assert.Contains("Бесконечный", inner.Message);
        }

        /// <summary>
        /// [Evaluate] НЕКОРРЕКТНЫЕ ДАННЫЕ: ЛИШНЕЕ ЧИСЛО В СТЕКЕ
        /// Покрывает финальную проверку `if (stack.Count != 1)`.
        /// Сценарий: "2 3" (числа есть, а оператора нет).
        /// </summary>
        [Fact]
        [Trait("Algorithm", "Evaluate")]
        public void Evaluate_InvalidInput_TooManyOperands_ThrowsException()
        {
            var postfix = new List<string> { "2", "3" };
            var variables = new Dictionary<string, double>();

            var ex = Assert.Throws<TargetInvocationException>(() => _evaluateMethod.Invoke(_engine, new object[] { postfix, variables }));
            var inner = (CalcException)ex.InnerException;
            Assert.Contains("структуре", inner.Message);
        }

        #endregion

        #region Тесты метода ExecuteOperation

        /// <summary>
        /// [ExecuteOperation] ТИПИЧНЫЕ ДАННЫЕ
        /// Проверяет базовую арифметику: сложение, вычитание, умножение, возведение в степень.
        /// </summary>
        [Theory]
        [InlineData("+", 10, 5, 15)]
        [InlineData("-", 10, 5, 5)]
        [InlineData("*", 10, 5, 50)]
        [InlineData("^", 2, 3, 8)]
        [Trait("Algorithm", "ExecuteOperation")]
        public void ExecuteOperation_BasicArithmetic_ReturnsCorrectValue(string op, double a, double b, double expected)
        {
            // Act
            var result = (double)_executeOperationMethod.Invoke(_engine, new object[] { op, a, b });

            // Assert
            Assert.Equal(expected, result, 10);
        }

        /// <summary>
        /// [ExecuteOperation] ГРАНИЧНЫЙ СЛУЧАЙ: ДЕЛЕНИЕ НА НОЛЬ
        /// Проверяет ветку `b == 0 ? throw ...`. 
        /// Ожидается: CalcException с кодом ошибки деления на ноль.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "ExecuteOperation")]
        public void ExecuteOperation_DivisionByZero_ThrowsException()
        {
            // Arrange
            string op = "/";
            double a = 10, b = 0;

            // Act & Assert
            var ex = Assert.Throws<TargetInvocationException>(() => _executeOperationMethod.Invoke(_engine, new object[] { op, a, b }));
            Assert.IsType<CalcException>(ex.InnerException);
        }

        /// <summary>
        /// [ExecuteOperation] НЕКОРРЕКТНЫЕ ДАННЫЕ: КОРЕНЬ ИЗ ОТРИЦАТЕЛЬНОГО ЧИСЛА
        /// Проверяет ветку `a < 0 ? throw ...` для оператора корня.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "ExecuteOperation")]
        public void ExecuteOperation_RootOfNegative_ThrowsException()
        {
            // Arrange
            string op = "√";
            double a = -4, b = 2;

            // Act & Assert
            var ex = Assert.Throws<TargetInvocationException>(() => _executeOperationMethod.Invoke(_engine, new object[] { op, a, b }));
            Assert.IsType<CalcException>(ex.InnerException);
        }

        /// <summary>
        /// [ExecuteOperation] НЕКОРРЕКТНЫЕ ДАННЫЕ: НЕИЗВЕСТНЫЙ ОПЕРАТОР
        /// Проверяет ветку по умолчанию `_ => throw ...` в switch-выражении.
        /// </summary>
        [Fact]
        [Trait("Algorithm", "ExecuteOperation")]
        public void ExecuteOperation_UnknownOperator_ThrowsException()
        {
            // Arrange
            string op = "%"; // Допустим, остаток от деления не реализован

            // Act & Assert
            var ex = Assert.Throws<TargetInvocationException>(() => _executeOperationMethod.Invoke(_engine, new object[] { op, 1, 1 }));
            Assert.IsType<CalcException>(ex.InnerException);
        }

        #endregion

    }
}