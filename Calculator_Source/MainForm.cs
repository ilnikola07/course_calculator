using Course_calculator;

namespace course_calculator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnRef_Click(object sender, EventArgs e)
        {
            FormAbout about = new FormAbout();
            about.Show();
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            FormHistory history = new FormHistory();
            history.Show();
        }
    }
}
