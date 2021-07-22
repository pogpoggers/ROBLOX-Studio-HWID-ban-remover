using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Un4seen.Bass;
using Un4seen.Bass.Misc;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using System.Management;
using Microsoft.Win32;
using System.IO;

namespace ROBLOX_Studio_HWID_ban_remover
{
   
    public partial class Form1 : Form
    {
        // admin check function
        private bool IsCurrentUserAnAdmin()
        {
            var principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        // copyPasted mac address changer library (bless u MadMAC)
        public class Adapter
        {
            public ManagementObject adapter;
            public string adaptername;
            public string customname;
            public int devnum;

            public Adapter(ManagementObject a, string aname, string cname, int n)
            {
                this.adapter = a;
                this.adaptername = aname;
                this.customname = cname;
                this.devnum = n;
            }

            public Adapter(NetworkInterface i) : this(i.Description) { }

            public Adapter(string aname)
            {
                this.adaptername = aname;

                var searcher = new ManagementObjectSearcher("select * from win32_networkadapter where Name='" + adaptername + "'");
                var found = searcher.Get();
                this.adapter = found.Cast<ManagementObject>().FirstOrDefault();

                // Extract adapter number; this should correspond to the keys under
                // HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}
                try
                {
                    var match = Regex.Match(adapter.Path.RelativePath, "\\\"(\\d+)\\\"$");
                    this.devnum = int.Parse(match.Groups[1].Value);
                }
                catch
                {
                    return;
                }

                // Find the name the user gave to it in "Network Adapters"
                this.customname = NetworkInterface.GetAllNetworkInterfaces().Where(
                    i => i.Description == adaptername
                ).Select(
                    i => " (" + i.Name + ")"
                ).FirstOrDefault();
            }

            /// <summary>
            /// Get the .NET managed adapter.
            /// </summary>
            public NetworkInterface ManagedAdapter
            {
                get
                {
                    return NetworkInterface.GetAllNetworkInterfaces().Where(
                        nic => nic.Description == this.adaptername
                    ).FirstOrDefault();
                }
            }

            /// <summary>
            /// Get the MAC address as reported by the adapter.
            /// </summary>
            public string Mac
            {
                get
                {
                    try
                    {
                        return BitConverter.ToString(this.ManagedAdapter.GetPhysicalAddress().GetAddressBytes()).Replace("-", "").ToUpper();
                    }
                    catch { return null; }
                }
            }

            /// <summary>
            /// Get the registry key associated to this adapter.
            /// </summary>
            public string RegistryKey
            {
                get
                {
                    return String.Format(@"SYSTEM\ControlSet001\Control\Class\{{4D36E972-E325-11CE-BFC1-08002BE10318}}\{0:D4}", this.devnum);
                }
            }

            /// <summary>
            /// Get the NetworkAddress registry value of this adapter.
            /// </summary>
            public string RegistryMac
            {
                get
                {
                    try
                    {
                        using (RegistryKey regkey = Registry.LocalMachine.OpenSubKey(this.RegistryKey, false))
                        {
                            return regkey.GetValue("NetworkAddress").ToString();
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            /// <summary>
            /// Sets the NetworkAddress registry value of this adapter.
            /// </summary>
            /// <param name="value">The value. Should be EITHER a string of 12 hexadecimal digits, uppercase, without dashes, dots or anything else, OR an empty string (clears the registry value).</param>
            /// <returns>true if successful, false otherwise</returns>
            public bool SetRegistryMac(string value)
            {
                bool shouldReenable = false;

                try
                {
                    // If the value is not the empty string, we want to set NetworkAddress to it,
                    // so it had better be valid
                    if (value.Length > 0 && !Adapter.IsValidMac(value, false))
                        throw new Exception(value + " is not a valid mac address");

                    using (RegistryKey regkey = Registry.LocalMachine.OpenSubKey(this.RegistryKey, true))
                    {
                        if (regkey == null)
                            throw new Exception("Failed to open the registry key");

                        // Sanity check
                        if (regkey.GetValue("AdapterModel") as string != this.adaptername
                            && regkey.GetValue("DriverDesc") as string != this.adaptername)
                            throw new Exception("Adapter not found in registry");

                        // Attempt to disable the adepter
                        var result = (uint)adapter.InvokeMethod("Disable", null);
                        if (result != 0)
                            throw new Exception("Failed to disable network adapter.");

                        // If we're here the adapter has been disabled, so we set the flag that will re-enable it in the finally block
                        shouldReenable = true;

                        // If we're here everything is OK; update or clear the registry value
                        if (value.Length > 0)
                            regkey.SetValue("NetworkAddress", value, RegistryValueKind.String);
                        else
                            regkey.DeleteValue("NetworkAddress");


                        return true;
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return false;
                }

                finally
                {
                    if (shouldReenable)
                    {
                        uint result = (uint)adapter.InvokeMethod("Enable", null);
                        if (result != 0)
                            MessageBox.Show("Failed to re-enable network adapter.");
                    }
                }
            }
            public override string ToString()
            {
                return this.adaptername + this.customname;
            }

            /// <summary>
            /// Get a random (locally administered) MAC address.
            /// </summary>
            /// <returns>A MAC address having 01 as the least significant bits of the first byte, but otherwise random.</returns>
            public static string GetNewMac()
            {
                System.Random r = new System.Random();

                byte[] bytes = new byte[6];
                r.NextBytes(bytes);

                // Set second bit to 1
                bytes[0] = (byte)(bytes[0] | 0x02);
                // Set first bit to 0
                bytes[0] = (byte)(bytes[0] & 0xfe);

                return MacToString(bytes);
            }

            /// <summary>
            /// Verifies that a given string is a valid MAC address.
            /// </summary>
            /// <param name="mac">The string.</param>
            /// <param name="actual">false if the address is a locally administered address, true otherwise.</param>
            /// <returns>true if the string is a valid MAC address, false otherwise.</returns>
            public static bool IsValidMac(string mac, bool actual)
            {
                // 6 bytes == 12 hex characters (without dashes/dots/anything else)
                if (mac.Length != 12)
                    return false;

                // Should be uppercase
                if (mac != mac.ToUpper())
                    return false;

                // Should not contain anything other than hexadecimal digits
                if (!Regex.IsMatch(mac, "^[0-9A-F]*$"))
                    return false;

                if (actual)
                    return true;

                // If we're here, then the second character should be a 2, 6, A or E
                char c = mac[1];
                return (c == '2' || c == '6' || c == 'A' || c == 'E');
            }

            /// <summary>
            /// Verifies that a given MAC address is valid.
            /// </summary>
            /// <param name="mac">The address.</param>
            /// <param name="actual">false if the address is a locally administered address, true otherwise.</param>
            /// <returns>true if valid, false otherwise.</returns>
            public static bool IsValidMac(byte[] bytes, bool actual)
            {
                return IsValidMac(Adapter.MacToString(bytes), actual);
            }

            /// <summary>
            /// Converts a byte array of length 6 to a MAC address (i.e. string of hexadecimal digits).
            /// </summary>
            /// <param name="bytes">The bytes to convert.</param>
            /// <returns>The MAC address.</returns>
            public static string MacToString(byte[] bytes)
            {
                return BitConverter.ToString(bytes).Replace("-", "").ToUpper();
            }
        }

       

        public Form1()
        {
            InitializeComponent();

            // music function
            // init BASS using the default output device
            if (Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                // create a stream channel from a file
                int stream = Bass.BASS_MusicLoad("Music.xm", 0, 0, BASSFlag.BASS_DEFAULT, 0);
                if (stream != 0)
                {
                    // play the stream channel
                    Bass.BASS_ChannelPlay(stream, false);
                }

            }
            log.Text = log.Text + "Info: Greets to Vastyy , Ez , ALX , Danny_ , Enicalyth and Anti. Y'all are Epic!\n";
            log.Text = log.Text + "HWID Method: All Active MAC Address'es + Bluetooth\n\n";

            // check if program is running as admin (required to change HWID's and block Telemetry)

            if (IsCurrentUserAnAdmin() == false)
            {
                unban.Text = "Not running as admin!";
                unban.Enabled = false;
                log.Text = log.Text + "WARNING: You are not running this as Admin! THIS PROGRAM WILL NOT WORK AS NON-ADMIN\n";
            }
           
            
           

        }

        // Music Checkbox
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            
            switch (musicChkBx.Checked)
            {
                case true:
                    Bass.BASS_Start();
                    break;
                case false:
                    Bass.BASS_Pause();
                    break;
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://github.com/pogPoggers/");
        }

        // library stuff to get location of Studio Folder
        private static string getDirectory(params string[] paths)
        {
            string basePath = Path.Combine(paths);

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            return basePath;
        }
        public static string GetStudioDirectory()
        {
            string localAppData = Environment.GetEnvironmentVariable("LocalAppData");
            return getDirectory(localAppData, "Roblox Studio");
        }


        // unbanning function
        private void unban_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Are you sure you want to unban this computer? \nThis will change some HWID's in your system", "Are you sure?", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                log.Text = log.Text + "Info: Beginning to Unban\n";

                unban.Text = "Please Wait";
                unban.Enabled = false;

                // Uninstall Studio option
                if (uninstall.Checked == true) {
                    log.Text = log.Text + "Info: Uninstalling Studio..\n";

                    string localAppData2 = Environment.GetEnvironmentVariable("LocalAppData");
                    
                    try
                    {
                        var dir = new DirectoryInfo(localAppData2+"/Roblox");
                        dir.Attributes = dir.Attributes & ~FileAttributes.ReadOnly;
                        dir.Delete(true);
                        
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    log.Text = log.Text + "Info: Successfully Uninstalled Studio!\n";

                }

                // now, change all MAC address
                

                 // Windows internet bug!

                 /* Windows generally seems to add a number of non-physical devices, of which
                 * we would not want to change the address. Most of them have an impossible
                 * MAC address. */
                foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces().Where(
                        a => Adapter.IsValidMac(a.GetPhysicalAddress().GetAddressBytes(), true)
                    ).OrderByDescending(a => a.Speed))
                {
                    //AdaptersComboBox.Items.Add(new Adapter(adapter));
                   
                    if ( new Adapter(adapter).SetRegistryMac(Adapter.GetNewMac()) )
                    {
                        System.Threading.Thread.Sleep(100);
                        log.Text = log.Text + "Log: Successfully Set a MAC Address!\n";
                    }
                }


                // Block Telemetry option
                if (tele.Checked == true)
                {
                    //log.Text = log.Text + "Info: Blocking Studio Telemetry via HOSTS..\n";
                }

                MessageBox.Show("Completed Unbanning.\nPlease concider star'ing my project.", "Completed", MessageBoxButtons.OK);
            }
            else if (dialog == DialogResult.No)
            {
                //Application.Exit();
            }

        }
       
    }
}
