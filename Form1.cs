using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Management;
using System.Media;

namespace PrinterIPLookup
{
    public partial class Form1 : Form
    {
        string macText;
        CancellationTokenSource cancellationTokenSource;
        const int MAX = 100;

        public Form1()
        {
            Thread thread = new Thread(new ThreadStart(splashFormRun));
            thread.Start();
            Thread.Sleep(4500);
            InitializeComponent();
            thread.Abort();
            Form1_Load(null, null);
            search_Click(null, null);
        }

        private void splashFormRun()
        {
            Application.Run(new Form_Splash());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            macText = macAddressTextBox.Text.ToUpper();
        }

        private async void search_Click(object sender, EventArgs e)
        {
            bool result = false;

            Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
            progress.ProgressChanged += ReportProgress;

            DisableControls();

            if (cancellationTokenSource != null)
                cancellationTokenSource.Dispose();

            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            
            string IPMACPattern = @"\b(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\b"; // matching (192.168.1.109) within (Address:  192.168.1.109)
            // Instantiate the regular expression object.
            Regex regex = new Regex(IPMACPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            // NSLOOKUP STRATEGY
            if (await nslookupAsync(regex, macText, token, progress))
            {
                EnableControls();
                SetPrinterIPInControlPanelProperties();
                return;
            }

            IPMACPattern = @"\b(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\b\s*\b([a-fA-F0-9-]{17}|[a-fA-F0-9]{12})\b"; // matching (192.168.0.16          88-36-5f-df-84-6b) within (192.168.0.16          88-36-5f-df-84-6b     dynamic)
            regex = new Regex(IPMACPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            // ARP & PINGING STRATEGY
            if (checkBoxCleanCache.Checked)
                Utility.ExecuteCommandLine("arp", "-d");    //Flush ARP Cache

            if (await checkARPTableAsync(regex, macText, token, progress))
            {
                EnableControls();
                SetPrinterIPInControlPanelProperties();
                return;
            }

            // Ping all the addresses from 192.168.0.0 until  192.168.0.255 to populate the ARP (Address Resolution Protocol) table with fresh data.
            int min = 0;
            int max = 255;
            const int STEP = 10;
            float percentage = 0;

            await Task.Run(async () =>
            {
                for (int i = min; i <= max; i += STEP)
                {
                    percentage = i;

                    if (token.IsCancellationRequested)
                        break;

                    for (int j = i; (j < (i + STEP)) && (j <= max); j++)
                    {
                        if (token.IsCancellationRequested)
                            break;

                        PingExample.PingAndFind("192.168.0." + j);
                        percentage++;
                    }

                    if (await checkARPTableAsync(regex, macText, token, progress))
                    {
                        EnableControls();
                        result = SetPrinterIPInControlPanelProperties();
                        break;
                    }

                    Invoke((Action)delegate
                    {
                        progressBar.Value = (int)((percentage / (max - min)) * 100);
                    });
                }
            }, token);

            EnableControls();

            if (!result)
            {
                var player = new SoundPlayer(PrinterIPLookup.Properties.Resources.EndFx);
                player.Play();
            }
        }

        private bool SetPrinterIPInControlPanelProperties()
        {
            bool result = false;

            if (!checkBoxMapPortIP.Checked)
            {
                var player = new SoundPlayer(PrinterIPLookup.Properties.Resources.chime);
                player.Play();
                result = true;
            }
            else
            {
                try
                {
                    string onlinePrinterIPAddress = IPAddressLabel.Text;
                    string localPortName = txtBoxPortName.Text;

                    //set the class name and namespace
                    string NamespacePath = "\\\\.\\ROOT\\cimv2";
                    string ClassName = "Win32_TCPIPPrinterPort";

                    ConnectionOptions connectionOptions;

                    connectionOptions = new ConnectionOptions();
                    connectionOptions.EnablePrivileges = true;
                    connectionOptions.Impersonation =
                    System.Management.ImpersonationLevel.Impersonate;

                    //Create ManagementClass
                    ManagementClass oClass = new ManagementClass(NamespacePath + ":" + ClassName);

                    //Get all instances of the class and enumerate them
                    foreach (ManagementObject oObject in oClass.GetInstances())
                    {
                        //access a property of the Management object
                        string hostName = oObject.GetPropertyValue("Name").ToString();

                        if (localPortName == hostName)
                        {
                            string localPrinterIPAddress = oObject.GetPropertyValue("HostAddress").ToString();

                            if (onlinePrinterIPAddress != localPrinterIPAddress)
                            {
                                oObject.SetPropertyValue("HostAddress", onlinePrinterIPAddress);
                                oObject.Put();
                            }

                            var player = new SoundPlayer(PrinterIPLookup.Properties.Resources.shooting_star);
                            player.Play();
                            result = true;
                            break;
                        }
                    }

                    // USEFULL CODE NO NEEDED FOR NOW BUT NICE WAY TO READ AND MODIFY REGISTRY KEYS

                    // Both path below access the same registry key
                    //Computer\HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Control\Print\Monitors\Standard TCP/IP Port\Ports\192.168.0.17 Data=192.168.0.25
                    //Computer\HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Print\Monitors\Standard TCP/IP Port\Ports\192.168.0.17 Data=192.168.0.25

                    ////RegistryKey key = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\Print\Monitors\Standard TCP/IP Port\Ports\" + "192.168.0.17"/*printerPortName*/, RegistryKeyPermissionCheck.Default, System.Security.AccessControl.RegistryRights.QueryValues);
                    ////RegistryKey key = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\Print\Monitors\Standard TCP/IP Port\Ports\" + "192.168.0.17"/*printerPortName*/, RegistryKeyPermissionCheck.Default, System.Security.AccessControl.RegistryRights.FullControl);
                    //string printerPortName = "192.168.0.17";
                    //RegistryKey key = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\Print\Monitors\Standard TCP/IP Port\Ports\" + printerPortName, true);
                    //if (key != null)
                    //{
                    //    String IP = (String)key.GetValue("HostName", String.Empty, RegistryValueOptions.DoNotExpandEnvironmentNames);
                    //    string freshIPAddress = IPAddressLabel.Text;
                    //    /*String IP = (String)*/
                    //    key.SetValue("HostName", freshIPAddress);
                    //}

                    ////RegistryKey key2 = Registry.LocalMachine.OpenSubKey(@"SYSTEM\ControlSet001\Control\Print\Monitors\Standard TCP/IP Port\Ports\192.168.0.17", true);
                    ////String IP2 = (String)key2.GetValue("HostName", String.Empty, RegistryValueOptions.DoNotExpandEnvironmentNames);
                    ////key2.SetValue("HostName", "192.168.0.24");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }

            return result;
        }

        private void DisableControls()
        {
            macAddressTextBox.Enabled = false;
            search.Enabled = false;
            search.Text = "SEARCH Running!!! Please wait...";
            search.BackColor = Color.AliceBlue;
            macAddressTextBox.Enabled = true;
            macAddressTextBox.ReadOnly = true;
            macAddressTextBox.BackColor = Color.LightSteelBlue;
            macAddressTextBox.ForeColor = Color.Black;
            txtBoxPortName.Enabled = true;
            txtBoxPortName.ReadOnly = true;
            txtBoxPortName.BackColor = Color.LightSteelBlue;
            txtBoxPortName.ForeColor = Color.Black;
            progressBar.Value = 0;
            IPAddressLabel.Text = "";
            macText = macAddressTextBox.Text.ToUpper();
        }

        private void EnableControls()
        {
            search.Enabled = true;
            search.Text = "SEARCH";
            search.BackColor = default(Color); ;
            macAddressTextBox.ForeColor = default(Color);
            macAddressTextBox.BackColor = default(Color);
            macAddressTextBox.ReadOnly = false;
            txtBoxPortName.ForeColor = default(Color);
            txtBoxPortName.BackColor = default(Color);
            txtBoxPortName.ReadOnly = false;

            if (cancellationTokenSource.IsCancellationRequested || progressBar.Value < MAX)
                progressBar.Value = 0;
        }

        private async Task<bool> checkARPTableAsync(Regex regex, string input, CancellationToken cancellationToken, IProgress<ProgressReportModel> progress)
        {
            Match match;
            Group group1, group2;
            bool result = false;
            ProgressReportModel report = new ProgressReportModel();

            await Task.Run(() =>
           {
               var arpStream = Utility.ExecuteCommandLine("arp", "-a");
               var line = arpStream.ReadLine();

               while (line != null)
               {
                   if (cancellationToken.IsCancellationRequested)
                       break;

                   if (line != "")
                   {
                       match = regex.Match(line);

                       if (match.Success)
                       {
                           group1 = match.Groups[1];
                           group2 = match.Groups[2];

                           if (group2.ToString().ToUpper().Equals(input))
                           {
                               Invoke((Action)delegate
                               {
                                   IPAddressLabel.Text = group1.ToString();
                               });

                               result = true;
                               report.PercentageComplete = 100;
                               progress.Report(report);
                               break;
                           }
                       }
                   }

                   line = arpStream.ReadLine();
               }
           });

            return result;
        }

        private async Task<bool> nslookupAsync(Regex regex, string input, CancellationToken cancellationToken, IProgress<ProgressReportModel> progress)
        {
            Match match;
            bool result = false;
            ProgressReportModel report = new ProgressReportModel();
            string hostName = textBoxHostName.Text;
            
            if (string.IsNullOrWhiteSpace(hostName))
                return result;

            await Task.Run(() =>
            {
                bool checkIP = false;
                var arpStream = Utility.ExecuteCommandLine("nslookup", hostName);
                var line = arpStream.ReadLine();

                while (line != null)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    if (line != "")
                    {
                        if (!checkIP && line.Contains(hostName))
                        {
                            checkIP = true;
                            line = arpStream.ReadLine();
                        }
                        else
                        {
                            line = arpStream.ReadLine();
                            continue;
                        }

                        match = regex.Match(line);

                        if (match.Success)
                        {
                            Invoke((Action)delegate
                            {
                                IPAddressLabel.Text = match.Value;
                            });

                            result = true;
                            report.PercentageComplete = 100;
                            progress.Report(report);
                            break;
                        }

                    }
                    
                    line = arpStream.ReadLine();
                }
                
            });

            return result;
        }

        private void ReportProgress(object sender, ProgressReportModel e)
        {
            progressBar.Value = e.PercentageComplete;
        }

        private void macAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            Invoke((Action)delegate
            {
                TextBox txbox = (TextBox)sender;
                macText = txbox.Text.ToUpper();
            });
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            Invoke((Action)delegate
            {
                IPAddressLabel.Text = "";
                textBoxHostName.Text = "SAMSUNG_PRINTER";
                macAddressTextBox.Text = "00-15-99-92-CC-EB";
                txtBoxPortName.Text = "192.168.0.17";
                IPAddressLabel.Text = "";
                progressBar.Value = 0;
            });

            cancellationTokenSource.Cancel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Invoke((Action)delegate
            {
                progressBar.Value = 0;
            });

            cancellationTokenSource.Cancel();
        }
    }
}
