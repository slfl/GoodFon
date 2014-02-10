using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        }

        private void button1_Click(object sender, EventArgs e) //Кнопка выхода
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) //Кнопка инфы
        {
            MessageBox.Show("Программу написал Mansi");
        }

        private void button3_Click(object sender, EventArgs e) //Кнопка старта
        {
            MessageBox.Show("Ну что, программа начала работать!");
        }
    }
}
