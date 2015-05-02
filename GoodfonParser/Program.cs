using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using System.IO;
using System.Net;
using System.Text;

namespace GoodfonParser
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Thread thread1 = new Thread(FormParser); //Создаем первый поток
            thread1.Start(); //Запускаем поток

            Thread thread2 = new Thread(ParserScript); //Создаем второй поток
            thread2.Start(); //Запускаем поток
        }

        static void FormParser() // Первый поток
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void ParserScript() // Второй поток
        {
            //Application.Run(new Form2());

        }
    }
}