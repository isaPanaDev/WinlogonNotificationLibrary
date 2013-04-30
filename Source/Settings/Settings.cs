using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Security.AccessControl;
using System.Drawing.Text;
namespace Settings
{
    public partial class Settings : Form
    {

        #region Declaration and Initialize
        public Settings()
        {
            InitializeComponent();

        }
        static DataSet ds = new DataSet();

        void DefaultColorSetting()
        {
            txtResult.Visible = false;
            if (checkBox_logon.Checked == false)
            {
                panel_Logon.Enabled = false;
                panel_Logon.BackColor = Color.LightGray;
            }
            else
            {
                panel_Logon.Enabled = true;
                panel_Logon.BackColor = Color.LightSteelBlue;
            }
            if (checkBox_logoff.Checked == false)
            {
                panel_logoff.Enabled = false;
                panel_logoff.BackColor = Color.LightGray;

            }
            else
            {
                panel_logoff.Enabled = true;
                panel_logoff.BackColor = Color.LightSteelBlue;
            }
            if (checkBox_displaylock.Checked == false)
            {
                panel_displaylock.Enabled = false;
                panel_displaylock.BackColor = Color.LightGray;
            }
            else
            {
                panel_displaylock.Enabled = true;
                panel_displaylock.BackColor = Color.LightSteelBlue;
            }

            if (checkBox_displayunlock.Checked == false)
            {
                panel_displayunlock.Enabled = false;
                panel_displayunlock.BackColor = Color.LightGray;
            }
            else
            {
                panel_displayunlock.Enabled = true;
                panel_displayunlock.BackColor = Color.LightSteelBlue;
            }

            if (checkBox_servicestart.Checked == false)
            {
                panel_servicestart.Enabled = false;
                panel_servicestart.BackColor = Color.LightGray;
            }
            else
            {
                panel_servicestart.Enabled = true;
                panel_servicestart.BackColor = Color.LightSteelBlue;
            }

            if (checkBox_servicestop.Checked == false)
            {
                panel_servicestop.Enabled = false;
                panel_servicestop.BackColor = Color.LightGray;
            }
            else
            {
                panel_servicestop.Enabled = true;
                panel_servicestop.BackColor = Color.LightSteelBlue;
            }

        }
        #endregion

        #region Event Handler for File Selection Buttons
        private void btnlogon_Click(object sender, EventArgs e)
        {


            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            txtLogonfilepath.Text = ofd.FileName;
            ofd.Dispose();

        }

        private void btnlogoff_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            txtLogofffilepath.Text = ofd.FileName;
            ofd.Dispose();

        }

