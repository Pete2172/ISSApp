using System;
using System.Collections.Generic;
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
using System.Drawing;
using System.IO;

namespace ISStrackApp
{
    /// <summary>
    /// Logika interakcji dla klasy MapDisplay.xaml
    /// </summary>
    public partial class MapDisplay : UserControl
    {

        internal class PathNode
        {
            public float Lat { get; private set; }
            public float Lon { get; private set; }
            public Ellipse point { get; private set; }
            public PathNode(float lat, float lon, Ellipse el)
            {
                Lat = lat;
                Lon = lon;
                point = el;
            }
        }
        private stationData data;
        /// <summary>
        /// Bitmaps needed to draw the world map and proper tokens
        /// </summary>
        Bitmap dark_map;
        Bitmap light_map;
        Bitmap sun_token;
        Bitmap iss_token;
        System.Windows.Controls.Image sun_image;
        System.Windows.Controls.Image iss_image;
        List<PathNode> path;
        int scale;
        /// <summary>
        /// To make better performance in drawing a day/night map, there is a need to archive previous coordinates
        /// </summary>
        float prev_lat; 
        float prev_lon;

        int width;
        int height;



        public MapDisplay()
        {
            InitializeComponent();
            path = new List<PathNode>();
            data = new stationData();
            InitializeMap();
            DrawMap();
            InitTokens();
            DrawISStoken();
        }
        public void InitializeMap()
        {
            width = 360;
            height = width / 2;
            map.Width = Width;
            map.Height = Height;
            scale = width / 360;
            dark_map = new Bitmap(Resource1.dark, 1800, 900);
            sun_token = new Bitmap(Resource1.sun, (width / 20),  (width / 20));
            iss_token = new Bitmap(Resource1.ISS, (width / 15), (width / 15));
        }
        public void InitTokens()
        {
            sun_image = new System.Windows.Controls.Image
            {
                Source = ChangeBitmapToBitmapImage(sun_token, System.Drawing.Imaging.ImageFormat.Png)
            };
            iss_image = new System.Windows.Controls.Image
            {
                Source = ChangeBitmapToBitmapImage(iss_token, System.Drawing.Imaging.ImageFormat.Png)
            };
            cnv.Children.Add(iss_image);
            cnv.Children.Add(sun_image);

        }
        public void SetNewData(stationData dat)
        {
            prev_lat = data.solar_Latitude;
            prev_lon = data.solar_Longitude;
            data = dat;
            DrawMap();
            AddPathPoint();
            DrawISStoken();
            DrawSun();
        }
        public void DrawMap()
        {
            if (Math.Abs(data.solar_Longitude - prev_lon) >= 1 || Math.Abs(data.solar_Latitude - prev_lat) >= 1)   // for better performance, there is a need to check the difference of coordinates (O(n^2) drawing) 
            {
                light_map = new Bitmap(Resource1.light, 1800, 900);
                int x, y;

                for (int i = 0; i < light_map.Height; i++)  // Drawing a day/night map depended on solar data
                {
                    for (int j = 0; j < light_map.Width; j++)
                    {
                        x = (j/5 >= 180) ? j/5 - 180 : -(180 - j/5);  // trasforming pixels coordinates to geographic coordinates
                        y = (i/5 <= 90) ? 90 - i/5 : -(i/5 - 90);  
                        if ((data.GetGreatCircleDistance((double)y, (double)x)) >= 90)
                        {
                            light_map.SetPixel(j, i, dark_map.GetPixel(j, i));  // changing daylight picture's pixels to nightmap pixels
                        }
                    }
                }
                map.Source = ChangeBitmapToBitmapImage(light_map, System.Drawing.Imaging.ImageFormat.Jpeg);  // changing the source of image control
                map.Width = Width;
                map.Height = Height;
            }
        }
        public void DrawSun()
        {
            double s = map.ActualWidth / 360;
            Canvas.SetLeft(sun_image, (map.ActualWidth/2 + data.solar_Longitude*s) - sun_token.Width/2);
            Canvas.SetTop(sun_image, (map.ActualHeight / 2 - data.solar_Latitude * s) - (sun_token.Height/2));

            if (SunCheckBox.IsChecked == true)
            {
                sun_image.Visibility = Visibility.Visible;
            }
            else
            {
                sun_image.Visibility = Visibility.Hidden;
            }
        }
        /// <summary>
        /// Changes bitmap object to a bitmapImage object
        /// </summary>
        /// <param name="map">Bitmap object</param>
        /// <param name="form">Format of an image</param>
        /// <returns></returns>
        public static BitmapImage ChangeBitmapToBitmapImage(Bitmap map, System.Drawing.Imaging.ImageFormat form)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                map.Save(memory, form);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }
        /// <summary>
        /// Draw ISS token, which can be visible on the day/night map
        /// </summary>
        public void DrawISStoken()
        {
            double s = map.ActualWidth / 360;   // calculating a scale 
            Canvas.SetLeft(iss_image, (map.ActualWidth / 2 + data.longitude*s) - iss_token.Width / 2);
            Canvas.SetTop(iss_image, map.ActualHeight / 2 - data.latitude*s - (iss_token.Height / 2));
        }

        private void SunCheckBoxOn(object sender, RoutedEventArgs e)
        {
            sun_image.Visibility = Visibility.Visible;
        }

        private void SunCheckBoxOff(object sender, RoutedEventArgs e)
        {
            sun_image.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Adding path point to the list 
        /// </summary>
        private void AddPathPoint()
        {
            PathNode node = new PathNode(data.latitude, data.longitude, new Ellipse  // creating a new node
            {
                Width = 2,
                Height = 2,
                Fill = (data.visibility == "eclipsed") ? System.Windows.Media.Brushes.Gray : System.Windows.Media.Brushes.Yellow // changing color of point depending on ISS visibility
            });
            path.Add(node);  // adding point to the list
            double s = map.ActualWidth / 360;
            Canvas.SetLeft(path[path.Count-1].point, (map.ActualWidth / 2 + data.longitude * s));
            Canvas.SetTop(path[path.Count-1].point, (map.ActualHeight / 2 - data.latitude * s));
        }
        /// <summary>
        /// Member function is redrawing the ISS' path after window's resize
        /// </summary>
        private void RedrawPath()
        {
            double s = map.ActualWidth / 360;
            cnvPath.Children.Clear();   // clearing canvas, which draws the path
            for (int i = 0; i < path.Count; i++)        // redrawing path with new coordinates
            {
                cnvPath.Children.Add(path[i].point);
                Canvas.SetLeft(path[i].point, (map.ActualWidth / 2 + path[i].Lon * s));
                Canvas.SetTop(path[i].point, (map.ActualHeight / 2 - path[i].Lat * s));
            }
        }
        private void TrackCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            cnvPath.Visibility = Visibility.Visible;
        }

        private void TrackCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            cnvPath.Visibility = Visibility.Hidden;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            map.Width = Width;
            map.Height = Height;
            cnv.Width = map.ActualWidth;
            cnv.Height = map.ActualHeight;
            cnvPath.Width = map.ActualWidth;
            DrawSun();
            DrawISStoken();
            RedrawPath();
        }
    }
}
