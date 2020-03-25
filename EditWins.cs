using System;
using System.IO;
using System.Windows.Forms;

namespace DS3_Arena_Tool {
    public partial class EditWins : Form {
        public EditWins() {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            numericUpDown1.Text = File.ReadAllText("wins.txt");
        }

        private void button1_Click(object sender, EventArgs e) {
            System.IO.File.WriteAllText("wins.txt", numericUpDown1.Text);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}