        private void btnlock_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            txtLockfilepath.Text = ofd.FileName;
            ofd.Dispose();

        }

        private void btnunlock_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            txtUnlockfilepath.Text = ofd.FileName;
            ofd.Dispose();

        }

        private void btnstart_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            txtStartupfilepath.Text = ofd.FileName;
            ofd.Dispose();
        }

        private void btnstop_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            txtShutdownfilepath.Text = ofd.FileName;
            ofd.Dispose();

        }


        #endregion

        #region Load Current Settings from XML into UI
        private void Settings_Load(object sender, EventArgs e)
        {

            LoadXMLintoSettingsForm();

        }

        private void LoadXMLintoSettingsForm()
        {
            //Load the existing xml file data//

            ds.Clear();
            string xmlPath1 = System.Configuration.ConfigurationManager.AppSettings["EventSubscriptionXMLPath"].ToString();

            ds.ReadXml(xmlPath1, XmlReadMode.ReadSchema);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["Enable"].ToString().ToLower() == "true")
                {
                    switch (dr["EventName"].ToString())
                    {
                        case "logon": panel_Logon.Enabled = true;
                            checkBox_logon.Checked = true;

                            if (dr["TargetWindow"].ToString() == @"WinSta0\Winlogon")
                            {
                                radioButton1.Enabled = true;
                            }
                            else radioButton2.Enabled = true;
                            txtLogonfilepath.Text = dr["ExecutePath"].ToString();

                            LogoncomboBox.SelectedIndex = Convert.ToInt16(dr["Delaytime"].ToString());
                            break;
                        case "logoff": panel_logoff.Enabled = true;
                            checkBox_logoff.Checked = true;
                            if (dr["TargetWindow"].ToString() == @"WinSta0\Winlogon")
                            {
                                radioButton3.Enabled = true;
                            }
                            else radioButton4.Enabled = true;
                            txtLogofffilepath.Text = dr["ExecutePath"].ToString();
                            logoffcomboBox.SelectedIndex = Convert.ToInt16(dr["Delaytime"].ToString());
                            break;
                        case "displaylock": panel_displaylock.Enabled = true;
                            checkBox_displaylock.Checked = true;
                            if (dr["TargetWindow"].ToString() == @"WinSta0\Winlogon")
                            {
                                radioButton5.Enabled = true;
                            }
                            else radioButton6.Enabled = true;
                            txtLockfilepath.Text = dr["ExecutePath"].ToString();
                            lockcomboBox.SelectedIndex = Convert.ToInt16(dr["Delaytime"].ToString());
                            break;
                        case "displayunlock": panel_displayunlock.Enabled = true;
                            checkBox_displayunlock.Checked = true;
                            if (dr["TargetWindow"].ToString() == @"WinSta0\Winlogon")
                            {
                                radioButton7.Enabled = true;
                            }
                            else radioButton8.Enabled = true;
                            txtUnlockfilepath.Text = dr["ExecutePath"].ToString();
                            unlockcomboBox.SelectedIndex = Convert.ToInt16(dr["Delaytime"].ToString());
                            break;
                        case "servicestart": panel_servicestart.Enabled = true;
                            checkBox_servicestart.Checked = true;

                            if (dr["TargetWindow"].ToString() == @"WinSta0\Winlogon")
                            {
                                radioButton9.Enabled = true;
                            }
                            else radioButton10.Checked = true;
                            txtStartupfilepath.Text = dr["ExecutePath"].ToString();
                            //startupcomboBox.Text = dr["Delaytime"].ToString();
                            startupcomboBox.SelectedIndex = Convert.ToInt16(dr["Delaytime"].ToString()) ;
                            break;
                        case "servicestop": panel_servicestop.Enabled = true;
                            checkBox_servicestop.Checked = true;
                            if (dr["TargetWindow"].ToString() == @"WinSta0\Winlogon")
                            {
                                radioButton11.Enabled = true;
                            }
                            else radioButton12.Enabled = true;
                            txtShutdownfilepath.Text = dr["ExecutePath"].ToString();
                           // shutdowncomboBox.Text = dr["Delaytime"].ToString();
                            shutdowncomboBox.SelectedIndex = Convert.ToInt16(dr["Delaytime"].ToString());
                            break;

                    }

                }
            }
            DefaultColorSetting();

        }
        #endregion

        #region Event Handlers for Enable/Disable CheckBoxes
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_logon.Checked == false)
            {
                panel_Logon.Enabled = false;
                panel_Logon.BackColor = Color.LightGray;
            }
            else
            {
                panel_Logon.Enabled = true;
                panel_Logon.BackColor = Color.LightSteelBlue;
            }
        }
        private void checkBox_logoff_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_logoff.Checked == false)
            {
                panel_logoff.Enabled = false;
                panel_logoff.BackColor = Color.LightGray;

            }
            else
            {
                panel_logoff.Enabled = true;
                panel_logoff.BackColor = Color.LightSteelBlue;
            }
        }

        private void checkBox_displaylock_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_displaylock.Checked == false)
            {
                panel_displaylock.Enabled = false;
                panel_displaylock.BackColor = Color.LightGray;
            }
            else
            {
                panel_displaylock.Enabled = true;
                panel_displaylock.BackColor = Color.LightSteelBlue;
            }
        }

        private void checkBox_displayunlock_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_displayunlock.Checked == false)
            {
                panel_displayunlock.Enabled = false;
                panel_displayunlock.BackColor = Color.LightGray;
            }
            else
            {
                panel_displayunlock.Enabled = true;
                panel_displayunlock.BackColor = Color.LightSteelBlue;
            }
        }

        private void checkBox_servicestart_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_servicestart.Checked == false)
            {
                panel_servicestart.Enabled = false;
                panel_servicestart.BackColor = Color.LightGray;
            }
            else
            {
                panel_servicestart.Enabled = true;
                panel_servicestart.BackColor = Color.LightSteelBlue;
            }
        }

        private void checkBox_servicestop_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_servicestop.Checked == false)
            {
                panel_servicestop.Enabled = false;
                panel_servicestop.BackColor = Color.LightGray;
            }
            else
            {
                panel_servicestop.Enabled = true;
                panel_servicestop.BackColor = Color.LightSteelBlue;
            }
        }
        #endregion

        #region Validate & Save Settings to XML DB
            public static Boolean savexml()
            {//Save to XML DB//
                try
                {
                    ds.WriteXml(System.Configuration.ConfigurationManager.AppSettings["EventSubscriptionXMLPath"].ToString(), XmlWriteMode.WriteSchema);
                    return true;
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Event Subscription XML Path is either not valid or missing on the configuration file. Please Check..\\n " + ex.Message.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }

            bool validatepath(bool Flags)
            {
                //validate File path //
                if ((panel_Logon.Enabled == true) && (String.IsNullOrEmpty(txtLogonfilepath.Text) == true))
                {
                    panel_Logon.BackColor = Color.Salmon;
                    MessageBox.Show("Logon Application path is Empty! Please select an application path", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Flags = false;
                }

                else if ((panel_Logon.Enabled == true) && (File.Exists(txtLogonfilepath.Text) == false))
                {
                    panel_Logon.BackColor = Color.Salmon;
                    MessageBox.Show("Logon Application path is invalid!!! Please check the path", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Flags = false;
                }

                if ((panel_logoff.Enabled == true) && (String.IsNullOrEmpty(txtLogofffilepath.Text) == true))
                {
                    panel_Logon.BackColor = Color.Salmon;
                    MessageBox.Show("LogOff Application path is Empty! Please select an application path", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Flags = false;
                }
                else if ((panel_logoff.Enabled == true) && (File.Exists(txtLogofffilepath.Text) == false))
                {
                    panel_logoff.BackColor = Color.Salmon;
                    MessageBox.Show("LogOff Application path is invalid!!! Please check the path", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Flags = false;
                }
                if ((panel_displaylock.Enabled == true) && (String.IsNullOrEmpty(txtLockfilepath.Text) == true))
                {
                    panel_displaylock.BackColor = Color.Salmon;
                    MessageBox.Show("Display Lock Application path is Empty! Please select an application path", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Flags = false;
                }
                else if ((panel_displaylock.Enabled == true) && (File.Exists(txtLockfilepath.Text) == false))
                {
                    panel_displaylock.BackColor = Color.Salmon;
                    MessageBox.Show("Display Lock Application path is invalid!!! Please check the path", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Flags = false;
                }
                if ((panel_displayunlock.Enabled == true) && (String.IsNullOrEmpty(txtUnlockfilepath.Text) == true))
                {
                    panel_displayunlock.BackColor = Color.Salmon;
                    MessageBox.Show("Display Unlock Application path is Empty! Please select an application path", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Flags = false;
                }
                else if ((panel_displayunlock.Enabled == true) && (File.Exists(txtUnlockfilepath.Text) == false))
                {
                    panel_displayunlock.BackColor = Color.Salmon;
                    MessageBox.Show("Display Unlock Application path is invalid!!! Please check the path", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Flags = false;
                }
                if ((panel_servicestart.Enabled == true) && (String.IsNullOrEmpty(txtStartupfilepath.Text) == true))
                {
                    panel_servicestart.BackColor = Color.Salmon;
                    MessageBox.Show("System Start Application path is Empty! Please select an application path", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Flags = false;
                }
                else if ((panel_servicestart.Enabled == true) && (File.Exists(txtStartupfilepath.Text) == false))
                {
                    panel_servicestart.BackColor = Color.Salmon;
                    MessageBox.Show("System Start Application path is invalid!!! Please check the path", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Flags = false;
                }
                if ((panel_servicestop.Enabled == true) && (String.IsNullOrEmpty(txtShutdownfilepath.Text) == true))
                {
                    panel_servicestop.BackColor = Color.Salmon;
                    MessageBox.Show("System Shutdown Application path is Empty! Please select an application path", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Flags = false;
                }
                else if ((panel_servicestop.Enabled == true) && (File.Exists(txtShutdownfilepath.Text) == false))
                {
                    panel_servicestop.BackColor = Color.Salmon;
                    MessageBox.Show("System Shutdown Application path is invalid!!! Please check the path", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Flags = false;
                }
                return (Flags);
            }

            private void save_Click(object sender, EventArgs e)
            {
                txtResult.Visible = false;
                DefaultColorSetting();

                try
                {
                    bool Flags = true;
                    Flags = validatepath(Flags);
                    if (Flags == true)
                    {
                        //Add the data to dataset//
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            switch (dr["EventName"].ToString())
                            {
                                case "logon": if (panel_Logon.Enabled == true)
                                    {
                                        dr["Enable"] = true;
                                   
                                        dr["ExecutePath"] = txtLogonfilepath.Text;
                                        dr["Delaytime"] = LogoncomboBox.SelectedItem.ToString();
                                        if (radioButton1.Checked == true)
                                            dr["TargetWindow"] = @"WinSta0\Winlogon";
                                        else
                                            dr["TargetWindow"] = @"WinSta0\Default";
                                    }
                                    else
                                    {
                                        panel_Logon.BackColor = Color.LightGray;
                                        dr["Enable"] = false;
                                        dr["ExecutePath"] = "";
                                        dr["TargetWindow"] = "";
                                    }
                                    break;
                                case "logoff": if (panel_logoff.Enabled == true)
                                    {
                                        dr["Enable"] = true;

                                        dr["ExecutePath"] = txtLogofffilepath.Text;
                                        dr["Delaytime"] = logoffcomboBox.SelectedItem.ToString();
                                        if (radioButton3.Checked == true)
                                            dr["TargetWindow"] = @"WinSta0\Winlogon";

                                        else
                                            dr["TargetWindow"] = @"WinSta0\Default";
                                    }
                                    else
                                    {
                                        dr["Enable"] = false;
                                        panel_logoff.BackColor = Color.LightGray;
                                        dr["ExecutePath"] = "";
                                        dr["TargetWindow"] = "";
                                    }
                                    break;

                                case "displaylock": if (panel_displaylock.Enabled == true)
                                    {
                                        dr["Enable"] = true;

                                        dr["ExecutePath"] = txtLockfilepath.Text;
                                        dr["Delaytime"] =lockcomboBox.SelectedItem.ToString();
                                        if (radioButton5.Checked == true)
                                            dr["TargetWindow"] = @"WinSta0\Winlogon";
                                        else
                                            dr["TargetWindow"] = @"WinSta0\Default";
                                    }
                                    else
                                    {
                                        dr["Enable"] = false;
                                        panel_displaylock.BackColor = Color.LightGray;
                                        dr["ExecutePath"] = "";
                                        dr["TargetWindow"] = "";
                                    }
                                    break;
                                case "displayunlock": if (panel_displayunlock.Enabled == true)
                                    {
                                        dr["Enable"] = true;

                                        dr["ExecutePath"] = txtUnlockfilepath.Text;
                                        dr["Delaytime"] = unlockcomboBox.SelectedItem.ToString();
                                        if (radioButton7.Checked == true)
                                            dr["TargetWindow"] = @"WinSta0\Winlogon";
                                        else
                                            dr["TargetWindow"] = @"WinSta0\Default";
                                    }
                                    else
                                    {
                                        dr["Enable"] = false;
                                        panel_displayunlock.BackColor = Color.LightGray;
                                        dr["ExecutePath"] = "";
                                        dr["TargetWindow"] = "";
                                    }
                                    break;
                                case "servicestart": if (panel_servicestart.Enabled == true)
                                    {
                                        dr["Enable"] = true;

                                        dr["ExecutePath"] = txtStartupfilepath.Text;
                                        dr["Delaytime"] = startupcomboBox.SelectedItem.ToString();
                                        if (radioButton9.Checked == true)
                                            dr["TargetWindow"] = @"WinSta0\Winlogon";
                                        else
                                            dr["TargetWindow"] = @"WinSta0\Default";
                                    }
                                    else
                                    {
                                        dr["Enable"] = false;
                                        panel_servicestart.BackColor = Color.LightGray;
                                        dr["ExecutePath"] = "";
                                        dr["TargetWindow"] = "";
                                    }
                                    break;
                                case "servicestop": if (panel_servicestop.Enabled == true)
                                    {
                                        dr["Enable"] = true;

                                        dr["ExecutePath"] = txtShutdownfilepath.Text;
                                        dr["Delaytime"] = shutdowncomboBox.SelectedItem.ToString();
                                        if (radioButton11.Checked == true)
                                            dr["TargetWindow"] = @"WinSta0\Winlogon";
                                            
                                        else
                                            dr["TargetWindow"] = @"WinSta0\Default";
                                    }
                                    else
                                    {
                                        dr["Enable"] = false;
                                        panel_servicestop.BackColor = Color.LightGray;
                                        dr["ExecutePath"] = "";
                                        dr["TargetWindow"] = "";
                                    }
                                    break;
                            }
                        }
                        if (savexml() == true)
                        {
                            txtResult.Visible = true;


                            MessageBox.Show("Event Subscription Xml Database has been updated", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("exception caught: " + exp, "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }

            private void cancel_Click(object sender, EventArgs e)
            {
                this.Close();
            }


        #endregion

           


    }
}
