using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirlinePlanner;

namespace AirlinePlanner.Models
{
    public class City
    {
        public int Id { get; set; }
        public string AirportCode { get; set; }

        public City(string code, int id = 0)
        {
            AirportCode = code;
        }

        public override bool Equals(System.Object otherCity)
        {
            if (!(otherCity is City))
            {
                return false;
            }
            else
            {
                City city = (City) otherCity;
                bool idEquality = (this.Id == city.Id);
                bool airportCodeEquality = (this.AirportCode == city.AirportCode);
                return (idEquality && airportCodeEquality);
            }
        }
    }
}
