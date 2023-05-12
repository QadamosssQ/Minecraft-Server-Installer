using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinecraftServerMaker
{

    public partial class MainWindow : Window
    {
        string InstallationLocation;
        bool mods = false;
        string McVersion = "1.19.4";





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

        private void StartInstallation(object sender, RoutedEventArgs e)
        {


            if (FolderCheck() == 0)
            {
                



                if (mods == true)
                {
                    string url = $"https://meta.fabricmc.net/v2/versions/loader/{McVersion}/0.14.19/0.11.2/server/jar"; // adres URL pliku do pobrania


                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            client.DownloadFile(url, InstallationLocation); // pobranie pliku
                            MessageBox.Show("Plik został pobrany i zapisany w: " + InstallationLocation);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Wystąpił błąd: " + ex.Message);
                        }
                    }


                }



            }
            else if (FolderCheck() == 1)
            {
                MessageBox.Show("Folder nie jest pusty.");
            }
            else
            {
                MessageBox.Show("This folder doesn't exist");
            }




        }

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
    }

}






//todo:
//dokńcz, dodaj plik txt eula=true, wykonaj plik servera jar, dodaj mc bez modów