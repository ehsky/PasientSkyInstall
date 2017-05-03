using System;
using System.Collections.Generic;
using System.Linq;
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
//For .NET check
using Microsoft.Win32;

namespace PasientSky.Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DotNet_Check(object sender, RoutedEventArgs e)
        {
            // Opens the registry key for the .NET Framework entry.
            using (RegistryKey ndpKey =
                RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").
                OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                // As an alternative, if you know the computers you will query are running .NET Framework 4.5 
                // or later, you can use:
                // using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, 
                // RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
                foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                {
                    if (versionKeyName.StartsWith("v"))
                    {

                        RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                        string name = (string)versionKey.GetValue("Version", "");
                        string sp = versionKey.GetValue("SP", "").ToString();
                        string install = versionKey.GetValue("Install", "").ToString();
                        if (install == "") //no install info, must be later.
                        {
                            Console.WriteLine(versionKeyName + "  " + name);
                        }
                        else
                        {
                            if (sp != "" && install == "1")
                            {
                                Console.WriteLine(versionKeyName + "  " + name + "  SP" + sp);
                                if (versionKeyName.StartsWith("v2"))
                                {
                                    lblNET20.Foreground = Brushes.Black;
                                    lblNET20.FontWeight = FontWeights.Bold;
                                }
                                if (versionKeyName.StartsWith("v3.0"))
                                {
                                    lblNET30.Foreground = Brushes.Black;
                                    lblNET30.FontWeight = FontWeights.Bold;
                                }
                                if (versionKeyName.StartsWith("v3.5"))
                                {
                                    lblNET35.Foreground = Brushes.Black;
                                    lblNET35.FontWeight = FontWeights.Bold;
                                }
                            }

                        }
                        if (name != "")
                        {
                            continue;
                        }
                        foreach (string subKeyName in versionKey.GetSubKeyNames())
                        {
                            RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                            name = (string)subKey.GetValue("Version", "");
                            if (name != "")
                                sp = subKey.GetValue("SP", "").ToString();
                            install = subKey.GetValue("Install", "").ToString();
                            if (install == "") //no install info, must be later.
                                Console.WriteLine(versionKeyName + "  " + name);
                            else
                            {
                                if (sp != "" && install == "1")
                                {
                                    Console.WriteLine("  " + subKeyName + "  " + name + "  SP" + sp);
                                }
                                else if (install == "1")
                                {
                                    Console.WriteLine("  " + subKeyName + "  " + name);
                                    if (name.StartsWith("4.5"))
                                    {
                                        lblNET45.Foreground = Brushes.Black;
                                        lblNET45.FontWeight = FontWeights.Bold;
                                    }
                                    if (name.StartsWith("4.6"))
                                    {
                                        lblNET46.Foreground = Brushes.Black;
                                        lblNET46.FontWeight = FontWeights.Bold;
                                    }
                                    if (name.StartsWith("4.7"))
                                    {
                                        lblNET47.Foreground = Brushes.Black;
                                        lblNET47.FontWeight = FontWeights.Bold;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ACdeactivate(object sender, RoutedEventArgs e)
        {
            string PStext;
            PStext = "powercfg -x -monitor-timeout-ac 0;powercfg -x -standby-timeout-ac 0";
            System.Diagnostics.Process.Start("powershell.exe", PStext);
        }

        private void ScreenDeactivate(object sender, RoutedEventArgs e)
        {

        }
    }
}
