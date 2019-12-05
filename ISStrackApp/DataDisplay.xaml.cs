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

namespace ISStrackApp
{
    /// <summary>
    /// Class creates an own control, which is displaying data of ISS' location.
    /// </summary>
    public partial class DataDisplay : UserControl
    {

        stationData handler;  // reference to object with gathered station's data

        public DataDisplay()
        {
            InitializeComponent();

        }
        /// <summary>
        /// Displays fresh data in labels
        /// </summary>
        /// <param name="dat">Object with gathered data</param>
        public void SetNewData(stationData dat)
        {
            handler = dat;
            lon_label.Text = dat.GetLongitude();  // changing labels' content
            lat_label.Text = dat.GetLatitude();
            time_label.Text = dat.GetDateTime();

            if (km_radio.IsChecked == true)  // checking radio buttons
            {
                vel_label.Text = dat.GetVelocityInKmPerHour();  // changing units to kilometers
                alt_label.Text = dat.GetAltitudeInKm();
            }
            else
            {
                vel_label.Text = dat.GetVelocityInMPH();  // changing units to miles
                alt_label.Text = dat.GetAltitudeInMiles();
            }
        }
        /// <summary>
        /// Event changes units of displayed data to miles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckMiles(object sender, RoutedEventArgs e)
        {
            if (handler != null)
            {
                vel_label.Text = handler.GetVelocityInMPH();
                alt_label.Text = handler.GetAltitudeInMiles();
            }
        }
        /// <summary>
        /// Event changes units of displayed data to kilometres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckKm(object sender, RoutedEventArgs e)
        {
            if (handler != null)
            {
                vel_label.Text = handler.GetVelocityInKmPerHour();
                alt_label.Text = handler.GetAltitudeInKm();
            }
        }
    }
}
