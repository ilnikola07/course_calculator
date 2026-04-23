using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Course_calculator
{
    public partial class FormHistory : Form
    {
        public FormHistory() // Этот оставляем для дизайнера
        {
            InitializeComponent();
        }


        // Свойство, которое прочитает главная форма
        public string SelectedExpression { get; private set; }

        public FormHistory(List<string> data) : this()
        {
            if (data != null)
            {
                listBoxHist.Items.AddRange(data.ToArray());
            }

            // Подписываемся на двойной клик (можно сделать и в дизайнере)
            listBoxHist.DoubleClick += ListBoxHist_DoubleClick;
        }

        private void ListBoxHist_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxHist.SelectedItem != null)
            {
                // Получаем строку вида "5*6 = 30"
                string fullLine = listBoxHist.SelectedItem.ToString();

                // Забираем только то, что до знака "="
                if (fullLine.Contains("="))
                {
                    SelectedExpression = fullLine.Split('=')[0].Trim();
                    this.DialogResult = DialogResult.OK; // Закрываем форму с успешным результатом
                    this.Close();
                }
            }
        }
    }
}
