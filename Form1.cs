using AForge.Video;
using AForge.Video.DirectShow;

namespace DotVideo
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection _fic;
        private VideoCaptureDevice? _vcd;
        private Bitmap _bitmap;
        public Form1()
        {
            InitializeComponent();
            _fic = new(FilterCategory.VideoInputDevice);
        }

        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Copia a imagem do buffer da camera
            using var image = (Bitmap)eventArgs.Frame.Clone();
            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            _bitmap = image;
            Invoke(() => panel1.Refresh);
        }

        private void UpdateVideoDevices()
        {
            _fic = new(FilterCategory.VideoInputDevice);
            comboBox1.Items.Clear();

            foreach (FilterInfo dev in _fic)
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
            if (comboBox1.SelectedIndex < 0)
            {
                return;
            }
            _vcd = new(_fic[comboBox1.SelectedIndex].MonikerString);
            var capabilities = _vcd.VideoCapabilities[1];
            _vcd.VideoResolution = capabilities;
            _vcd.NewFrame += FinalFrame_NewFrame;
            _vcd.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_vcd != null)
            {
                _vcd.SignalToStop();
                _vcd.NewFrame -= FinalFrame_NewFrame;
                _vcd = null;
            }
            if (comboBox1.SelectedIndex < 0)
            {
                return;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (_vcd != null)
            {
                _vcd.NewFrame -= FinalFrame_NewFrame;
                _vcd.SignalToStop();
                _vcd.WaitForStop();
                _vcd = null;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (_bitmap != null)
            {
                e.Graphics.Clear(Color.White);
                e.Graphics.DrawImageUnscaled(_bitmap, new Point(0, 0));
            }
        }
    }
}
