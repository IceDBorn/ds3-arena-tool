using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DS3_Arena_Tool {
    internal static class Program {
        [STAThread]
        private static void Main() {
            
            if (System.IO.File.Exists("wins.txt") == false) {
                System.IO.File.WriteAllText("wins.txt", "0");
            }
            else {
                var wins = File.ReadAllText("wins.txt");
                try {
                    var winsInt = int.Parse(wins);
                }
                catch {
                    System.IO.File.WriteAllText("wins.txt", "0");
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}