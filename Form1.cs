using AForge.Video;
using AForge.Video.DirectShow;

namespace DotVideo
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection Fic;

        public Form1()
        {
            InitializeComponent();
            Fic = new(FilterCategory.VideoInputDevice);
        }

        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Refresh();
        }

        private void UpdateVideoDevices()
        {
            Fic = new(FilterCategory.VideoInputDevice);

            comboBox1.Items.Clear();

            foreach (FilterInfo dev in Fic)
            {
                comboBox1.Items.Add(dev.Name);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateVideoDevices();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VideoCaptureDevice vcd;
            vcd = new(Fic[comboBox1.SelectedIndex].MonikerString);
            vcd.NewFrame += FinalFrame_NewFrame;
            vcd.Start();
        }
    }
}
