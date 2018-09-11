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
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Collections;

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
            //Process.Start(@"C:\Users\Carlos\Documents\proyectos\kinect\Nueva carpeta\FaceBot-master\FaceBot\WindowsFormsApp1\facebot\Facebot.exe");
            //Process.Start(@"%SystemRoot%\system32\calc.exe");
            //Process.Start(@"C:\Users\Carlos\source\repos\ScreenRecorderLib-master\TestConsoleApp\bin\Debug\TestConsoleApp.exe");
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            //startInfo.FileName = @"C:\Users\Carlos\source\repos\ScreenRecorderLib-master\TestConsoleApp\bin\Debug\ScreenRecorder.exe";
            startInfo.FileName = @"C:\Users\Carlos\Documents\proyectos\kinect\Nueva carpeta\FaceBot-master\FaceBot\FaceBot\bin\Release\FaceBot.exe";
            //startInfo.FileName = @"cmd";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //startInfo.Arguments = @" start -C " + '\u0022' + @"C:\Users\Carlos\Documents\proyectos\kinect\Nueva carpeta\FaceBot-master\FaceBot\FaceBot\bin\Release\FaceBot.exe'" + '\u0022';
            //startInfo.Arguments = @"C:\Users\Carlos\source\repos\ScreenRecorderLib-master\TestConsoleApp\bin\Debug\TestConsoleApp.exe";
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using-statement will close.
                Process exeProcess = Process.Start(startInfo);
                Process[] localByName = Process.GetProcessesByName("FaceBot");
                int tiempo = 0;
                while (localByName.Length > 0)
                {
                    localByName = Process.GetProcessesByName("FaceBot");
                    tiempo += 1;
                    Thread.Sleep(1000);
                }
                //leer archivo
                StreamReader objReader = new StreamReader(System.IO.Path.GetTempPath() + @"\Archivo.dat");
                string sLine = "";
                ArrayList arrText = new ArrayList();

                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                        arrText.Add(sLine);
                }
                objReader.Close();

                foreach (string sOutput in arrText)
                {
                    if (sOutput.Equals("NO RECONOCE"))
                    {
                        Console.WriteLine("El rostro no es reconocible por favor repita el proceso con los adecuados parametros o registrese");
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(sOutput);
                    }
                    //Console.WriteLine(sOutput);
                    //Console.ReadLine();
                }
                    //System.Windows.Forms.MessageBox.Show("tiempo de reconocimiento= "+tiempo);
                    //socketEscucha = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //direccion = new IPEndPoint(IPAddress.Any, 9000);

                    //try
                    //{
                    //    socketEscucha.Bind(direccion);
                    //    socketEscucha.Listen(1);
                    //    //labelStatus.Content = "Escuchando";
                    //    socket2 = socketEscucha.Accept();
                    //    recibido = new byte[1500];
                    //    int a = socket2.Receive(recibido, 0, recibido.Length, 0);
                    //    //socket2.BeginReceiveFrom(recibido, 0, recibido.Length, SocketFlags.None, ref direccion, new AsyncCallback(MessageCallBack), recibido);
                    //    Array.Resize(ref recibido, a);
                    //    string sad = Encoding.Default.GetString(recibido);
                    //    System.Windows.Forms.MessageBox.Show(sad);
                    //    //labelRespuesta.Content = sad;
                    //}
                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show(ex.Message);
                    //}
                }
            catch
            {
                // Log error.
            }
        }
        public void ProcessExited()
        {

        }
    }
}
