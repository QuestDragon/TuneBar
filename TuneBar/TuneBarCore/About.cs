using System.Reflection;

namespace TuneBar
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            textBox1.Text = Assembly.GetExecutingAssembly().GetName().Name;
            textBox2.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            textBox3.Text = "QuestDragon";
            textBox3.SelectionStart = 0; //選択状態解除
        }
    }
}
