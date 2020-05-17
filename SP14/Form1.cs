using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSIS14
{
    // 14.3.1 Поиск в реесте значений, в том числе по шаблону
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            RegistryKey[] registryKeys = { Registry.ClassesRoot, Registry.CurrentConfig, Registry.CurrentUser,
                Registry.LocalMachine, Registry.PerformanceData, Registry.Users };
            if(checkBox1.Checked)
            {
                Regex regex = new Regex(textBox1.Text);
                foreach (var item in registryKeys)
                {
                    await Task.Run(() => search(item, regex));
                }
            }
            else
            {
                foreach (var item in registryKeys)
                {
                    await Task.Run(() => search(item, textBox1.Text));
                }
            }
        }
        async void search(RegistryKey key, Regex val)
        {
            try
            {
                var subs = key.GetSubKeyNames();
                if (subs.Length > 0)
                {
                    foreach (var subkeyName in subs)
                    {
                        var subkey = key.OpenSubKey(subkeyName);
                        if(subkey != null)
                            await Task.Run(() => search(subkey, val));
                    }
                }
                var values = key.GetValueNames();
                foreach (var v in values)
                {
                    var value = key.GetValue(v).ToString();
                    if (val.IsMatch(value))
                        await Task.Run(() =>
                        {
                            dataGridView1.Invoke(new Action(() =>
                            {
                                dataGridView1.Rows.Add(string.IsNullOrEmpty(v) ? "Default name" : v,
                                    key.GetValueKind(v), value);
                            }));
                            Thread.Sleep(10);
                        });
                }
               
            }
            catch
            {
            }
        }
        async void search(RegistryKey key, string val)
        {
            try
            {
                var subs = key.GetSubKeyNames();
                if (subs.Length > 0)
                {
                    foreach (var subkeyName in subs)
                    {
                        var subkey = key.OpenSubKey(subkeyName);
                        if (subkey != null)
                            await Task.Run(() => search(subkey, val));
                    }
                }
                var values = key.GetValueNames();
                foreach (var v in values)
                {
                    var value = key.GetValue(v).ToString();
                    if (val == value)
                        await Task.Run(() =>
                        {
                            dataGridView1.Invoke(new Action(() =>
                            {
                                dataGridView1.Rows.Add(string.IsNullOrEmpty(v) ? "Default name" : v,
                                    key.GetValueKind(v), value);
                            }));
                            Thread.Sleep(10);
                        });
                }

            }
            catch
            {
            }
        }
    }
}
