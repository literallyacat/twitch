using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NAudio;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using System.Diagnostics;
using NAudio.Mixer;

namespace LiterallyACatOverlay
{
    public partial class Form1 : Form
    {
        private WaveIn _sourceStream = null;

        private float _lastPeak;

        private readonly SampleAggregator _aggregator;

        private LiterallyACat _catForm;

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;

            _aggregator = new SampleAggregator();
        }

        private static void InvokeIfRequired(Control control, MethodInvoker action)
        {
            // See Update 2 for edits Mike de Klerk suggests to insert here.

            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _catForm = new LiterallyACat();
            _catForm.Show();
        }

        private void UpdateProgressBarVolume(int val)
        {
            progressBar1.Value = val;
            if (val > 0)
            {
                _catForm.Talking();
                return;
            }
            _catForm.NotTalking();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sources = new List<WaveInCapabilities>();

            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                sources.Add(WaveIn.GetCapabilities(i));
            }

            comboBox1.Items.Clear();

            foreach (var source in sources)
            {
                comboBox1.Items.Add(source.ProductName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null) return;

            int deviceNumber = comboBox1.SelectedIndex;

            _sourceStream = new WaveIn();
            _sourceStream.DeviceNumber = deviceNumber;
            _sourceStream.WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(deviceNumber).Channels);
            _sourceStream.DataAvailable += _sourceStream_DataAvailable;
            _aggregator.MaximumCalculated += _aggregator_MaximumCalculated;
            //_aggregator.NotificationCount = _sourceStream.WaveFormat.SampleRate / 10;
            _aggregator.NotificationCount = 1000;

            _sourceStream.StartRecording();
        }

        private void _aggregator_MaximumCalculated(object sender, MaxSampleEventArgs e)
        {
            _lastPeak = Math.Max(e.MaxSample, Math.Abs(e.MinSample));
            InvokeIfRequired(this, () => UpdateProgressBarVolume((int)(_lastPeak * 100)));
        }

        private void _sourceStream_DataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            int bytesRecorded = e.BytesRecorded;

            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                short sample = (short)((buffer[index + 1] << 8) | buffer[index + 0]);
                float sample32 = sample / 32768f;
                _aggregator.Add(sample32);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_sourceStream != null)
            {
                _sourceStream.StopRecording();
                _sourceStream.Dispose();
                _sourceStream = null;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
            this.Close();
        }
    }
}
