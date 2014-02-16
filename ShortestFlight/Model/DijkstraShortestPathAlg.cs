using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShortestFlight.Model
{
    public class DijkstraShortestPathAlg
    {
        GraphAirports graph = null;

        List<airport> determined_airports_set;
        VPriorityQueue<airport> airports_candidate_queue;
        Dictionary<int, Double> start_airport_distance_index;
        Dictionary<int, airport> predecessor_index;


        public DijkstraShortestPathAlg(GraphAirports graph)
        {
            determined_airports_set = new List<airport>();
            airports_candidate_queue = new VPriorityQueue<airport>();
            start_airport_distance_index = new Dictionary<int, double>();
            predecessor_index = new Dictionary<int, airport>();
            this.graph = graph;
        }

        public void clear()
        {
            determined_airports_set.Clear();
            airports_candidate_queue.Clear();
            start_airport_distance_index.Clear();
            predecessor_index.Clear();
        }

        public Dictionary<int, double> get_start_airport_distance_index()
        {
            return start_airport_distance_index;
        }

        public Dictionary<int, airport> get_predecessor_index()
        {
            return predecessor_index;
        }

 

        public void get_shortest_path_flower(airport root)
        {
            determine_shortest_paths(null, root, false);
        }

        protected void determine_shortest_paths(airport source_airport,
            airport sink_airport, bool is_source2sink)
        {
            clear();


            airport end_airport = is_source2sink ? sink_airport : source_airport;
            airport start_airport = is_source2sink ? source_airport : sink_airport;
            start_airport_distance_index[start_airport.airport_ID] = 0.0;
            start_airport.set_distance(0.0);
            airports_candidate_queue.Add(start_airport);

            while (!airports_candidate_queue.IsEmpty())
            {
                airport cur_candidate = airports_candidate_queue.Poll();

                if (cur_candidate.Equals(end_airport)) break;

                determined_airports_set.Add(cur_candidate);

                improve_to_airport(cur_candidate, is_source2sink);
            }
        }

        private void improve_to_airport(airport airport, bool is_source2sink)
        {

            List<airport> neighbor_airports_list = is_source2sink ?
                graph.get_adjacent_airports(airport) : graph.get_precedent_airports(airport);

            foreach (airport cur_adjacent_airport in neighbor_airports_list)
            {
                if (determined_airports_set.Contains(cur_adjacent_airport)) continue;

                double distance = start_airport_distance_index.ContainsKey(airport.airport_ID) ?
                    start_airport_distance_index[airport.airport_ID] : GraphAirports.DISCONNECTED;

                distance += is_source2sink ? graph.get_routes_distance(airport, cur_adjacent_airport)
                    : graph.get_routes_distance(cur_adjacent_airport, airport);

                if (!start_airport_distance_index.ContainsKey(cur_adjacent_airport.airport_ID)
                || start_airport_distance_index[cur_adjacent_airport.airport_ID] > distance)
                {
                    start_airport_distance_index[cur_adjacent_airport.airport_ID] = distance;

                    predecessor_index[cur_adjacent_airport.airport_ID] = airport;

                    cur_adjacent_airport.set_distance(distance);
                    airports_candidate_queue.Add(cur_adjacent_airport);
                }
            }
        }


        public Path get_shortest_path(airport source_airport, airport sink_airport)
        {
            determine_shortest_paths(source_airport, sink_airport, true);

           
            List<airport> airports_list = new List<airport>();
            double distance = start_airport_distance_index.ContainsKey(sink_airport.airport_ID) ?
                start_airport_distance_index[sink_airport.airport_ID] : GraphAirports.DISCONNECTED;
            if (distance != GraphAirports.DISCONNECTED)
            {
                airport cur_airport = sink_airport;
                do
                {
                    airports_list.Add(cur_airport);
                    cur_airport = predecessor_index[cur_airport.airport_ID];
                } while (cur_airport != null && cur_airport != source_airport);

                airports_list.Add(source_airport);
                airports_list.Reverse();
            }
            return new Path(airports_list, distance);
        }

        public Path update_cost_forward(airport airport)
        {
            double cost = GraphAirports.DISCONNECTED;

            List<airport> adj_vertex_set = graph.get_adjacent_airports(airport);

            if (!start_airport_distance_index.ContainsKey(airport.airport_ID))
            {
                start_airport_distance_index[airport.airport_ID] = GraphAirports.DISCONNECTED;
            }

            foreach (airport cur_airport in adj_vertex_set)
            {
                double distance = start_airport_distance_index.ContainsKey(cur_airport.airport_ID) ?
                        start_airport_distance_index[cur_airport.airport_ID] : GraphAirports.DISCONNECTED;

                distance += graph.get_routes_distance(airport, cur_airport);

                double cost_of_vertex = start_airport_distance_index[airport.airport_ID];
                if (cost_of_vertex > distance)
                {
                    start_airport_distance_index[airport.airport_ID] = distance;
                    predecessor_index[airport.airport_ID] = cur_airport;
                    cost = distance;
                }
            }

            Path sub_path = null;
            if (cost < GraphAirports.DISCONNECTED)
            {
                sub_path = new Path();
                sub_path.set_distance(cost);
                List<airport> airports_list = sub_path.get_airports();
                airports_list.Add(airport);

                airport sel_airport = predecessor_index[airport.airport_ID];


                while (predecessor_index.ContainsKey(sel_airport.airport_ID))
                {
                    airports_list.Add(sel_airport);
                    sel_airport = predecessor_index[sel_airport.airport_ID];
                }
                airports_list.Add(sel_airport);

            }

            return sub_path;
        }

        public void correct_cost_backward(airport airport)
        {
            List<airport> airports_list = new List<airport>();
            airports_list.Add(airport);

            while (airports_list.Count > 0)
            {
                airport cur_airport = airports_list[0];
                airports_list.RemoveAt(0);
                double cost_of_cur_airport = start_airport_distance_index[cur_airport.airport_ID];

                List<airport> pre_airport_set = graph.get_precedent_airports(cur_airport);
                foreach (airport pre_airport in pre_airport_set)
                {
                    double cost_of_pre_airport = start_airport_distance_index.ContainsKey(pre_airport.airport_ID) ?
                            start_airport_distance_index[pre_airport.airport_ID] : GraphAirports.DISCONNECTED;

                    double fresh_cost = cost_of_cur_airport + graph.get_routes_distance(pre_airport, cur_airport);

                    if (cost_of_pre_airport > fresh_cost)
                    {
                        start_airport_distance_index[pre_airport.airport_ID] = fresh_cost;
                        predecessor_index[pre_airport.airport_ID] = cur_airport;
                        airports_list.Add(pre_airport);
                    }
                }
            }
        }
    }
}