using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CountdownTimer {
    public partial class Form1 : Form {
        private int _seconds = 600;
        public Form1() {
            InitializeComponent();
            MouseUp += Form1_MouseUp;
            MouseMove += Form1_MouseMove;
            MouseDown += Form1_MouseDown;

            label1.MouseUp += Form1_MouseUp;
            label1.MouseMove += Form1_MouseMove;
            label1.MouseDown += Form1_MouseDown;

            label2.MouseUp += Form1_MouseUp;
            label2.MouseMove += Form1_MouseMove;
            label2.MouseDown += Form1_MouseDown;

            UpdateSeconds();
        }

        private void UpdateSeconds() {
            int minutes = _seconds / 60;
            int seconds = _seconds - (minutes * 60);
            label2.Text = $"{minutes.ToString("00")}:{seconds.ToString("00")}";
        }

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private void Form1_MouseDown(object sender, MouseEventArgs e) {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e) {
            if (dragging) {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e) {
            dragging = false;
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if (_seconds > 0) {
                _seconds--;
                UpdateSeconds();
            }
        }
    }
}
