using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace CalcLib.Tests
{
    /// <summary>
    /// Тестовый класс для проверки функциональности менеджера истории вычислений CalcHistory.
    /// Тестирует корректность добавления записей, ограничение размера, 
    /// возврат копий данных и очистку истории.
    /// </summary>
    public class CalcHistoryTests
    {

        #region Тесты метода Add()

        /// <summary>
        /// [Add] ТИПИЧНЫЕ ДАННЫЕ
        /// Проверяет корректное форматирование записи: выражение и результат 
        /// объединяются в строку вида "выражение = результат".
        /// </summary>
        [Fact]
        [Trait("Method", "Add")]
        [Trait("Category", "Typical")]
        public void Add_TypicalData_FormatsEntryCorrectly()
        {
            // Arrange
            var history = new CalcHistory();
            string expression = "2 + 2";
            double result = 4.0;
            string expected = "2 + 2 = 4";

            // Act
            history.Add(expression, result);
            var entries = history.GetAll();

            // Assert
            Assert.Single(entries);
            Assert.Equal(expected, entries[0]);
        }

        /// <summary>
        /// [Add] ГРАНИЧНЫЙ СЛУЧАЙ
        /// Проверяет обработку результатов с особыми значениями: ноль и отрицательное число.
        /// Важно для корректного отображения в истории.
        /// </summary>
        [Fact]
        [Trait("Method", "Add")]
        [Trait("Category", "EdgeCase")]
        public void Add_EdgeCase_ZeroAndNegativeResults()
        {
            // Arrange
            var history = new CalcHistory();

            // Act
            history.Add("5 - 5", 0.0);
            history.Add("3 - 10", -7.0);
            var entries = history.GetAll();

            // Assert
            Assert.Equal(2, entries.Count);
            Assert.Contains("5 - 5 = 0", entries);
            Assert.Contains("3 - 10 = -7", entries);
        }



        /// <summary>
        /// [Add] БОЛЬШОЙ ОБЪЁМ ДАННЫХ
        /// Проверяет работу механизма ограничения размера: при добавлении записей 
        /// сверх лимита (maxItems=5) старые записи автоматически удаляются (FIFO).
        /// </summary>
        [Fact]
        [Trait("Method", "Add")]
        [Trait("Category", "Performance")]
        public void Add_LargeVolume_ExceedsMaxLimit_RemovesOldest()
        {
            // Arrange
            var history = new CalcHistory(maxItems: 5);
            var stopwatch = Stopwatch.StartNew();

            // Act: добавляем 10 записей в историю с лимитом 5
            for (int i = 1; i <= 10; i++)
            {
                history.Add($"expr{i}", i);
            }
            stopwatch.Stop();
            var entries = history.GetAll();

            // Assert
            Assert.Equal(5, entries.Count);
            Assert.DoesNotContain("expr1 = 1", entries); // Старейшая удалена
            Assert.Contains("expr6 = 6", entries);        // Самая старая из оставшихся
            Assert.Contains("expr10 = 10", entries);      // Последняя добавлена
            Assert.True(stopwatch.ElapsedMilliseconds < 100);
        }

        #endregion

        #region Тесты метода GetAll()

        /// <summary>
        /// [GetAll] ТИПИЧНЫЕ ДАННЫЕ
        /// Проверяет, что метод возвращает защитную копию списка, а не ссылку на внутреннее поле.
        /// Модификация возвращённого списка не должна влиять на состояние объекта.
        /// </summary>
        [Fact]
        [Trait("Method", "GetAll")]
        [Trait("Category", "Typical")]
        public void GetAll_TypicalData_ReturnsDefensiveCopy()
        {
            // Arrange
            var history = new CalcHistory();
            history.Add("2+2", 4);

            // Act
            var list1 = history.GetAll();
            var list2 = history.GetAll();
            list1.Add("MODIFIED");

            // Assert
            Assert.Equal(1, list2.Count);        // Оригинальная копия не изменена
            Assert.DoesNotContain("MODIFIED", list2);
            Assert.NotSame(list1, list2);         // Разные экземпляры
        }

        /// <summary>
        /// [GetAll] ГРАНИЧНЫЙ СЛУЧАЙ
        /// Проверяет возврат непустого объекта списка при пустой истории.
        /// Важно для предотвращения NullReferenceException у клиента.
        /// </summary>
        [Fact]
        [Trait("Method", "GetAll")]
        [Trait("Category", "EdgeCase")]
        public void GetAll_EdgeCase_EmptyHistory_ReturnsEmptyList()
        {
            // Arrange
            var history = new CalcHistory();

            // Act
            var entries = history.GetAll();

            // Assert
            Assert.NotNull(entries);
            Assert.Empty(entries);
            Assert.IsType<List<string>>(entries);
        }

        /// <summary>
        /// [GetAll] НЕКОРРЕКТНЫЕ ДАННЫЕ (ПОВЕДЕНЧЕСКИЙ ТЕСТ)
        /// Проверяет, что последовательные вызовы GetAll возвращают независимые копии,
        /// отражающие актуальное состояние истории на момент вызова.
        /// </summary>
        [Fact]
        [Trait("Method", "GetAll")]
        [Trait("Category", "InvalidInput")]
        public void GetAll_InvalidInput_MultipleCallsConsistent()
        {
            // Arrange
            var history = new CalcHistory();
            history.Add("test", 1);

            // Act
            var result1 = history.GetAll();
            history.Add("test2", 2);
            var result2 = history.GetAll();

            // Assert
            Assert.Equal(1, result1.Count);
            Assert.Equal(2, result2.Count);
            Assert.DoesNotContain("test2 = 2", result1); // Первый вызов не видит второе добавление
        }

        /// <summary>
        /// [GetAll] БОЛЬШОЙ ОБЪЁМ ДАННЫХ
        /// Проверяет производительность возврата истории из 1000 записей.
        /// Время выполнения должно быть <50 мс.
        /// </summary>
        [Fact]
        [Trait("Method", "GetAll")]
        [Trait("Category", "Performance")]
        public void GetAll_LargeVolume_1000Items_CompletesFast()
        {
            // Arrange
            var history = new CalcHistory(maxItems: 1000);
            for (int i = 0; i < 1000; i++)
                history.Add($"expr{i}", i);
            var stopwatch = Stopwatch.StartNew();

            // Act
            var entries = history.GetAll();
            stopwatch.Stop();

            // Assert
            Assert.Equal(1000, entries.Count);
            Assert.True(stopwatch.ElapsedMilliseconds < 50);
            Assert.Equal("expr999 = 999", entries.Last());
        }

        /// <summary>
        /// [GetAll] СПЕЦИФИЧЕСКИЙ СЛУЧАЙ
        /// Проверяет сохранение порядка записей по принципу FIFO (первый добавлен — первый в списке).
        /// Критично для корректного отображения истории пользователю.
        /// </summary>
        [Fact]
        [Trait("Method", "GetAll")]
        [Trait("Category", "Specific")]
        public void GetAll_SpecificCase_OrderPreserved_FIFO()
        {
            // Arrange
            var history = new CalcHistory();
            var expectedOrder = new[] { "A", "B", "C", "D", "E" };

            // Act
            foreach (var item in expectedOrder)
                history.Add(item, 1);
            var entries = history.GetAll();

            // Assert
            for (int i = 0; i < expectedOrder.Length; i++)
            {
                Assert.StartsWith(expectedOrder[i], entries[i]);
            }
        }

        #endregion

        #region Тесты метода Clear() и свойства Count

        /// <summary>
        /// [Clear] ТИПИЧНЫЕ ДАННЫЕ
        /// Проверяет, что вызов Clear() полностью очищает историю 
        /// и сбрасывает счётчик записей в ноль.
        /// </summary>
        [Fact]
        [Trait("Method", "Clear")]
        [Trait("Category", "Typical")]
        public void Clear_TypicalData_EmptiesHistory()
        {
            // Arrange
            var history = new CalcHistory();
            history.Add("1+1", 2);
            history.Add("2+2", 4);

            // Act
            history.Clear();
            var entries = history.GetAll();

            // Assert
            Assert.Empty(entries);
            Assert.Equal(0, history.Count);
        }

        /// <summary>
        /// [Clear] ГРАНИЧНЫЙ СЛУЧАЙ
        /// Проверяет, что очистка уже пустой истории не вызывает исключений 
        /// и оставляет объект в валидном состоянии.
        /// </summary>
        [Fact]
        [Trait("Method", "Clear")]
        [Trait("Category", "EdgeCase")]
        public void Clear_EdgeCase_AlreadyEmpty_NoException()
        {
            // Arrange
            var history = new CalcHistory();

            // Act & Assert
            var exception = Record.Exception(() => history.Clear());
            Assert.Null(exception);
            Assert.Equal(0, history.Count);
        }

        /// <summary>
        /// [Clear] НЕКОРРЕКТНЫЕ ДАННЫЕ (ИДЕМПОТЕНТНОСТЬ)
        /// Проверяет, что многократные вызовы Clear() безопасны 
        /// и не приводят к ошибкам или неопределённому поведению.
        /// </summary>
        [Fact]
        [Trait("Method", "Clear")]
        [Trait("Category", "InvalidInput")]
        public void Clear_InvalidInput_MultipleCalls_Idempotent()
        {
            // Arrange
            var history = new CalcHistory();
            history.Add("test", 1);

            // Act
            history.Clear();
            history.Clear();
            history.Clear();

            // Assert
            Assert.Equal(0, history.Count);
            Assert.Empty(history.GetAll());
        }

        /// <summary>
        /// [Clear] БОЛЬШОЙ ОБЪЁМ ДАННЫХ
        /// Проверяет производительность очистки истории из 10000 записей.
        /// Время выполнения должно быть <100 мс.
        /// </summary>
        [Fact]
        [Trait("Method", "Clear")]
        [Trait("Category", "Performance")]
        public void Clear_LargeVolume_10000Items_CompletesFast()
        {
            // Arrange
            var history = new CalcHistory(maxItems: 10000);
            for (int i = 0; i < 10000; i++)
                history.Add($"expr{i}", i);
            var stopwatch = Stopwatch.StartNew();

            // Act
            history.Clear();
            stopwatch.Stop();

            // Assert
            Assert.Equal(0, history.Count);
            Assert.True(stopwatch.ElapsedMilliseconds < 100);
        }

        /// <summary>
        /// [Clear] СПЕЦИФИЧЕСКИЙ СЛУЧАЙ
        /// Проверяет возможность повторного использования истории после очистки:
        /// новые записи добавляются корректно, старые не восстанавливаются.
        /// </summary>
        [Fact]
        [Trait("Method", "Clear")]
        [Trait("Category", "Specific")]
        public void Clear_SpecificCase_ReuseAfterClear_WorksCorrectly()
        {
            // Arrange
            var history = new CalcHistory(maxItems: 3);

            // Act: цикл "добавить-очистить-добавить"
            history.Add("old1", 1);
            history.Add("old2", 2);
            history.Clear();
            history.Add("new1", 10);
            history.Add("new2", 20);
            history.Add("new3", 30);
            history.Add("new4", 40); // Должно вытеснить new1
            var entries = history.GetAll();

            // Assert
            Assert.Equal(3, entries.Count);
            Assert.DoesNotContain("old1 = 1", entries);
            Assert.DoesNotContain("new1 = 10", entries);
            Assert.Contains("new4 = 40", entries);
        }

        /// <summary>
        /// [Count] ТИПИЧНЫЕ ДАННЫЕ
        /// Проверяет, что свойство Count всегда отражает актуальное количество 
        /// записей в истории после добавления и удаления.
        /// </summary>
        [Fact]
        [Trait("Property", "Count")]
        [Trait("Category", "Typical")]
        public void Count_TypicalData_ReflectsActualItems()
        {
            // Arrange
            var history = new CalcHistory();

            // Act & Assert
            Assert.Equal(0, history.Count);
            history.Add("a", 1);
            Assert.Equal(1, history.Count);
            history.Add("b", 2);
            Assert.Equal(2, history.Count);
        }

        /// <summary>
        /// [Constructor] ГРАНИЧНЫЙ СЛУЧАЙ: ЛИМИТ 0
        /// Проверяет поведение при создании истории с maxItems=0:
        /// записи добавляются, но немедленно удаляются из-за лимита.
        /// </summary>
        [Fact]
        [Trait("Method", "Constructor")]
        [Trait("Category", "EdgeCase")]
        public void Constructor_EdgeCase_ZeroMaxItems_AllowsAddsButRemovesImmediately()
        {
            // Arrange & Act
            var history = new CalcHistory(maxItems: 0);
            history.Add("test", 1);

            // Assert: при лимите 0 запись сразу удаляется
            Assert.Equal(0, history.Count);
            Assert.Empty(history.GetAll());
        }

        #endregion
    }
}