using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceBot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        FaceBot faceBot;


        private void button1_Click(object sender, EventArgs e)
        {
            //Creamos el delegado 
            ThreadStart delegado = new ThreadStart(iniciarHilo);
            //Creamos la instancia del hilo 
            Thread hilo = new Thread(delegado);
            //Iniciamos el hilo 
            //dfs
            hilo.Start();
            //Thread.Sleep(10000);
            //hilo.Abort();
        }

        private void iniciarHilo()
        {
            faceBot = new FaceBot();
            if (faceBot.ShowDialog() == DialogResult.OK)
            {
                label1.Text = faceBot.UserName;
                faceBot = null;
                return;
            }
        }

    }
}
