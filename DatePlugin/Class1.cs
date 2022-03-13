using PluginInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace DatePlugin
{
    [Version(1, 3)]
    public class Class1 : IPlugin
    {
        public string Name
        {
            get
            {
                return "Поставить дату и геолокацию";
            }
        }

        public string Author
        {
            get
            {
                return "Васильев Игорь";
            }
        }

        public void Transform(Bitmap bitmap)
        {
            DateTime localDate = DateTime.Now;
            string text = "";
            bool OK_code = true;

            try
            {
                string IP = new WebClient().DownloadString("http://icanhazip.com/");
                string adress = "http://ipwhois.app/json/" + IP;
                HttpWebRequest location = (HttpWebRequest)WebRequest.Create(adress);
                HttpWebResponse response = (HttpWebResponse)location.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream stream = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream);
                    string URLString = sr.ReadToEnd();
                    dynamic d = JsonConvert.DeserializeObject<dynamic>(URLString);
                    text += d.country + ", " + d.city + ", ";
                }
            }
            catch (Exception ex) { OK_code = false; }


            int proc = text.Length;
            using (Graphics gr = Graphics.FromImage(bitmap))
            {

                text += localDate.ToString("H:mm, MM/dd/yyyy");
                const int fontSize = 22;
                if (OK_code == true)
                    gr.DrawString(text, new Font("Consolas", fontSize), new SolidBrush(Color.Black), bitmap.Width - 400-proc*12, bitmap.Height - 50);
                else
                    gr.DrawString(text, new Font("Consolas", fontSize), new SolidBrush(Color.Black), bitmap.Width - 300, bitmap.Height - 50);
            }
        }
    }

}
