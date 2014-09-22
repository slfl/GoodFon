using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net;
using HtmlAgilityPack; // Тут я подключаю HtmlAgilityPack

namespace GoodfonParser
{
    public partial class Form1 : Form // Тут у нас первый поток проги...
    {
        public Form1() { InitializeComponent(); }

        // Старт
        private void button1_Click(object sender, EventArgs e)
        {
            List<string> images = ParseLinks(listBox1.Text, Convert.ToInt32(textBox2.Text));
            foreach (string a in images)
            {
                DownloadImage(GetImage(a));
            } 
        }

        // Выход
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Стоп - Временно убиваю процесс проги. Доделаю стоп после многопоточности.
        private void button3_Click(object sender, EventArgs e)
        {
            //Environment.Exit(0);
        }

        // Инфа о говнокодерах
        private void button4_Click(object sender, EventArgs e)
        {
            // Немного благодарностей
            MessageBox.Show("Данную прогу написал Артур Буян.\nРеализация скриптов и моральная поддержка от Руслана Слинкова.\nТак-же спасибо rutracker.org за подсказку в написании проги.", "Информация о проге!");

            // Совсем чуть-чуть матов
            DialogResult result = MessageBox.Show("Ты пожертвовал денюшку на развитие моего говнокода?", "Скажи честно!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) //Если нажал Да
            {
                MessageBox.Show("Малорик!!! Уважаю!!!");
            }

            if (result == DialogResult.No) //Если нажал Нет
            {
                MessageBox.Show("Вот пидр...");
            }
        }
        
        // А эта функция принимает в качестве аргументов ссылку на категорию и количество страниц для парсинга.
        public List<string> ParseLinks(string category, int pages)
        {
            List<string> links = new List<string>();

            for (int i = 1; i <= pages; ++i)
            {
                HtmlAgilityPack.HtmlDocument HD = new HtmlAgilityPack.HtmlDocument();
                var web = new HtmlWeb
                {
                    AutoDetectEncoding = false,
                    OverrideEncoding = Encoding.UTF8, // Декодим в UTF-8
                };
                HD = web.Load(String.Format("{0}/index-{1}.html", category, i)); // Ссылка на категорию

                HtmlNodeCollection images = HD.DocumentNode.SelectNodes("//div[@class='tabl_td']");
                if (images != null)
                {
                    foreach (HtmlNode node in images)
                    {
                        HtmlNodeCollection link_tmp = node.SelectNodes("//a[@itemprop='url']");

                        foreach (HtmlNode node_tmp in link_tmp)
                        {
                            links.Add(node_tmp.Attributes["href"].Value.ToString()); // Ссылки
                        }
                    }
                }
            }

            return links;
        }

        // Парсим картинки
        public string GetImage(string url)
        {
            HtmlAgilityPack.HtmlDocument HD = new HtmlAgilityPack.HtmlDocument();
            var web = new HtmlWeb
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8, // Декодим в UTF-8
            };
            HD = web.Load(url);

            string imgurl = "";
            HtmlNodeCollection images = HD.DocumentNode.SelectNodes("//a[@target='_blank']");
            if (images != null)
            {
                foreach (HtmlNode node in images)
                {
                    if (node.Attributes["href"].Value.ToString().Contains("download.php")) // Страница, с которой выдираем пикчи
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
                OverrideEncoding = Encoding.UTF8, // Декодим в UTF-8
            };
            HD2 = web1.Load(String.Format("http://www.goodfon.ru/{0}", imgurl));

            HtmlNodeCollection image_final = HD2.DocumentNode.SelectNodes("//img[@border='1']");
            if (image_final != null)
            {
                foreach (HtmlNode node in image_final)
                {
                        return node.Attributes["src"].Value.ToString();
                        break;
                }
            }

            return "UNDERFINED";
        }

        // Качаем картинки
        public void DownloadImage(string img)
        {
            DirectoryInfo info = new DirectoryInfo("/"); // Пусть сохранения
            long uniqueKey = info.LastWriteTime.Ticks + 1L;
            string filename = Guid.NewGuid() + ".jpg"; // Расшрение пикчи

            WebClient webclient = new WebClient();
            webclient.DownloadFile(img, filename);
        }

        // Текст
        private void label1_Click(object sender, EventArgs e)
        {

        }

        // Количество страниц для парсинга
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        // Меню с выбором категории на пикчи
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Логинимся
        private void button5_Click(object sender, EventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://www.goodfon.ru/user/enter.php");
            request.ProtocolVersion = new Version("1.1");
            request.Method = "POST";
            request.KeepAlive = true;
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, ("ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4"));
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, ("gzip, deflate"));
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.66 Safari/537.36";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.CookieContainer = new CookieContainer();
            request.ContentType = "application/x-www-form-urlencoded";
            request.AllowAutoRedirect = false;

            string Login = "логин";
            string Password = "пасс";
            string authString = "login=" + Login + "&pas=" + Password;

            UTF8Encoding encodind = new UTF8Encoding();
            byte[] buffer = encodind.GetBytes(authString);
            using (Stream newStream = request.GetRequestStream())
            {
                newStream.Write(buffer, 0, authString.Length);
            }

            // авторизация

            CookieContainer cookies = new CookieContainer();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // запоминаем куки
                foreach (Cookie c in response.Cookies)
                {
                    cookies.Add(c);
                }
                // --

                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader strReader = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    richTextBox1.Text = strReader.ReadToEnd();
                }
            }
        }

    }

    public partial class Form2 : Form // Второй поток проги. БЛЯДОПРОГИ!
    {
        public Form2() { InitializeComponent(); }


    }
}
