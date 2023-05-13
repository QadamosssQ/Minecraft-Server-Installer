using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Management;

namespace MinecraftServerMaker
{

    public partial class MainWindow : Window
    {

        string InstallationLocation;
        bool mods = false;
        string McVersion = "1.19.4";  //default
        int RAMforServer = 2;



        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChooseFolder(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                FilesLocation.Text = dialog.SelectedPath;
                InstallationLocation = dialog.SelectedPath;
            }
        }

        private void YesMods(object sender, RoutedEventArgs e)
        {
            mods = true;

            BtYes.BorderBrush = Brushes.White;
            BtYes.BorderThickness = new Thickness(5);
            BtNo.BorderBrush = null;
            BtNo.BorderThickness = new Thickness(0);
        }

        private void NoMods(object sender, RoutedEventArgs e)
        {
            mods = false;
            BtNo.BorderBrush = Brushes.White;
            BtNo.BorderThickness = new Thickness(5);
            BtYes.BorderBrush = null;
            BtYes.BorderThickness = new Thickness(0);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string ChoosedMcVersion = GetMcVersion.SelectedItem.ToString();
            McVersion = ChoosedMcVersion.Replace("System.Windows.Controls.ComboBoxItem: ", "");
        }

        //---------------Main---------------

        private void StartInstallation(object sender, RoutedEventArgs e)
        {

            if (FolderCheck() == 0 && GetRAM() != -1)
            {

                if (mods == true)
                {

                    if(CheckRAM() == true)
                    {

                        InstallServerFiles();

                        MessageBox.Show($"All done. Wait until you see end of preparing spawm area.\n To stop server just close terminal. \n\n Server location: {InstallationLocation} \n Server version: {McVersion} \n RAM for server {RAMforServer} \n Mods: {mods}", "All Done & status");

                        RunServer();

                    }
                    else
                    {
                        MessageBox.Show("Change RAM amount", "Error: RAM");
                    }

                }

            }
            else if (FolderCheck() == 1)
            {
                MessageBox.Show("Folder isn't empty.", "Error: Folder");
            }
            else if (GetRAM() == -1)
            {
                MessageBox.Show("Bad amount of RAM", "Error: RAM");
            }
            else
            {
                MessageBox.Show("This folder doesn't exist", "Error: Folder");
            }

        }

        //---------------Main---------------

        private int FolderCheck()
        {
            if (Directory.Exists(InstallationLocation))
            {
                if (Directory.GetFiles(InstallationLocation).Length != 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 2;
            }
        }

        private int GetRAM()
        {
            string input = CountRAM.Text;
            int result;

            if (int.TryParse(input, out result))
            {
                return result;
            }
            else
            {
                return 1000;
            }

        }

        private bool CheckRAM()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory");

            long totalMemoryBytes = 0;
            foreach (ManagementObject mo in searcher.Get())
            {
                long capacity = Convert.ToInt64(mo["Capacity"]);
                totalMemoryBytes += capacity;
            }

            double totalMemoryGB = (double)totalMemoryBytes / (1024 * 1024 * 1024);

            double finall_ram = (totalMemoryGB - 5) - GetRAM();



            

            if (finall_ram >= 2)
            {
                RAMforServer = GetRAM();
                return true;
            }
            else
            {
                return false;
            }

        }

        private void CreateEula()
        {
            string EulaPath = InstallationLocation + @"\eula.txt";

            if (!File.Exists(EulaPath))
            {

                using (StreamWriter sw = File.CreateText(EulaPath))
                {
                    sw.WriteLine("eula=true");

                }
            }

        }

        private void InstallServerFiles()
        {
            try
            {
                string url = $"https://meta.fabricmc.net/v2/versions/loader/{McVersion}/0.14.19/0.11.2/server/jar"; 

                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c curl {url} --output \"{InstallationLocation}\\fabric-server-mc.{McVersion}-loader.0.14.19-launcher.0.11.2.jar\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process process = new Process();
                process.StartInfo = processStartInfo;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();


                CreateEula();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }


        private void RunServer()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.WorkingDirectory = $"{InstallationLocation}";
            startInfo.Arguments = $"/k java -Xmx{RAMforServer}G -jar fabric-server-mc.{McVersion}-loader.0.14.19-launcher.0.11.2.jar nogui";
            Process.Start(startInfo);

        }



    }

}






//todo:
// dodaj mc bez modów




