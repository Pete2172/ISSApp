using Microsoft.Maps.MapControl.WPF;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ISStrackApp
{
    /// <summary>
    /// Logika interakcji dla klasy LocationDisplay.xaml
    /// </summary>
    public partial class LocationDisplay : UserControl
    {
        stationData handler;
        Location location;

        public LocationDisplay()
        {
            InitializeComponent();
            location = new Location();  // creates a new location object 

        }
        /// <summary>
        /// Sends a new data to bing maps obejct
        /// </summary>
        /// <param name="dat"></param>
        public void SendNewData(stationData dat)
        {
            handler = dat;
            CountryCodeBox.Text = handler.CountryCode;  // changing labels 
            TimeZoneBox.Text = handler.TimeZoneID;
            location.Latitude = handler.latitude; // adding coordinates of ISS to the location object
            location.Longitude = handler.longitude;
            Map.Center = location;  // changing location of map centering point
            Pin.Location = location;   // changing location of the pin in bing maps object
        }

        private void MapLoaded(object sender, RoutedEventArgs e)
        {

        }
        
    }
}
