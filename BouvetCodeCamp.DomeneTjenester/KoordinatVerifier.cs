using System;
using System.Linq;

using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.DomeneTjenester
{
    public class KoordinatVerifier : IKoordinatVerifier
    {
        public static double LongProximityThreshold = 10;
        public static double LatProximityThreshold = 10;
        public static double RadianceThreshold = 100; //meter
        
        public bool KoordinaterErNærHverandre(Koordinat first, Koordinat second)
        {
            if (KoordinaterErLike(first, second)) 
                return true;

            double firstLong, firstLat,
                secondLong, secondLat;

            Double.TryParse(first.Latitude, out firstLat);
            Double.TryParse(first.Longitude, out firstLong);
            Double.TryParse(second.Latitude, out secondLat);
            Double.TryParse(second.Longitude, out secondLong);

            var dist = this.distance(first.Y, first.X, second.Y, second.X, 'K');

            return dist*1000 <= RadianceThreshold;
        }

        // algoritme tatt fra
        // http://stackoverflow.com/a/14998816/1770699
        public bool KoordinatErInnenforPolygonet(Koordinat koordinat, Koordinat[] polygon)
        {
            if (polygon == null)
                return false;

            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < koordinat.Y && polygon[j].Y >= koordinat.Y || polygon[j].Y < koordinat.Y && polygon[i].Y >= koordinat.Y)
                {
                    if (polygon[i].X + (koordinat.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < koordinat.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //:::                                                                         :::
        //:::  This routine calculates the distance between two points (given the     :::
        //:::  latitude/longitude of those points). It is being used to calculate     :::
        //:::  the distance between two locations using GeoDataSource(TM) products    :::
        //:::                                                                         :::
        //:::  Definitions:                                                           :::
        //:::    South latitudes are negative, east longitudes are positive           :::
        //:::                                                                         :::
        //:::  Passed to function:                                                    :::
        //:::    lat1, lon1 = Latitude and Longitude of point 1 (in decimal degrees)  :::
        //:::    lat2, lon2 = Latitude and Longitude of point 2 (in decimal degrees)  :::
        //:::    unit = the unit you desire for results                               :::
        //:::           where: 'M' is statute miles                                   :::
        //:::                  'K' is kilometers (default)                            :::
        //:::                  'N' is nautical miles                                  :::
        //:::                                                                         :::
        //:::  Worldwide cities and other features databases with latitude longitude  :::
        //:::  are available at http://www.geodatasource.com                          :::
        //:::                                                                         :::
        //:::  For enquiries, please contact sales@geodatasource.com                  :::
        //:::                                                                         :::
        //:::  Official Web site: http://www.geodatasource.com                        :::
        //:::                                                                         :::
        //:::           GeoDataSource.com (C) All Rights Reserved 2014                :::
        //:::                                                                         :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

        private double distance(double lat1, double lon1, double lat2, double lon2, char unit) {
          double theta = lon1 - lon2;
          double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
          dist = Math.Acos(dist);
          dist = rad2deg(dist);
          dist = dist * 60 * 1.1515;
          if (unit == 'K') {
            dist = dist * 1.609344;
          } else if (unit == 'N') {
  	        dist = dist * 0.8684;
            }
          return (dist);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double deg2rad(double deg) {
          return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double rad2deg(double rad) {
          return (rad / Math.PI * 180.0);
        }

        private bool KoordinaterErLike(Koordinat first, Koordinat second)
        {
            return first.Latitude.Equals(second.Latitude) &&
                first.Longitude.Equals(second.Longitude);
        }
    }
}