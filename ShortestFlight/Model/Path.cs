using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShortestFlight.Model
{
    public class Path
    {
        List<airport> airports_list;
        double distance = -1;

        public Path()
        {
            airports_list = new List<airport>();
        }

        public Path(List<airport> airports_list, double distance)
        {
            this.airports_list = airports_list;
            this.distance = distance;
        }

        public double get_distance()
        {
            return distance;
        }

        public void set_distance(double distance)
        {
            this.distance = distance;
        }

        public List<airport> get_airports()
        {
            return airports_list;
        }

        public override int GetHashCode()
        {
            return airports_list.GetHashCode();
        }

        public override bool Equals(Object right)
        {
            if (right is Path)
            {
                Path r_path = right as Path;
                return airports_list.Equals(r_path.airports_list);
            }
            return false;
        }

        public List<route> getRoutes()
        {
            List<route> pathRoutes = new List<route>();


            for (int i = 0; i < airports_list.Count; i++)
            {
                route rt = null;

                if (i > 0)
                {

                    int f = airports_list[i - 1].airport_ID;
                    int t = airports_list[i].airport_ID;

                    ShortestFlightEntities db = new ShortestFlightEntities();
                    rt = db.routes.SingleOrDefault(
                        r => r.from_OID == f && r.to_OID == t);

                }

                if (rt != null)
                {

                    pathRoutes.Add(rt);
                }

            }

            return pathRoutes;

        }
    }
}