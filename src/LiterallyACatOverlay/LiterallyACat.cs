using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LiterallyACatOverlay
{
    public partial class LiterallyACat : Form
    {
        private static Random rnd = new Random();

        private bool _isTalking = false;

        private System.Timers.Timer _updateImageTimer;

        private List<Bitmap> _talkingImages = new List<Bitmap>()
        {
            LiterallyACatOverlay.Properties.Resources.overlay_t_talking,
            LiterallyACatOverlay.Properties.Resources.overlay_t_talking_2,
            LiterallyACatOverlay.Properties.Resources.overlay_t_talking_3,
            LiterallyACatOverlay.Properties.Resources.overlay_t_talking_4,
        };

        private List<Bitmap> _silentImages = new List<Bitmap>()
        {
            LiterallyACatOverlay.Properties.Resources.overlay_t_silent,
        };

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private void LiterallyACat_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void LiterallyACat_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void LiterallyACat_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        public LiterallyACat()
        {
            InitializeComponent();
            Load += LiterallyACat_Load;
            MouseUp += LiterallyACat_MouseUp;
            MouseMove += LiterallyACat_MouseMove;
            MouseDown += LiterallyACat_MouseDown;

            pictureBox1.MouseUp += LiterallyACat_MouseUp;
            pictureBox1.MouseMove += LiterallyACat_MouseMove;
            pictureBox1.MouseDown += LiterallyACat_MouseDown;

        }

        private void LiterallyACat_Load(object sender, EventArgs e)
        {
            _updateImageTimer = new System.Timers.Timer(100);
            _updateImageTimer.Enabled = true;
            _updateImageTimer.Elapsed += _updateImageTimer_Elapsed;
            pictureBox1.Image = LiterallyACatOverlay.Properties.Resources.overlay_t_silent;
        }

        private void _updateImageTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int r;
            if (!_isTalking)
            {
                r = rnd.Next(_silentImages.Count);
                pictureBox1.Image = _silentImages[r];
                return;
            }

            r = rnd.Next(_talkingImages.Count);
            pictureBox1.Image = _talkingImages[r];
        }

        public void Talking()
        {
            _isTalking = true;
        }

        public void NotTalking()
        {
            _isTalking = false;
        }
    }
}
