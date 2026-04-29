using System;
using System.Collections.Generic;
namespace CalcLib
{
    /// <summary>
    /// Менеджер истории вычислений
    /// </summary>
    public class CalcHistory
    {
        private readonly List<string> _history = new List<string>(); // Список строк, где хранятся записи вида "2+2=4"
        private readonly int _maxHistoryItems; // Максимум записей в истории

        public CalcHistory(int maxItems = 100)
        {
            _maxHistoryItems = maxItems;
        }

        /// <summary>
        /// Добавляет запись в историю
        /// </summary>
        public void Add(string expression, double result)
        {
            var entry = $"{expression} = {result}"; // Числа и выражение объединяются в одну строку 
            _history.Add(entry); // И добавляются далее
            if (_history.Count > _maxHistoryItems)
            {
                _history.RemoveAt(0); // Если слишком много записей - удаляет самую старую
            }
        }

        /// <summary>
        /// Возвращает все записи истории
        /// </summary>
        public List<string> GetAll()
        {
            return new List<string>(_history); // Метод не отдает сам список _history, а создает его копию
        }

        /// <summary>
        /// Очищает историю
        /// </summary>
        public void Clear()
        {
            _history.Clear(); // Oбнуляет историю
        }

        /// <summary>
        /// Количество записей в истории
        /// </summary>
        public int Count
        {
            get // Свойство, которое позволяет быстро узнать, сколько сейчас записей в базе
            { 
                return _history.Count;
            }
        }
    }
}