using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;
using System.Drawing;
using System.ComponentModel;

namespace ISStrackApp
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        stationData Data;
        DispatcherTimer timer;
        BackgroundWorker worker;

        public MainWindow()
        {
            InitializeComponent();
            InitBackgroundWorkers();
            InitTimer();
            dataDisp.Width = this.Width / 4;
            mapDisp.Width = Width - dataDisp.Width;
            mapDisp.Height = Height - dataDisp.Height;
        }
        /// <summary>
        /// Initializes timer, which triggers downloading data, after counted time.
        /// </summary>
        void InitTimer()
        {
            Data = new stationData();  // creating new data object and timer object
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(Timer_Tick);   // Adding new TimerTick event
            timer.Interval = new TimeSpan(0, 0, 2);  // Defining amount of time
            timer.Start();
        }
        /// <summary>
        /// Member funtion initializes BackgroundWorker object, which serves to better performance of visualisation
        /// </summary>
        void InitBackgroundWorkers()
        {
            worker = new BackgroundWorker(); // create new object

            worker.DoWork += new DoWorkEventHandler(UpdateAllControls);     // adding RunWorker event
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UpdateFinished); // adding finishing event to worker
        }
        /// <summary>
        /// Shows message box with given content and type of message vox
        /// </summary>
        /// <param name="imag">Type of message box</param>
        /// <param name="text">Information</param>
        void ShowMessageBox(MessageBoxImage imag, string text)
        {
            StringBuilder information = new StringBuilder();
            string title = "";
            switch (imag)
            {
                case MessageBoxImage.Error:
                    title = "Error!";
                    break;
                case MessageBoxImage.Warning:
                    title = "Warning!";
                    break;
                case MessageBoxImage.Information:
                    title = "Info";
                    break;
            }
            information.AppendLine(text + "\n \n");
            MessageBox.Show(information.ToString(), title, MessageBoxButton.OK, imag);
        }
        /// <summary>
        /// Sends occured errors to a text file
        /// </summary>
        /// <param name="e">Exception, which occured</param>
        void SendToLog(Exception e)
        {
            DateTime currentTime = DateTime.Now;
            StringBuilder text = new StringBuilder();
            // building string 
            text.AppendLine("An error has occured on " + currentTime.ToLocalTime().ToString("yyyy-MM-dd at HH:mm:ss"));
            text.AppendLine("Details: ");
            text.AppendLine(e.ToString() + "\n");
            // writing created string to a file
            StreamWriter writeStream = new StreamWriter(Resource1.log, append: true);
            writeStream.WriteLine(text.ToString());
            writeStream.Close(); // closing a filestream
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            worker.RunWorkerAsync();
            timer.Stop();
        }

            /// <summary>
            /// Function is updating all controls in the window.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
        private void UpdateAllControls(object sender, DoWorkEventArgs e)
        {
            progBar.Dispatcher.Invoke(() => { progBar.Value = 0; });  // add value to progress bar
            Exception result = Data.DownloadISSData();  // downloading location of ISS
            if (result == null && Data.AreDataNew() == true)
            {
                dataDisp.Dispatcher.Invoke(() => { dataDisp.km_radio.IsEnabled = true; });
                dataDisp.Dispatcher.Invoke(() => { dataDisp.mil_radio.IsEnabled = true; });
                Exception res = Data.DownloadLocationData();    // downloading country data, where ISS is at
                progBar.Dispatcher.Invoke(() => { progBar.Value = 30; });
                if (res == null)
                {   // updating all controls
                    timer.Stop();
                    locDisp.Dispatcher.Invoke(() => { locDisp.SendNewData(Data); });
                    dataDisp.Dispatcher.Invoke(() => { dataDisp.SetNewData(Data); });
                    mapDisp.Dispatcher.Invoke(() => { mapDisp.SetNewData(Data); });
                    progBar.Dispatcher.Invoke(() => { progBar.Value = 70; });
                }
                else
                {
                    // when error happens- show messageBox and send failure text to a file
                    ShowMessageBox(MessageBoxImage.Error, "Something went wrong with finding a geolocation...");
                    SendToLog(res);
                }
            }
            else
            {
                ShowMessageBox(MessageBoxImage.Error, "You're not connected to the internet or the source webpage stopped work! \n Details in log.txt.");
                dataDisp.Dispatcher.Invoke(() => { dataDisp.km_radio.IsEnabled = false; });
                dataDisp.Dispatcher.Invoke(() => { dataDisp.mil_radio.IsEnabled = false; });
                SendToLog(result);
            }

        }
        private void UpdateFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            timer.Start();
            progBar.Dispatcher.Invoke(() => { progBar.Value = 100; });
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            dataDisp.Width = 150;
            dataDisp.Height = 450;
            dataDisp.HorizontalAlignment = HorizontalAlignment.Left;
            dataDisp.VerticalAlignment = VerticalAlignment.Top;
            mapDisp.Width = locDisp.Width;
            mapDisp.Height = locDisp.Height;
        }

    }
}
