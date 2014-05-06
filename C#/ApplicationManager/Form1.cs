using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void PopulateApplications()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Aha! for .NET").OpenSubKey("ConsoleApp");
            string[] names = key.GetSubKeyNames();
            foreach (string name in names)
            {
                ListViewItem item = new ListViewItem(new string[3] { name, (string)key.OpenSubKey("Path").GetValue(name), (string)key.OpenSubKey("Settings").GetValue(name) });
                listView1.Items.Add(item);
            }
            key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Aha! for .NET").OpenSubKey("MuteApp");
            names = key.GetSubKeyNames();
            foreach (string name in names)
            {
                ListViewItem item = new ListViewItem(new string[3] { name, (string)key.OpenSubKey("Path").GetValue(name), (string)key.OpenSubKey("Settings").GetValue(name) });
                listView3.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListViewItem item = new ListViewItem(new string[3] { textBox1.Text, textBox2.Text, textBox3.Text });
            listView1.Items.Add(item);
            listView1.FocusedItem = item;
            listView1.Focus();
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Aha! for .NET\\ConsoleApp\\" + textBox1.Text, "Path", textBox2.Text);
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Aha! for .NET\\ConsoleApp\\" + textBox1.Text, "Settings", textBox3.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateApplications();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
            }

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
