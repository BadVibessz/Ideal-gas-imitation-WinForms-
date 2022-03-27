using System;
using System.Windows.Forms;

namespace animationTry2
{
    public partial class Statistics : Form
    {
        private Animator _animator;
        public Statistics(Animator animator)
        {
            InitializeComponent();
            
            this._animator = animator;
            this.dataGridView1.DataSource = _animator.Balls;
            this.timer1.Start();
            
        }

        public void DataGridRefresh()
        {
            this.dataGridView1.Refresh();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }
    }
}