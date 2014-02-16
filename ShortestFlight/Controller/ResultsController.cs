using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShortestFlight.DBLayer;
using ShortestFlight.Model;

namespace ShortestFlight.Controller
{
    public class ResultsController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        List<List<route>> allRoutes;
        private VariableGraph graph;
        private YenTopKShortestPathsAlg yenAlg;


        public List<List<route>> searchPaths(string from, string to)
        {
            int fromId = unitOfWork.getAirportsRepository().getAirportIdByAirport(from);
            int toId = unitOfWork.getAirportsRepository().getAirportIdByAirport(to);
            graph = new VariableGraph();
            yenAlg = new YenTopKShortestPathsAlg(graph);
            List<Path> shortest_paths_list = new List<Path>();

            shortest_paths_list = yenAlg.get_shortest_paths(
           graph.get_airport(fromId), graph.get_airport(toId), 10);

            List<List<route>> result = new List<List<route>>();

            foreach (Path path in shortest_paths_list)
            {
                result.Add(path.getRoutes());

            }

            List<List<route>> temmpAllRoutes = result;

 

            allRoutes = new List<List<route>>();

            foreach (List<route> a in temmpAllRoutes)
            {
             
                    allRoutes.Add(a);
                
            }
            return allRoutes;
        }

        public List<Result> getListResults(string from, string to)
        {
            allRoutes = searchPaths(from, to);

            List<Result> resultList = new List<Result>();

            for (int i = 0; i< allRoutes.Count; i++)
            {
               
                    List<route> lr = allRoutes[i];
                    Result rst = new Result(lr);
                    resultList.Add(rst);
                
            }
            return resultList;
        }
    }
}