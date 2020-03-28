using System;
using System.IO;
using System.Windows.Forms;

namespace DS3_Arena_Tool {
    public partial class Main : Form {
        private readonly GlobalKeyboardHook _backup = new GlobalKeyboardHook();
        private readonly GlobalKeyboardHook _restore = new GlobalKeyboardHook();
        
        public Main() {
            InitializeComponent();
            winsLabel.Text = File.ReadAllText("wins.txt");
        }
        
        private void Main_Load(object sender, EventArgs e) {
            _backup.HookedKeys.Add(Keys.F9);
            _restore.HookedKeys.Add(Keys.F10);
            _backup.KeyDown += new KeyEventHandler(backup_KeyDown);
            _restore.KeyDown += new KeyEventHandler(restore_KeyDown);
        }
        
        private void Main_Resize(object sender, EventArgs e) {
            if (FormWindowState.Minimized != WindowState) return;
            Hide();
            this.notifyIcon1.Visible = true;
        }
        
        private void button1_Click(object sender, EventArgs e) {
            Replacer.Backup(1);
            winsLabel.Text = File.ReadAllText("wins.txt");
            button3.Focus();
        }

        private void button2_Click(object sender, EventArgs e) {
            Replacer.Restore(1);
            button3.Focus();
        }
        
        private void button4_Click(object sender, EventArgs e) {
            var editor = new EditWins();
            editor.ShowDialog();
            winsLabel.Text = File.ReadAllText("wins.txt");
            button3.Focus();
        }

        private void notifyIcon1_MouseMove(object sender, MouseEventArgs e) {
            notifyIcon1.Text = "Current Wins: " + File.ReadAllText("wins.txt");
        }
        
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {
            Show();
            WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;
        }
        
        private void backup_KeyDown(object sender, KeyEventArgs e) {
            Replacer.Backup(0);
            winsLabel.Text = File.ReadAllText("wins.txt");
            e.Handled = true;
        }

        private static void restore_KeyDown(object sender, KeyEventArgs e) {
            Replacer.Restore(0);
            e.Handled = true;
        }
    }
}