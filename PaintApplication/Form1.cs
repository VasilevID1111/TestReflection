using PluginInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaintApplication
{
   
    public partial class Form1 : Form
    {
        public const string FileName = "config.txt";
        bool File_Plugins = false;
        Dictionary<string, IPlugin> plugins = new Dictionary<string, IPlugin>();
        public Form1()
        {
            InitializeComponent();

            FindPlugins();
            CreatePluginsMenu();

        }

        void FindPlugins()
        {
            try
            {

                if (File.Exists(FileName))
                {
                    string[] lines = File.ReadAllLines(FileName);
                    if (lines[0].Contains('0'))
                    {
                        File_Plugins = true;
                        for (int i = 1; i < lines.Length; i++)
                        {
                            try
                            {
                                string s = Path.GetFullPath(lines[i]);
                                Assembly assembly = Assembly.LoadFile(s + ".dll");

                                foreach (Type type in assembly.GetTypes())
                                {
                                    Type iface = type.GetInterface("PluginInterface.IPlugin");

                                    if (iface != null)
                                    {
                                        IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                                        plugins.Add(plugin.Name, plugin);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Ошибка загрузки плагина\n" + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        FindAllPlugins();
                    }
                } else
                {
                    FindAllPlugins();
                }
            } catch (Exception e) { MessageBox.Show(e.Message); }
        }

        void FindAllPlugins()
        {
            // папка с плагинами
            string folder = System.AppDomain.CurrentDomain.BaseDirectory;

            // dll-файлы в этой папке
            string[] files = Directory.GetFiles(folder, "*.dll");

            foreach (string file in files)
                try
                {
                    Assembly assembly = Assembly.LoadFile(file);

                    foreach (Type type in assembly.GetTypes())
                    {
                        Type iface = type.GetInterface("PluginInterface.IPlugin");

                        if (iface != null)
                        {
                            IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                            plugins.Add(plugin.Name, plugin);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки плагина\n" + ex.Message);
                }
        }

        void CreatePluginsMenu()
        {
            foreach (IPlugin p in plugins.Values)
            {
                var menuItem = new ToolStripMenuItem(p.Name);
                menuItem.Click += OnPluginClick;

                плагиныToolStripMenuItem.DropDownItems.Add(menuItem);
            }

        }

        private void OnPluginClick(object sender, EventArgs args)
        {
            IPlugin plugin = plugins[((ToolStripMenuItem)sender).Text];
            plugin.Transform((Bitmap)pictureBox1.Image);
            pictureBox1.Refresh();
        }

        private void оПлагинахToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form dlg1 = new Form2(plugins);
            dlg1.ShowDialog();
        }
    }
}
