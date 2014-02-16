using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ShortestFlight.Model
{
    public class Result
    {
        List<route> allRoutes;
        ShortestFlightEntities db;

        public Result(List<route> allRoutes)
        {
            this.allRoutes = allRoutes;
            findStartAirport(allRoutes);
            findEndAirport(allRoutes);
            calcStops(allRoutes);
            calcDistance(allRoutes);
            findFullRoute(allRoutes);
        }

        public int rid { get; set; }
        public List<String> FullRoute { get; set; }
        public string fromAirport { get; set; }
        public string toAirport { get; set; }
        public int stops { get; set; }
        public double distance { get; set; }



        public void findFullRoute(List<route> allRoutes)
        {
            FullRoute = new List<string>();

            foreach (route r in allRoutes)
            {
                db = new ShortestFlightEntities();
                int routeId = r.route_ID;
                int startAirportID = Int32.Parse(
                    db.routes.Where(rt => rt.route_ID == routeId).FirstOrDefault().from_OID.ToString());
                String airportNameFrom = db.airports.Where(a => a.airport_ID == startAirportID).FirstOrDefault().city;
                int toAirportID = Int32.Parse(
                  db.routes.Where(rt => rt.route_ID == routeId).FirstOrDefault().to_OID.ToString());
                String airportNameTo = db.airports.Where(a => a.airport_ID == toAirportID).FirstOrDefault().city;

                FullRoute.Add((airportNameFrom + " - "+ airportNameTo + "- " + (r.distance*1.61).ToString() + " km."));
            }
        }

        public void findStartAirport(List<route> allRoutes)
        {
            db = new ShortestFlightEntities();
            int startRoute = allRoutes[0].route_ID;
            int startAirportID = Int32.Parse(db.routes.Where(r => r.route_ID == startRoute).FirstOrDefault().from_OID.ToString());
            fromAirport = db.airports.Where(a => a.airport_ID == startAirportID).FirstOrDefault().city;

        }

        public void findEndAirport(List<route> allRoutes)
        {
            int lid = allRoutes[allRoutes.Count - 1].route_ID;
            db = new ShortestFlightEntities();
            int endAirportID = (int)db.routes.Where(r => r.route_ID == lid).FirstOrDefault().to_OID;
            toAirport = db.airports.Where(a => a.airport_ID == endAirportID).FirstOrDefault().city;
        }

        public void calcStops(List<route> allRoutes)
        {
            stops = allRoutes.Count - 1;
        }

        public void calcDistance(List<route> allRoutes)
        {
            double tempDistance = 0;

            foreach (route r in allRoutes)
            {
                tempDistance += (double)r.distance;
            }

            distance = tempDistance*1.61;
        }
    }
}
