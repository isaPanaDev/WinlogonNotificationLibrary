using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Win7EventsLibrary
{
    public partial class Settings : Form
    {
        
        
        public Settings()
        {
            InitializeComponent();
        }

        private void btnlogon_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath.ToString();

        }

        private void btnlogoff_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath.ToString();

        }

        private void btnlock_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath.ToString();

        }

        private void btnunlock_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath.ToString();

        }

        private void btnstart_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath.ToString();

        }

        private void btnstop_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath.ToString();

        }

       

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_logon.Checked == false)
            { panel1.Enabled = false; }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            panel1.Enabled = false;
        }

        private void checkBox_logoff_CheckedChanged(object sender, EventArgs e)
        {

        }

        

       
        

        
       

        
    }
}
