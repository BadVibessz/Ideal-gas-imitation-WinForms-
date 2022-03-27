using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace animationTry2
{
    public partial class Form1 : Form
    {
        private  readonly Animator _animator;
        private Graphics _graphics;

        public Form1()
        {
            InitializeComponent();
            
            this.ballCountNumeric.Controls.RemoveAt(0);
            
            this._graphics = panel1.CreateGraphics();
            this._animator = new Animator(panel1.CreateGraphics(), this.panel1.ClientRectangle,this, this.ballCountNumeric);
            this._animator.Start();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            _animator.AddBall(e.Location, panel1.ClientRectangle);
            this.ballCountNumeric.Value++;
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            if (_animator != null)
                this._animator.Update(panel1.CreateGraphics(), panel1.ClientRectangle); //todo:
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
    }
}