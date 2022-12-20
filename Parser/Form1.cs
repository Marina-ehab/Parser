using DotNetGraph;
using DotNetGraph.Extensions;
using GraphVizNet;
using System.Diagnostics.Metrics;
using System.Net.Http.Headers;

namespace Parser
{
    public partial class Form1 : Form
    {
        string text = "";
        static int counter = 0;

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
            try
            {
                textBox2.Text = "";
                if(textBox1.Text == "")
                {
                    throw new Exception("Please enter a tiny language valid program");
                }

                Scanner scanner = new Scanner(textBox1.Text);
                String scannerOutput = "";

                Token top = scanner.GetNextToken();
                while (top is not null)
                {
                    scannerOutput += top.tokenValue + ", " + top.printText;
                    scannerOutput += "\r\n";
                    top = scanner.GetNextToken();
                }
                textBox2.Text = scannerOutput;

                label2.Text = "";
                Parser parser = new Parser(textBox1.Text);
                DotGraph graph = new DotGraph("my Graph");
                Visualize.graphBuilder(parser.parseTree, ref graph);
                var dot = graph.Compile(true);
                var graphViz = new GraphViz();
                string fileName = "image" + counter++ + ".png";
                if (File.Exists("..//" + fileName))
                {
                    File.Delete("..//" + fileName);
                }
                graphViz.LayoutAndRenderDotGraph(dot, "..//" + fileName, "png");


                Form imageForm = new Form();
                PictureBox pictureBox = new PictureBox();
                pictureBox.Dock = DockStyle.Fill;
                pictureBox.Image = Image.FromFile(fileName);
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                imageForm.Controls.Add(pictureBox);
                imageForm.Size = new Size(1000, 700);
                imageForm.ShowDialog();

              

            }
            catch (Exception ex)
            {
                label2.Text = ex.Message.ToString();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}