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
    public partial class Form2 : Form
    {
        public Form2(Dictionary<string, IPlugin> plugins)
        {

            InitializeComponent();

            FindPlugins(plugins);

        }
        void FindPlugins(Dictionary<string, IPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                int row_num = dataGridView1.Rows.Add();
                dataGridView1.Rows[row_num].Cells["Плагин"].Value = plugin.Value.Name;
                dataGridView1.Rows[row_num].Cells["Автор"].Value = plugin.Value.Author;

                Type enumType = plugin.Value.GetType();
                if (enumType.IsDefined(typeof(VersionAttribute), false))
                {
                    System.Attribute[] attrs = System.Attribute.GetCustomAttributes(enumType);
                    foreach (System.Attribute attr in attrs)
                    {
                        if (attr is VersionAttribute)
                        {
                            VersionAttribute a = (VersionAttribute)attr;
                            dataGridView1.Rows[row_num].Cells["Версия"].Value = a.Major.ToString() + "." + a.Minor.ToString();
                        }
                    }
                }
            }
        }
    }

}
