namespace Parser
{
    public partial class Form1 : Form
    {
        string text = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "tiny language program file";
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "Text|*.txt";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                text = File.ReadAllText(fdlg.FileName);
                textBox1.Text = text;
            }
            else
            {
                //error
            }
             

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}