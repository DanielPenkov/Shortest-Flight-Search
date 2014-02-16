using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShortestFlight.DBLayer;

namespace ShortestFlight.Controller
{

    public class AirportsController
    {

        UnitOfWork unitOfWork = new UnitOfWork();


        public List<string> getListCountries()
        {
            return unitOfWork.getAirportsRepository().getListCountries();
        
        }
        public List<string> getListAirports( string city)
        {

            return unitOfWork.getAirportsRepository().getListAirports(city);
        }

        public List<string> getListCities( string country)
        {

            return unitOfWork.getAirportsRepository().getListCities(country);
      
        }
         

    }
}