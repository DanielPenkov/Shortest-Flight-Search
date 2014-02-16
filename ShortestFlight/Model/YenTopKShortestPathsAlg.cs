using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShortestFlight.Model
{
    public class YenTopKShortestPathsAlg
    {
        private VariableGraph graph = new VariableGraph();


        private List<Path> result_list;
        private Dictionary<Path, airport> path_derivation_airport_index;
        private PriorityQueue<Path> path_candidates;


        private airport source_airport = null;
        private airport target_airport = null;


        public YenTopKShortestPathsAlg(GraphAirports graph)
        {
            result_list = new List<Path>();
            path_derivation_airport_index = new Dictionary<Path, airport>();
            path_candidates = new PriorityQueue<Path>();

            if (graph == null)
            {
                throw new ArgumentException("A NULL graph object occurs!");
            }

            graph = new VariableGraph((GraphAirports)graph);
            source_airport = null;
            target_airport = null;

            _init();
        }

        private void _init()
        {
            clear();
            if (source_airport != null && target_airport != null)
            {
                Path shortest_path = get_shortest_path(source_airport, target_airport);
                if (shortest_path.get_airports().Count > 0)
                {
                    path_candidates.add(shortest_path);
                    path_derivation_airport_index[shortest_path] = source_airport;
                }
            }
        }

        public void clear()
        {
            path_candidates.Clear();
            path_derivation_airport_index.Clear();
            result_list.Clear();
        }

        public Path get_shortest_path(airport source_vt, airport target_vt)
        {
            DijkstraShortestPathAlg dijkstra_alg = new DijkstraShortestPathAlg(graph);
            return dijkstra_alg.get_shortest_path(source_vt, target_vt);
        }


        public bool has_next()
        {
            return !path_candidates.isEmpty();
        }

        public Path next()
        {
            Path cur_path = path_candidates.poll();
            result_list.Add(cur_path);

            airport cur_derivation = path_derivation_airport_index[cur_path];

            int count = result_list.Count;

            for (int i = 0; i < count - 1; ++i)
            {
                Path cur_result_path = result_list.ElementAt(i);

                int cur_dev_airport_id =
                    cur_result_path.get_airports().IndexOf(cur_derivation);

                airport cur_succ_airport =
                    cur_result_path.get_airports().ElementAt(cur_dev_airport_id + 1);

                graph.remove_route(new KeyValuePair<int, int>(
                        cur_derivation.airport_ID, cur_succ_airport.airport_ID));
            }

            int path_length = cur_path.get_airports().Count;
            List<airport> cur_path_airport_list = cur_path.get_airports();
            for (int i = 0; i < path_length - 1; ++i)
            {
                graph.remove_airport(cur_path_airport_list.ElementAt(i).airport_ID);
                graph.remove_route(new KeyValuePair<int, int>(
                        cur_path_airport_list.ElementAt(i).airport_ID,
                        cur_path_airport_list.ElementAt(i + 1).airport_ID));
            }

            DijkstraShortestPathAlg reverse_tree = new DijkstraShortestPathAlg((VariableGraph)graph);
            reverse_tree.get_shortest_path_flower(target_airport);

            bool is_done = false;
            for (int i = path_length - 2; i >= 0 && !is_done; --i)
            {
                airport cur_recover_airport = cur_path_airport_list.ElementAt(i);
                graph.recover_removed_airport(cur_recover_airport.airport_ID);

                if (cur_recover_airport.airport_ID == cur_derivation.airport_ID)
                {
                    is_done = true;
                }
                Path sub_path = reverse_tree.update_cost_forward(cur_recover_airport);

                if (sub_path != null)
                {
                    double cost = 0;
                    List<airport> pre_path_list = new List<airport>();
                    reverse_tree.correct_cost_backward(cur_recover_airport);

                    for (int j = 0; j < path_length; ++j)
                    {
                        airport cur_airport = cur_path_airport_list.ElementAt(j);
                        if (cur_airport.airport_ID == cur_recover_airport.airport_ID)
                        {
                            j = path_length;
                        }
                        else
                        {
                            cost += graph.get_route_distance_of_graph(cur_path_airport_list.ElementAt(j),
                                    cur_path_airport_list.ElementAt(j + 1));
                            pre_path_list.Add(cur_airport);
                        }
                    }



                    pre_path_list.AddRange(sub_path.get_airports());

                    sub_path.set_distance(cost + sub_path.get_distance());
                    sub_path.get_airports().Clear();
                    sub_path.get_airports().AddRange(pre_path_list);

                    if (!path_derivation_airport_index.ContainsKey(sub_path))
                    {
                        path_candidates.add(sub_path);
                        path_derivation_airport_index[sub_path] = cur_recover_airport;
                    }
                }

                airport succ_airport = cur_path_airport_list.ElementAt(i + 1);
                graph.recover_removed_route(new KeyValuePair<int, int>(
                        cur_recover_airport.airport_ID, succ_airport.airport_ID));

                double cost_1 = graph.get_routes_distance(cur_recover_airport, succ_airport)
                    + reverse_tree.get_start_airport_distance_index()[succ_airport.airport_ID];

                if (reverse_tree.get_start_airport_distance_index()[cur_recover_airport.airport_ID] > cost_1)
                {
                    reverse_tree.get_start_airport_distance_index()[cur_recover_airport.airport_ID] = cost_1;
                    reverse_tree.get_predecessor_index()[cur_recover_airport.airport_ID] = succ_airport;
                    reverse_tree.correct_cost_backward(cur_recover_airport);
                }
            }

            graph.recover_removed_route();
            graph.recover_removed_airport();

            return cur_path;
        }

        public List<Path> get_shortest_paths(airport source_airport,
                airport target_airport, int top_k)
        {
            this.source_airport = source_airport;
            this.target_airport = target_airport;

            _init();
            int count = 0;
            while (has_next() && count < top_k)
            {
                next();
                ++count;
            }

            return result_list;
        }


        public List<Path> get_result_list()
        {
            return result_list;
        }

        public int get_cadidate_size()
        {
            return path_derivation_airport_index.Count;
        }
    }
}