using FaceBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FaceBot;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Creamos el delegado 
            ThreadStart delegado = new ThreadStart(iniciarHilo);
            //Creamos la instancia del hilo 
            Thread hilo = new Thread(delegado);
            //Iniciamos el hilo 
            hilo.Start();
            //Thread.Sleep(10000);
            //hilo.Abort();
        }
        private void iniciarHilo()
        {
            FaceBot.FaceBot faceBot = new FaceBot.FaceBot((@"C:\Users\Carlos\Videos\VID_20180811_144136.mp4"));
            faceBot.ShowDialog();
                //faceBot = null;
                return;
        }
    }
}
