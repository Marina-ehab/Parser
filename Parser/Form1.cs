using DotNetGraph;
using DotNetGraph.Extensions;
using GraphVizNet;
using System.Net.Http.Headers;

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
            Parser parser = new Parser(textBox1.Text);
            DotGraph graph = new DotGraph("my Graph");
            Visualize.graphBuilder(parser.parseTree, ref graph);
            var dot = graph.Compile(true);
            File.WriteAllText("myFile.dot", dot); //remove later
            var graphViz = new GraphViz();
            if (File.Exists("..//image2.png"))
            {
                File.Delete("..//image2.png");
            }
            graphViz.LayoutAndRenderDotGraph(dot, "..//image2.png", "png");

            Form imageForm = new Form();
            PictureBox pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.Image = Image.FromFile("image2.png");
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            imageForm.Controls.Add(pictureBox);
            imageForm.Size = new Size(1000, 700);
            imageForm.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}