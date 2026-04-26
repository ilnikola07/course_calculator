using System;
using System.Collections.Generic;
namespace CalcLib
{
    /// <summary>
    /// Менеджер истории вычислений
    /// </summary>
    public class CalcHistory
    {
        private readonly List<string> _history = new List<string>();
        private readonly int _maxHistoryItems;
        public CalcHistory(int maxItems = 100)
        {
            _maxHistoryItems = maxItems;
        }
        /// <summary>
        /// Добавляет запись в историю
        /// </summary>
        public void Add(string expression, double result)
        {
            var entry = $"{expression} = {result}";
            _history.Add(entry);
            if (_history.Count > _maxHistoryItems)
            {
                _history.RemoveAt(0);
            }
        }
        /// <summary>
        /// Возвращает все записи истории
        /// </summary>
        public List<string> GetAll()
        {
            return new List<string>(_history);
        }
        /// <summary>
        /// Очищает историю
        /// </summary>
        public void Clear()
        {
            _history.Clear();
        }
        /// <summary>
        /// Количество записей в истории
        /// </summary>
        public int Count
        {
            get { return _history.Count; }
        }
    }
}