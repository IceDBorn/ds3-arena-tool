using System;
using System.Windows.Forms;

namespace DS3_Arena_Tool {
    public partial class Error : Form {
        public Error(bool flag) {
            InitializeComponent();
            label1.Text = flag.Equals(true) ? "You don't have a Dark Souls 3 Backup Folder!" : "You don't have a Dark Souls 3 Save Folder!";
        }

        private void button1_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}