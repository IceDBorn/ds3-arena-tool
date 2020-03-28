using System;
using System.IO;
using System.Media;

namespace DS3_Arena_Tool {
    public static class Replacer {
        private static void Copy(string sourceDirectory, string targetDirectory) {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);
 
            CopyAll(diSource, diTarget);
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target) {
            Directory.CreateDirectory(target.FullName);
            
            foreach (var fi in source.GetFiles()) {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }
            
            foreach (var diSourceSubDir in source.GetDirectories()) {
                var nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        public static void Backup(int errorFlag) {
            var flag = true;
            try {
                var sourceDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                      "//DarkSoulsIII";
                const string targetDirectory = @"Backup";
                Replacer.Copy(sourceDirectory, targetDirectory);
                if (errorFlag.Equals(0)) {
                    SystemSounds.Asterisk.Play();
                }
            }
            catch {
                flag = false;
                if (errorFlag.Equals(1)) {
                    var error = new Error(false);
                    SystemSounds.Hand.Play();
                    error.ShowDialog();
                }
            }

            try {
                var wins = File.ReadAllText("wins.txt");
                var winsInt = int.Parse(wins);
                winsInt++;
                wins = winsInt.ToString();
                if (flag.Equals(true)) {
                    System.IO.File.WriteAllText("wins.txt", wins);
                }
            }
            catch {
                System.IO.File.WriteAllText("wins.txt", "0");
            }
        }

        public static void Restore(int errorFlag)
        {
            try {
                var targetDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                      "//DarkSoulsIII";
                const string sourceDirectory = @"Backup";
                Replacer.Copy(sourceDirectory, targetDirectory);
                if (errorFlag.Equals(0)) {
                    SystemSounds.Asterisk.Play();
                }
            }
            catch {
                if (errorFlag.Equals(1)) {
                    const bool flag = true;
                    var error = new Error(flag);
                    SystemSounds.Hand.Play();
                    error.ShowDialog();
                }
            }
        }
    }
}