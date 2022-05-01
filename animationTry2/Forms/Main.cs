using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace animationTry2
{
    public partial class Form1 : Form
    {
        private Animator _animator;
        private Graphics _graphics;
        private string _theme = "light";
        private ControlRecorder _recorder;

        public Form1()
        {
            InitializeComponent();

            this.ballCountNumeric.Controls.RemoveAt(0);

            this._graphics = panel1.CreateGraphics();
            this._animator = new Animator(panel1.CreateGraphics(), this.panel1.ClientRectangle);
            this._animator.colorToClear = Color.White;
            this._animator.BallDied += c_BallDied;
        }


        private void c_BallDied(object sender, EventArgs e)
        {
            new Thread((s) => //todo: understand properly
            {
                this.ballCountNumeric.BeginInvoke((MethodInvoker)(() => this.ballCountNumeric.Value--));
            }).Start();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            _animator.AddBallAndStart(e.Location, panel1.ClientRectangle);
            this.ballCountNumeric.Value++;
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            if (_animator != null)
                this._animator.Update(panel1.CreateGraphics(), panel1.ClientRectangle);
        }

        private void AccelBtn_Click(object sender, EventArgs e)
        {
            this._animator.Accelerate();
        }

        private void slowBtn_Click(object sender, EventArgs e)
        {
            this._animator.Slow();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
        }

        public void DecreaseNumeric()
        {
            this.ballCountNumeric.Value--;
        }

        private void statBtn_Click(object sender, EventArgs e)
        {
            var s = new Statistics(this._animator);
            this._animator.Physics.StatisticsForm = s;
            s.ShowDialog();
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            this._animator.Clear();
            this._animator = new Animator(panel1.CreateGraphics(), this.panel1.ClientRectangle);
            this._animator.colorToClear = this.panel1.BackColor;
            this.ballCountNumeric.Value = 0;
        }

        private void darkBtn_Click(object sender, EventArgs e)
        {
            this.ChangeTheme("dark");
        }

        private void lghtBtn_Click(object sender, EventArgs e)
        {
            this.ChangeTheme("light");
        }

        private void ChangeTheme(string theme)
        {
            switch (theme)
            {
                case "dark":
                    if (_theme == "dark") return;
                    Color formDarkBack = Color.FromArgb(66, 66, 66);
                    Color btnDarkBack = Color.FromArgb(84, 84, 84);
                    Color panelDarkBack = Color.FromArgb(220, 220, 220);


                    // form
                    this.BackColor = formDarkBack;
                    this.ForeColor = Color.White;

                    // btns
                    this.accelBtn.BackColor = btnDarkBack;
                    this.slowBtn.BackColor = btnDarkBack;
                    this.clearBtn.BackColor = btnDarkBack;
                    this.statBtn.BackColor = btnDarkBack;

                    // panel
                    this.panel1.BackColor = panelDarkBack;
                    this._animator.colorToClear = panelDarkBack;
                    this._theme = "dark";
                    break;
                case "light": //todo:
                    if (_theme == "light") return;
                    Color formLightBack = Color.FromArgb(255, 253, 232);
                    Color btnLightBack = Color.FromArgb(84, 84, 84);
                    Color panelLightBack = Color.White;


                    // form
                    this.BackColor = formLightBack;
                    this.ForeColor = Color.White;

                    // btns
                    this.accelBtn.BackColor = btnLightBack;
                    this.slowBtn.BackColor = btnLightBack;
                    this.clearBtn.BackColor = btnLightBack;
                    this.statBtn.BackColor = btnLightBack;

                    // panel
                    this.panel1.BackColor = panelLightBack;
                    this._animator.colorToClear = panelLightBack;
                    this._theme = "light";
                    break;
            }
            this.Refresh();
        }

        private void recordBtn_Click(object sender, EventArgs e)
        {
            this._recorder = new ControlRecorder(new RecordArguments("C:/Users/danil/Desktop/record.avi", 60, SharpAvi.CodecIds.MotionJpeg,
                100, this.panel1.RectangleToScreen(panel1.ClientRectangle), panel1.ClientRectangle),this.panel1.PointToClient(this.panel1.Location));
        }

        private void stopRecBtn_Click(object sender, EventArgs e)
        {
            //this.panel1.
            this._recorder.Dispose();
        }
    }
}