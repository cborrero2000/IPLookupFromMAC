using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrinterIPLookup
{
    public partial class Form1 : Form
    {
        string macText;
        CancellationTokenSource cancellationTokenSource;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            macText = macAddressTextBox.Text.ToUpper();
        }

        private async void search_Click(object sender, EventArgs e)
        {
            DisableSearchButton();

            if (cancellationTokenSource != null)
                cancellationTokenSource.Dispose();

            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            string IPMACPattern = @"\b(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\b\s*\b([a-fA-F0-9-]{17}|[a-fA-F0-9]{12})\b"; // matching (192.168.0.16          88-36-5f-df-84-6b) within (192.168.0.16          88-36-5f-df-84-6b     dynamic)

            // Instantiate the regular expression object.
            Regex regex = new Regex(IPMACPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            if (await checkARPTableAsync(regex, macText, token))
            {
                EnableSearchButton();
                return;
            }

            // Ping all the addresses from 192.168.0.0 until  192.168.0.255 to populate the ARP (Address Resolution Protocol) table with fresh data.
            int min = 0;
            int max = 255;
            const int STEP = 10;

            await Task.Run(async () =>
            {
                for (int i = min; i <= max; i += STEP)
                {
                    if (token.IsCancellationRequested)
                        break;

                    for (int j = i; (j < (i + STEP)) && (j <= max); j++)
                    {
                        if (token.IsCancellationRequested)
                            break;

                        PingExample.PingAndFind("192.168.0." + j);
                    }

                    if (await checkARPTableAsync(regex, macText, token))
                    {
                        EnableSearchButton();
                        break;
                    }
                }
            }, token);

            EnableSearchButton();
        }

        private void DisableSearchButton()
        {
            search.Enabled = false;
            search.Text = "SEARCH RUNNING !!! Please wait...";
            search.BackColor = Color.LightBlue;
        }

        private void EnableSearchButton()
        {
            search.Enabled = true;
            search.Text = "SEARCH";
            search.BackColor = default(Color); ;
        }

        private async Task<bool> checkARPTableAsync(Regex regex, string input, CancellationToken cancellationToken)
        {
            Match match;
            Group group1, group2;
            bool result = false;

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

                           if (group2.ToString().ToUpper().Contains(input))
                           {
                               Invoke((Action)delegate
                               {
                                   IPAddressLabel.Text = group1.ToString();
                               });

                               result = true;
                               break;
                           }
                       }
                   }

                   line = arpStream.ReadLine();
               }
           });

            return result;
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
                macAddressTextBox.Text = "00-15-99-92-CC-EB";
            });

            cancellationTokenSource.Cancel();
        }
    }
}
