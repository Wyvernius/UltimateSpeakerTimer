using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UltimateSpeakerTimer
{
    public partial class ConfirmName : Form
    {
        public ConfirmName()
        {
            InitializeComponent();
        }
        public string SpeakerName;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SpeakerName = textBox1.Text;
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            ReturnOk();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            ReturnCancel();
        }

        private DialogResult ReturnOk()
        {
            return DialogResult.OK;
        }

        private DialogResult ReturnCancel()
        {
            return DialogResult.Cancel;
        }
    }
}
