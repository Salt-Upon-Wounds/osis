using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SP13
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var processes = Process.GetProcesses();
            foreach(var proc in processes)
            {
                listBox1.Items.Add(new ProcessItem(proc));                
            }
            contextMenuStrip1.ItemClicked += ContextMenuStrip1_ItemClicked;
            listBox1.ContextMenuStrip = contextMenuStrip1;
            listBox1.MouseUp += ListBox1_MouseUp;
        }

        private void ListBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                listBox1.SelectedIndex = listBox1.IndexFromPoint(e.X, e.Y);
            }
        }

        private void ContextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                var i = listBox1.SelectedIndex;
                var temp = (listBox1.SelectedItem as ProcessItem).Process;
                Enum.TryParse<ProcessPriorityClass>(e.ClickedItem.Text, out ProcessPriorityClass priority);
                temp.PriorityClass = priority;
                listBox1.Items.Remove(listBox1.SelectedItem);
                listBox1.Items.Insert(i, new ProcessItem(temp));
                listBox1.SelectedIndex = i;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
                
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                listBox2.Items.Clear();
                var process = (listBox1.SelectedItem as ProcessItem).Process;
                try
                {
                    foreach (var module in process.Modules)
                    {
                        listBox2.Items.Add(module.ToString());
                    }
                }
                catch (Exception ex)
                {
                    listBox2.Items.Add(ex.Message);
                }
            }
        }

        private void belowNormalToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
    class ProcessItem
    {
        public ProcessItem(Process _process)
        {
            Process = _process;
        }
        public Process Process { get; set; }
        public override string ToString()
        {
            try
            {
                return $"{Process.ProcessName} Priority:{Process.PriorityClass.ToString()}";
            }
            catch
            {
                return $"{Process.ProcessName}";
            }     
        }
    }
}
