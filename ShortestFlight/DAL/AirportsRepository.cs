using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ShortestFlight.DBLayer
{
    public class AirportsRepository
    {
        ShortestFlightEntities db;

        public AirportsRepository(ShortestFlightEntities db)
        {
            this.db = db;
        }
        public List<string> getListCountries()
        {
            return db.airports.Select(a => a.country).Distinct().ToList();
        }
        public List<string> getListAirports(string city)
        {
            return db.airports.Where(a => a.city == city).Select(a => a.airport1).Distinct().ToList();
        }
        public List<string> getListCities(string country)
        {
            return db.airports.Where(a => a.country == country).Select(a => a.city).Distinct().ToList();
        }
        public int getAirportIdByAirport(string airport )
        {
            return db.airports.Where(a => a.airport1 == airport).FirstOrDefault().airport_ID;
        }

    }
}