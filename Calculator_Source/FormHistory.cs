using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Course_calculator
{
    public partial class FormHistory : Form
    {
        /// <summary>
        /// Выбранное выражение из истории (возвращается при двойном клике)
        /// </summary>
        public string SelectedExpression { get; private set; }

        public FormHistory()
        {
            InitializeComponent();
            InitializeCustomLogic();
        }

        public FormHistory(List<string> historyItems) : this()
        {
            LoadHistoryItems(historyItems);
        }

        private void InitializeCustomLogic()
        {
            // Подписка на события, если не сделана в дизайнере
            listBoxHist.DoubleClick -= ListBoxHist_DoubleClick;
            listBoxHist.DoubleClick += ListBoxHist_DoubleClick;
        }

        private void LoadHistoryItems(List<string> items)
        {
            if (items == null || items.Count == 0)
            {
                listBoxHist.Items.Add("История пуста");
                listBoxHist.Enabled = false;
                return;
            }

            // Добавляем элементы в обратном порядке (новые сверху)
            for (int i = items.Count - 1; i >= 0; i--)
            {
                listBoxHist.Items.Add(items[i]);
            }
        }

        private void ListBoxHist_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxHist.SelectedItem is string fullLine && fullLine.Contains("="))
            {
                // Извлекаем выражение до знака "="
                SelectedExpression = fullLine.Split('=')[0].Trim();
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnClearHistory_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Очистить историю вычислений?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                listBoxHist.Items.Clear();
                // Здесь можно добавить событие для очистки в MainForm
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}