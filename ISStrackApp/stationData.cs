using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;





namespace ISStrackApp
{
    /// <summary>
    /// Class is gathering data, which are necessary to visualize location of International Space Station. Memeber functions are doing tasks like downloading data in json file, parsing it and formatting. 
    /// Class is also calculating values in different units.
    /// </summary>
    public class stationData
    {
        readonly double mile = 0.621371192f;
        public float longitude { get; private set; }
        public float latitude { get; private set; }
        public float solar_Latitude { get; private set; }
        public float solar_Longitude { get; private set; }
        float altitude;
        float velocity;
        int timestamp;
        public string visibility { get;  private set; }
        public string TimeZoneID { get; private set; }
        public string CountryCode { get; private set; }
        private string webpage;
        int old_timestamp;

        public stationData()
        {
        }
        /// <summary>
        /// Member function is connecting with a website, which is gathering necessary data. Informations are encrypted in json format. Function uses Newtonsoft libraries to receive data.
        /// </summary>
        /// <returns>Return an exception of failed operation or null value </returns>
        public Exception DownloadISSData()
        {

            WebClient wc = new WebClient();   // connecting to web
            string jsonFile ="";
            Exception exp = new Exception();

            try {
                jsonFile = wc.DownloadString("https://api.wheretheiss.at/v1/satellites/25544");   // copying website's json content to a string
            }
            catch (Exception e) // catch an exception, e.g. computer disconnected with internet
            {
                if (e is WebException || e is ArgumentNullException || e is NotSupportedException)
                {
                    return e;
                }
            } 
            if (!string.IsNullOrEmpty(jsonFile))  // parsing json data 
            {
                JObject jsonObject = JObject.Parse(jsonFile);
                old_timestamp = timestamp;
                string lat = jsonObject["latitude"].ToString().Replace(",", ".");   // getting proper values to variables
                string lon = jsonObject["longitude"].ToString().Replace(",", ".");
                webpage = "https://api.wheretheiss.at/v1/coordinates/" + lat + "," +  lon;
                longitude = float.Parse(jsonObject["longitude"].ToString());
                latitude = float.Parse(jsonObject["latitude"].ToString());
                velocity = float.Parse(jsonObject["velocity"].ToString());
                altitude = float.Parse(jsonObject["altitude"].ToString());
                timestamp = int.Parse(jsonObject["timestamp"].ToString());
                solar_Latitude = float.Parse(jsonObject["solar_lat"].ToString());
                solar_Longitude = float.Parse(jsonObject["solar_lon"].ToString());
                solar_Longitude = (solar_Longitude > 180) ? solar_Longitude - 360 : solar_Longitude;
                visibility = jsonObject["visibility"].ToString();
                return null;
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// Function finds location given by ISS' coordinates
        /// </summary>
        /// <returns>Exception after trying download data</returns>
        public Exception DownloadLocationData()
        {
            WebClient wc = new WebClient();   // connecting to web
            string jsonFile = "";

            try
            {
                jsonFile = wc.DownloadString(webpage);   // copy website json content to a string
            }
            catch (Exception e) // catch an exception, e.g. computer disconnected with internet
            {
                if (e is ArgumentNullException || e is NotSupportedException)
                {
                    return e;
                }
                if(e is WebException)       // website's error 500
                {
                    CountryCode = "None";  // that means- there is no country at these coordinates
                    TimeZoneID = "None";
                    return null;
                }
            }
            if (!string.IsNullOrEmpty(jsonFile))  // parsing json data 
            {
                JObject jsonObject = JObject.Parse(jsonFile);   // parsing country data
                CountryCode = jsonObject["country_code"].ToString();
                TimeZoneID = jsonObject["timezone_id"].ToString();
                return null;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Writes to a console all variables in a class
        /// </summary>
        public void WriteClassContent()
        {
            Console.WriteLine("Longitue: {0}", longitude);
            Console.WriteLine("Latitude {0}", latitude);
            Console.WriteLine("Solar_Latitude: {0} ", solar_Latitude);
            Console.WriteLine("Solar_Longitude: {0}", solar_Longitude);
            Console.WriteLine("Altitude {0}", altitude);
            Console.WriteLine("Velocity: {0}", velocity);
            Console.WriteLine("Timestamp: {0} ", GetDateTime());
            Console.WriteLine(GetLatitude());
            
        }
        /// <summary>
        /// Changes value of Latitude to string
        /// </summary>
        /// <returns>String with a proper geographical units</returns>
        public string GetLatitude()
        {
            StringBuilder text = new StringBuilder();
            if (latitude > 0) {
                text.Append(latitude.ToString("0.00") + "° N");
            }
            else {
                text.Append((-1 * latitude).ToString("0.00") + "° S");
            }
            return text.ToString();
        }
        /// <summary>
        /// Changes value of Longitude to string
        /// </summary>
        /// <returns>String with a proper geographical units</returns>
        public string GetLongitude()
        {
            StringBuilder text = new StringBuilder();
            if (longitude > 0)
            {
                text.Append(longitude.ToString("0.00") + "° E");
            }
            else
            {
                text.Append((-1 * longitude).ToString("0.00") + "° W");
            }
            return text.ToString();
        }
        /// <summary>
        /// Changes Altitude value to string
        /// </summary>
        /// <returns>String with altitude value with units in kilometers</returns>
        public string GetAltitudeInKm()
        {
            StringBuilder text = new StringBuilder();
            text.Append(altitude.ToString("0.00") + " km");
            return text.ToString();
        }
        /// <summary>
        /// Changes Altitude value to string
        /// </summary>
        /// <returns>String with altitude value with units in miles</returns>
        public string GetAltitudeInMiles()
        {
            StringBuilder text = new StringBuilder();
            text.Append((altitude* 0.621371192).ToString("0.00") + " miles");
            return text.ToString();
        }
        /// <summary>
        /// Changes velocity value to string
        /// </summary>
        /// <returns>String with velocity value with units in miles per hour</returns>
        public string GetVelocityInMPH()
        {
            StringBuilder text = new StringBuilder();
            text.Append((velocity * mile).ToString("0.00") + " mph");
            return text.ToString();
        }
        /// <summary>
        /// Changes velocity value to string
        /// </summary>
        /// <returns>String with velocity value with units in km per hour</returns>
        public string GetVelocityInKmPerHour()
        {
            StringBuilder text = new StringBuilder();
            text.Append((velocity).ToString("0.00") + " km/h");
            return text.ToString();
        }
        /// <summary>
        /// Changes downloaded unix timestamp to date format
        /// </summary>
        /// <returns>String with a proper date and hour</returns>
        public string GetDateTime()
        {
            DateTime localDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);  // creating a new DateTime object
            localDateTime = localDateTime.AddSeconds(timestamp); // adding downloaded timestamp to object

            return localDateTime.ToLocalTime().ToString("yyyy-MM-dd, HH:mm:ss "); // creating string based on DateTime class with given format
        }
        /// <summary>
        /// Member function  calculates a great-circle distance between solar lon/lat and given coordinates
        /// </summary>
        /// <param name="phi1">Latitude of given point (degrees)</param>
        /// <param name="l1">Longitude of given point (degrees)</param>
        /// <returns>Length of orthodoma in degrees</returns>
        public double GetGreatCircleDistance(double phi1, double l1)
        {
            double D, lambda, phi2;
            lambda = (solar_Longitude - l1)*Math.PI/180; // changing degrees to radians
            phi2 = (solar_Latitude * Math.PI) /180;
            phi1 = phi1 * Math.PI / 180;
            D = Math.Acos((Math.Sin(phi1)*Math.Sin(phi2)) + (Math.Cos(phi1)*Math.Cos(phi2)*Math.Cos(lambda)));  // calculating great circle distance
            D = D * 180 / Math.PI;  // changing to degrees
            return D;
        }
        /// <summary>
        /// Member functions checks, wheter downloaded data is different than the previous one
        /// </summary>
        /// <returns>True or false, depended on result</returns>
        public bool AreDataNew()  
        {
            if(timestamp != old_timestamp)
            {
                return true;
            }
            return false;
        }


    }
}
