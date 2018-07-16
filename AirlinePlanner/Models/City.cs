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
    }
}
