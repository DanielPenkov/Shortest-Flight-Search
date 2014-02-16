using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ShortestFlight.Model
{
    public class GraphAirports
    {
        public static double DISCONNECTED;


        protected Dictionary<int, List<airport>> fanout_airports_index;
        protected Dictionary<int, List<airport>> fanin_airports_index;
        protected Dictionary<KeyValuePair<int, int>, double> airports_pair_distance_index;
        protected Dictionary<int, airport> id_airports_index;
        protected List<airport> airports_list;
        public int airports_num;
        public int routes_num;
        private List<route> routesArray;
        airport a;
        ShortestFlightEntities db = new ShortestFlightEntities();
        List<route> fullRoutes;

        List<airport> ar;

        public GraphAirports()
        {
            DISCONNECTED = double.MaxValue;

            fanout_airports_index = new Dictionary<int, List<airport>>();
            fanin_airports_index = new Dictionary<int, List<airport>>();
            airports_pair_distance_index = new Dictionary<KeyValuePair<int, int>, double>();
            id_airports_index = new Dictionary<int, airport>();
            airports_list = new List<airport>();
            airports_num = 0;
            routes_num = 0;

            import_data();

        }


        public GraphAirports(GraphAirports graph)
        {
            DISCONNECTED = double.MaxValue;

            // index of fan-outs of one vertex
            fanout_airports_index = new Dictionary<int, List<airport>>(graph.fanin_airports_index);

            fanin_airports_index = new Dictionary<int, List<airport>>(graph.fanin_airports_index);

            airports_pair_distance_index = new Dictionary<KeyValuePair<int, int>, double>(graph.airports_pair_distance_index);

            // index for vertices in the graph
            id_airports_index = new Dictionary<int, airport>(graph.id_airports_index);

            // list of vertices in the graph 
            airports_list = new List<airport>(graph.airports_list);

            // the number of vertices in the graph
            airports_num = graph.airports_num;

            // the number of arcs in the graph
            routes_num = graph.routes_num;
        }





        public void Clear()
        {
            airports_num = 0;
            routes_num = 0;
            airports_list.Clear();
            id_airports_index.Clear();
            fanin_airports_index.Clear();
            fanout_airports_index.Clear();
            airports_pair_distance_index.Clear();
        }



        protected void add_route(int start_airport_id, int end_airport_id, double distance)
        {

            if (!id_airports_index.ContainsKey(start_airport_id)
            || !id_airports_index.ContainsKey(end_airport_id)
            || start_airport_id == end_airport_id)
            {
                throw new System.ArgumentOutOfRangeException("The edge from " + start_airport_id
                        + " to " + end_airport_id + " does not exist in the graph.");
            }

            // update the adjacent-list of the graph
            List<airport> fanout_airports_set;
            if (fanout_airports_index.ContainsKey(start_airport_id))
            {
                fanout_airports_set = fanout_airports_index[start_airport_id];
            }
            else
            {
                fanout_airports_set = new List<airport>();
                fanout_airports_index.Add(start_airport_id, fanout_airports_set);
            }
            fanout_airports_set.Add(id_airports_index[end_airport_id]);

            //
            List<airport> fanin_airports_set;
            if (fanin_airports_index.ContainsKey(end_airport_id))
            {
                fanin_airports_set = fanin_airports_index[end_airport_id];
            }
            else
            {
                fanin_airports_set = new List<airport>();
                fanin_airports_index.Add(end_airport_id, fanin_airports_set);
            }
            fanin_airports_set.Add(id_airports_index[start_airport_id]);

            // store the new edge 
            airports_pair_distance_index.Add(new KeyValuePair<int, int>(start_airport_id, end_airport_id),
                distance);

            ++routes_num;

        }


        public virtual List<airport> get_adjacent_airports(airport airport)
        {
            return fanout_airports_index.ContainsKey(airport.airport_ID)
                ? fanout_airports_index[airport.airport_ID]
                : new List<airport>();
        }







        public virtual List<airport> get_precedent_airports(airport airport)
        {
            return fanin_airports_index.ContainsKey(airport.airport_ID)
                ? fanin_airports_index[airport.airport_ID]
                : new List<airport>();
        }

        public virtual double get_routes_distance(airport source, airport sink)
        {
            KeyValuePair<int, int> Key = new KeyValuePair<int, int>(source.airport_ID, sink.airport_ID);
            return (airports_pair_distance_index.ContainsKey(Key)) ?
                airports_pair_distance_index[Key] : DISCONNECTED;
        }


        public virtual void set_airports_num(int num)
        {
            airports_num = num;
        }


        public virtual List<airport> get_airports_list()
        {
            return airports_list;
        }


        public virtual airport get_airport(int id)
        {
            return id_airports_index[id];
        }



        public void getAllroutes()
        {
            ShortestFlightEntities db = new ShortestFlightEntities();
            fullRoutes = new List<route>();
            fullRoutes = db.routes.ToList();

        }


        public void getAllAirports()
        {
            ShortestFlightEntities db = new ShortestFlightEntities();
            ar = new List<airport>();
            ar = db.airports.ToList();
        }



        public void import_data()
        {



            Thread t1 = new Thread(getAllroutes);
            t1.Start();
            Thread t2 = new Thread(getAllAirports);
            t2.Start();
            
            t1.Join();
            t2.Join();







            airports_num = ar.Count;


            for (int i = 0; i < airports_num; ++i)
            {
                airport airport = ar[i];
                airports_list.Add(airport);
                id_airports_index[airport.airport_ID] = airport;
            }


            foreach (airport a in airports_list)
            {

                foreach (route r in fullRoutes)
                {

                    if (r.from_OID == a.airport_ID)
                    {
                        int start_airport_id = (int)r.from_OID;
                        int end_airport_id = (int)r.to_OID;
                        double distance = (double)r.distance;
                        KeyValuePair<int, int> Key = new KeyValuePair<int, int>(start_airport_id, end_airport_id);

                        if (!airports_pair_distance_index.ContainsKey(Key))
                        {

                            add_route(start_airport_id, end_airport_id, distance);


                        }
                    }



                }


            }
        }




    }
}