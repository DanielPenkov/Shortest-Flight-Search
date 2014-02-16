using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShortestFlight.DBLayer
{
    public class UnitOfWork
    {

        ShortestFlightEntities db = new ShortestFlightEntities();
        AirportsRepository airpRep;
        RoutesRepository routeRep;

        public static UnitOfWork instance;
        public static UnitOfWork Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UnitOfWork();
                }
                return instance;
            }
        }

 
        public AirportsRepository getAirportsRepository()
        {

            airpRep = new AirportsRepository(db);
            return airpRep;
        }

        public RoutesRepository getRoutesRepository()
        {
            routeRep = new RoutesRepository(db);
            return routeRep;
        }

    }
}