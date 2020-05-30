using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteScreen
{
    public partial class Form2 : Form
    {
        public int interval { get; set; }

        public Form2(int interval)
        {
            InitializeComponent();

            this.interval = interval;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            numInterval.Value = this.interval;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.interval = (int)numInterval.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
