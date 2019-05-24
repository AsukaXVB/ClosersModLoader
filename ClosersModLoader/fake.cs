using System;
using System.Windows.Forms;

namespace ClosersModLoader
{
    public partial class fake : Form
    {
        public fake()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            SetVisibleCore(false);
        }
        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(value);
        }

        private void fake_Load(object sender, EventArgs e)
        {
            Program.start();
        }
    }
}
