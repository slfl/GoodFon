using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.IO;
using System.Net;
using System.Threading;


namespace WindowsFormsApplication1
{
    public partial class GoodFon : Form
    {
        public GoodFon()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        } //Форма
        private void textBox1_TextChanged(object sender, EventArgs e) //Ссылка на страницу
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e) //Кол-во страниц
        {

        }
        private void button1_Click(object sender, EventArgs e) //Кнопка выхода
        {
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e) //Кнопка инфы
        {
            MessageBox.Show("Программу написал Mansi");
            MessageBox.Show("За скрипты спасибо Руслану Слинкову");
        }
        private void button3_Click(object sender, EventArgs e) //Кнопка старта
        {
            List<string> images = ParseLinks(textBox1.Text, Convert.ToInt32(textBox2.Text));
            foreach (string a in images)
            {
                MessageBox.Show("Ну что, программа начала работать!");
                DownloadImage(GetImage(a));
            }
        }
        private void button4_Click(object sender, EventArgs e) //Кнопка остановки
        {
            //newThread.Abort();
        }

        public List<string> ParseLinks(string category, int pages)
        {
            List<string> links = new List<string>();

            for (int i = 1; i <= pages; ++i)
            {
                HtmlAgilityPack.HtmlDocument HD = new HtmlAgilityPack.HtmlDocument();
                var web = new HtmlWeb
                {
                    AutoDetectEncoding = false,
                    OverrideEncoding = Encoding.UTF8,
                };
                HD = web.Load(String.Format("{0}/index-{1}.html", category, i));

                HtmlNodeCollection images = HD.DocumentNode.SelectNodes("//div[@class='tabl_td']");
                if (images != null)
                {
                    foreach (HtmlNode node in images)
                    {
                        HtmlNodeCollection link_tmp = node.SelectNodes("//a[@itemprop='url']");

                        foreach (HtmlNode node_tmp in link_tmp)
                        {
                            links.Add(node_tmp.Attributes["href"].Value.ToString());
                        }
                    }
                }
            }

            return links;
        }

        public string GetImage(string url)
        {
            HtmlAgilityPack.HtmlDocument HD = new HtmlAgilityPack.HtmlDocument();
            var web = new HtmlWeb
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8,
            };
            HD = web.Load(url);

            string imgurl = "";
            HtmlNodeCollection images = HD.DocumentNode.SelectNodes("//a[@target='_blank']");
            if (images != null)
            {
                foreach (HtmlNode node in images)
                {
                    if (node.Attributes["href"].Value.ToString().Contains("download.php"))
                    {
                        imgurl = node.Attributes["href"].Value.ToString();
                        break;
                    }
                }
            }

            HtmlAgilityPack.HtmlDocument HD2 = new HtmlAgilityPack.HtmlDocument();
            var web1 = new HtmlWeb
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8,
            };
            HD2 = web1.Load(String.Format("http://www.goodfon.ru/{0}", imgurl));

            HtmlNodeCollection image_final = HD2.DocumentNode.SelectNodes("//img[@border='1']");
            if (image_final != null)
            {
                foreach (HtmlNode node in image_final)
                {
                    return node.Attributes["src"].Value.ToString();
                }
            }

            return "UNDERFINED";
        }

        public void DownloadImage(string img)
        {
            DirectoryInfo info = new DirectoryInfo("/");
            long uniqueKey = info.LastWriteTime.Ticks + 1L;
            string filename = Guid.NewGuid() + ".jpg";

            WebClient webclient = new WebClient();
            webclient.DownloadFile(img, filename);
        }
    }
}
