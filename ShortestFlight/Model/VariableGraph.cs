using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShortestFlight.Model
{
    public class VariableGraph : GraphAirports
    {
        List<int> rem_airport_id_set;
        List<KeyValuePair<int, int>> rem_route_set;

        public VariableGraph()
            : base()
        {
            rem_airport_id_set = new List<int>();
            rem_route_set = new List<KeyValuePair<int, int>>();

        }

        public VariableGraph(GraphAirports graph)
            : base(graph)
        {
            rem_airport_id_set = new List<int>();
            rem_route_set = new List<KeyValuePair<int, int>>();
        }


        public void remove_route(KeyValuePair<int, int> route)
        {
            rem_route_set.Add(route);
        }


        public void remove_airport(int airport_id)
        {
            rem_airport_id_set.Add(airport_id);
        }

        public void recover_removed_route()
        {
            rem_route_set.Clear();
        }

        public void recover_removed_route(KeyValuePair<int, int> route)
        {
            rem_route_set.Remove(route);
        }

        public void recover_removed_airport()
        {
            rem_airport_id_set.Clear();
        }

        public void recover_removed_airport(int airport_id)
        {
            rem_airport_id_set.Remove(airport_id);
        }


        public override double get_routes_distance(airport source, airport sink)
        {
            int source_id = source.airport_ID;
            int sink_id = sink.airport_ID;

            if (rem_airport_id_set.Contains(source_id) || rem_airport_id_set.Contains(sink_id)
                || rem_route_set.Contains(new KeyValuePair<int, int>(source_id, sink_id)))
            {
                return GraphAirports.DISCONNECTED;
            }
            return base.get_routes_distance(source, sink);
        }


        public double get_route_distance_of_graph(airport source, airport sink)
        {
            return base.get_routes_distance(source, sink);
        }


        public override List<airport> get_adjacent_airports(airport airport)
        {
            List<airport> ret_set = new List<airport>();
            int starting_airport_id = airport.airport_ID;
            if (!rem_airport_id_set.Contains(starting_airport_id))
            {
                List<airport> adj_airports_set = base.get_adjacent_airports(airport);
                foreach (airport cur_airport in adj_airports_set)
                {
                    int ending_airport_id = cur_airport.airport_ID;
                    if (rem_airport_id_set.Contains(ending_airport_id)
                    || rem_route_set.Contains(
                        new KeyValuePair<int, int>(starting_airport_id, ending_airport_id)))
                    {
                        continue;
                    }

                    // 
                    ret_set.Add(cur_airport);
                }
            }
            return ret_set;
        }


        public override List<airport> get_precedent_airports(airport airport)
        {
            List<airport> ret_set = new List<airport>();
            if (!rem_airport_id_set.Contains(airport.airport_ID))
            {
                int ending_airport_id = airport.airport_ID;
                List<airport> pre_airports_set = base.get_precedent_airports(airport);
                foreach (airport cur_airport in pre_airports_set)
                {
                    int starting_airport_id = cur_airport.airport_ID;
                    if (rem_airport_id_set.Contains(starting_airport_id)
                    || rem_route_set.Contains(
                        new KeyValuePair<int, int>(starting_airport_id, ending_airport_id)))
                    {
                        continue;
                    }

                    //
                    ret_set.Add(cur_airport);
                }
            }
            return ret_set;
        }

        public override List<airport> get_airports_list()
        {
            List<airport> ret_list = new List<airport>();
            foreach (airport cur_airport in base.get_airports_list())
            {
                if (rem_airport_id_set.Contains(cur_airport.airport_ID)) continue;
                ret_list.Add(cur_airport);
            }
            return ret_list;
        }


        public override airport get_airport(int id)
        {
            if (rem_airport_id_set.Contains(id))
            {
                return null;
            }
            else
            {
                return base.get_airport(id);
            }
        }







    }
}