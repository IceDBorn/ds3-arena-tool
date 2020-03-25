using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace DS3_Arena_Tool {
    public partial class Main : Form {
        private readonly GlobalKeyboardHook _backup = new GlobalKeyboardHook();
        private readonly GlobalKeyboardHook _restore = new GlobalKeyboardHook();
        public Main() {
            InitializeComponent();
            winsLabel.Text = File.ReadAllText("wins.txt");
        }
        
        private void button1_Click(object sender, EventArgs e) {
            var flag = false;
            try {
                var sourceDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                      "//DarkSoulsIII";
                const string targetDirectory = @"Backup";
                Replacer.Copy(sourceDirectory, targetDirectory);
            }
            catch
            {
                const bool flag2 = false;
                var error = new Error(flag2);
                error.ShowDialog();
                flag = true;
            }

            try {
                var wins = File.ReadAllText("wins.txt");
                var winsInt = int.Parse(wins);
                winsInt++;
                wins = winsInt.ToString();
                if (flag.Equals(false)) {
                    System.IO.File.WriteAllText("wins.txt", wins);
                }
            }
            catch {
                System.IO.File.WriteAllText("wins.txt", "0");
            }
            winsLabel.Text = File.ReadAllText("wins.txt");
            button3.Focus();
        }

        private void button2_Click(object sender, EventArgs e) {
            try {
                var targetDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                      "//DarkSoulsIII";
                const string sourceDirectory = @"Backup";
                Replacer.Copy(sourceDirectory, targetDirectory);
            }
            catch {
                const bool flag2 = true;
                var error = new Error(flag2);
                error.ShowDialog();
            }
            button3.Focus();
        }

        private void Main_Resize(object sender, EventArgs e) {
            if (FormWindowState.Minimized != WindowState) return;
            Hide();
            this.notifyIcon1.Visible = true;
        }

        private void notifyIcon1_MouseMove(object sender, MouseEventArgs e) {
            notifyIcon1.Text = "Current Wins: " + File.ReadAllText("wins.txt");
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {
            Show();
            WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;
        }

        private void Main_Load(object sender, EventArgs e) {
            _backup.HookedKeys.Add(Keys.F9);
            _restore.HookedKeys.Add(Keys.F10);
            _backup.KeyDown += new KeyEventHandler(backup_KeyDown);
            _restore.KeyDown += new KeyEventHandler(restore_KeyDown);
        }

        private void backup_KeyDown(object sender, KeyEventArgs e) {
            var sourceDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                                  + "//DarkSoulsIII"; 
            const string targetDirectory = @"Backup";
            try {
                var wins = File.ReadAllText("wins.txt");
                var winsInt = int.Parse(wins);
                winsInt++;
                wins = winsInt.ToString();
                System.IO.File.WriteAllText("wins.txt", wins);
            }
            catch {
                System.IO.File.WriteAllText("wins.txt", "0");
            }

            Replacer.Copy(sourceDirectory, targetDirectory);
            winsLabel.Text = File.ReadAllText("wins.txt");
            e.Handled = true ;
        }

        private static void restore_KeyDown(object sender, KeyEventArgs e) {
            var targetDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//DarkSoulsIII";
            const string sourceDirectory = @"Backup";
            Replacer.Copy(sourceDirectory, targetDirectory);
            e.Handled = true;
        }

        private void button4_Click(object sender, EventArgs e) {
            var editor = new EditWins();
            editor.ShowDialog();
            winsLabel.Text = File.ReadAllText("wins.txt");
            button3.Focus();
        }
    }
}