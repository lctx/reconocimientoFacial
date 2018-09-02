using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ClassLibrary1
{
    public partial class FaceBot : Form
    {
        // path video capturado anteriormente
        public string PathVideo;

        // Video Capture object.
        private static VideoCapture VideoCapture;

        // Cascade Classifier to detect Human faces.
        private static CascadeClassifier FrontalFaceCascadeClassifier;

        // LBPH Recognizer for face recognition.
        private static RecognizerEngine RecognizerEngine;

        // Database implementation for save/fetch operations.
        private static IDataStoreAccess DataStore;

        // Temporary values that store the most recent recognized user's information.
        public  string UserName = "";
        public static double Distance;

        // Accessed by the UserInfo form for saving the Image file to the database.
        public static Byte[] File;

        // Face detection & recognition timers. 
        public static System.Timers.Timer FacesDetectorTimer { get; set; }
        public static System.Timers.Timer FacesRecognizerTimer { get; set; }

        // Boolean values for making decisions in-code. 
        public static bool FacesDetected { get; set; }

        // Full & cropped version of each current frame for seamless access across multiple methods when detecting/recognizing faces.
        public static Image<Bgr, byte> CurrentFullFrame { get; set; }
        public static Image<Gray, byte> CurrentCroppedFrame { get; set; }

        // Detected Faces.
        public static Rectangle[] Faces { get; set; }

        // Recognized AND Unrecognized users.
        public static List<User> Users { get; set; }

        // Process ID of the Browser that has been launched, for shutting down when the user goes out of frame.
        public static int BrowserProcessID { get; set; }

        // Show FPS on the UI.
        public static int FPS;
        public static int AuthenticatedButUnrecognizedCount = 1;
        public FaceBot()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            Run();
        }
        public FaceBot(string pathVideo)
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            PathVideo= pathVideo;
            Run();
        }

        private void Run()
        {
            try
            { 
                //VideoCapture = new VideoCapture(@PathVideo);
                VideoCapture = new VideoCapture(Emgu.CV.CvEnum.CaptureType.OpenNI);
                MessageBox.Show(VideoCapture.CaptureSource.ToString());
                //VideoCapture = new VideoCapture(@"C:\Users\Carlos\Videos\VID_20180811_144136.mp4");
                //VideoCapture = new VideoCapture();
                VideoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, 720);
                VideoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, 1280);

                FrontalFaceCascadeClassifier = new CascadeClassifier(Application.StartupPath + Config.FaceHaarCascadeFilePath);
                RecognizerEngine = new RecognizerEngine(Config.DatabaseFilePath, Config.TrainingDataFilePath);
                DataStore = new DataStoreAccess(Config.DatabaseFilePath);

                //no se que hace este hilo :|
                FacesDetectorTimer = new System.Timers.Timer();
                FacesDetectorTimer.Interval = Config.FacesDetectorInterval;
                FacesDetectorTimer.Elapsed += FacesDetectorElapsed;
                FacesDetectorTimer.Start();

                FacesRecognizerTimer = new System.Timers.Timer();
                FacesRecognizerTimer.Interval = Config.FacesRecognizerInterval;
                FacesRecognizerTimer.Elapsed += FacesRecognizerElapsed;
                FacesRecognizerTimer.Start();

                FacesDetected = false;

                Users = new List<User>();

                Application.Idle += ProcessFrame;
            }
            catch (Exception)
            {
                // TODO: Log
                Application.Exit();
            }
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            try
            {
                CurrentFullFrame = VideoCapture.QueryFrame().ToImage<Bgr, Byte>();
                if (CurrentFullFrame != null)
                {
                    if (!FacesDetected)
                        DetectFaces();
                    else
                    {
                        foreach(var face in Faces)
                            CurrentFullFrame.Draw(face, new Bgr(Color.Red), 1);
                        string temp = ""; 
                        foreach (var item in Users)
                        {
                            temp += item.UserName + " - ";
                        }
                        //MessageBox.Show(temp);
                        foreach (var user in Users)
                        {
                            DialogResult sino = MessageBox.Show("Es usted "+ user.UserName+" ?", "Reconocimiento", MessageBoxButtons.YesNo);
                            if (sino == DialogResult.Yes)
                            {
                                UserName = user.UserName;
                                this.DialogResult = DialogResult.OK;
                                this.Dispose();
                                FacesDetectorTimer.Stop();
                                FacesRecognizerTimer.Stop();
                                VideoCapture.Stop();
                                break;
                            }
                            //CurrentFullFrame.Draw(user.UserName, new Point(user.Face.Left, user.Face.Top), Emgu.CV.CvEnum.FontFace.HersheySimplex, 1, new Bgr(Color.White), 2);
                            //CurrentFullFrame.Draw(user.Distance.ToString(), new Point(user.Face.Right, user.Face.Bottom), Emgu.CV.CvEnum.FontFace.HersheySimplex, 1, new Bgr(Color.White), 2);
                            //MessageBox.Show("Test " + user.UserName);
                        }
                    }

                    FPS++;
                    VideoFrameImageBox.Image = CurrentFullFrame;
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void FacesDetectorElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DetectFaces();

            FPSTextBox.Text = FPS.ToString();
            FPS = 0;
        }

        private static void DetectFaces()
        {
            var grayframe = CurrentFullFrame.Convert<Gray, byte>();
            Faces = FrontalFaceCascadeClassifier.DetectMultiScale(grayframe, 1.1, 10, Size.Empty);
            FacesDetected = Faces.Length > 0;
        }

        private void FacesRecognizerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Users.Clear();

            foreach (var face in Faces)
            {
                var grayframe = CurrentFullFrame.Convert<Gray, byte>();
                CurrentCroppedFrame = new Image<Gray, byte>(Crop(grayframe.ToBitmap(), face.Left, face.Top, face.Width, face.Height));

                var result = RecognizerEngine.RecognizeUser(CurrentCroppedFrame);
                Users.Add(new User
                {
                    UserName = result.UserName,
                    URL = result.URL,
                    Distance = result.Distance,
                    Face = face
                });
                if (result.UserName.Equals(Config.UnrecognizedUserName))
                {
                    AuthenticatedButUnrecognizedCount++;
                    if (AuthenticatedButUnrecognizedCount == 3)
                    {
                        AuthenticatedButUnrecognizedCount = 1;
                    }
                }

                // Recognized & valid user found.
                if (!result.UserName.Equals(Config.UnrecognizedUserName))
                {
                        if (result.UserName.Equals(UserName))  // Same user is still in front of the camera, continue.
                        {
                            UserName = result.UserName;
                            continue;
                        }

                }
            }
        }

        public static Bitmap Crop(Bitmap bm, int cropX, int cropY, int cropWidth, int cropHeight)
        {
            try
            {
                var rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);
                return bm.Clone(rect, bm.PixelFormat);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static void SaveFaceWithUserInfo(string username, string url, byte[] file)
        {
            try
            {
                bool saved = DataStore.SaveFace(username, url, file);
                RecognizerEngine.TrainRecognizer();
            }
            catch (Exception e)
            {
                return;
            }
        }

        private void AddUserButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Faces.Length > 0)
                {
                    var croppedImage = Crop(CurrentFullFrame.Convert<Gray, byte>().ToBitmap(), Faces[0].Left, Faces[0].Top, Faces[0].Width, Faces[0].Height);

                    var result = RecognizerEngine.RecognizeUser(new Image<Gray, byte>(croppedImage));
                    File = (byte[])new ImageConverter().ConvertTo(croppedImage, typeof(byte[]));
                    UserInfo userInfoForm = new UserInfo();
                    if (!result.UserName.Equals(Config.UnrecognizedUserName))
                        userInfoForm.Populate(result.UserName, result.URL);
                    userInfoForm.Show();
                    userInfoForm.Activate();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void FaceBotForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show(UserName);
            //Process.GetCurrentProcess().Kill();
        }

        private void FaceBot_Load(object sender, EventArgs e)
        {

        }
    }
}