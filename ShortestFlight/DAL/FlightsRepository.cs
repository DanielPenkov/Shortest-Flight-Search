using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShortestFlight.DBLayer
{
    
    public class RoutesRepository
    {

        ShortestFlightEntities db = new ShortestFlightEntities();
        public RoutesRepository(ShortestFlightEntities db)
        {
            this.db = db;
        
        }
        public List<route> getListRoutes(int routrId)
        {
        
            return  db.routes.Where(r => r.route_ID == routrId).ToList();
        
        }


    }
}