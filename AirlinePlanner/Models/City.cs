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

        public static List<City> GetAll()
        {
            List<City> allCities = new List<City>() {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM cities;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int cityId = rdr.GetInt32(0);
                string cityName = rdr.GetString(1);
                City city = new City(cityName, cityId);
                allCities.Add(city);
            }
            conn.Close();

            if (conn != null)
            {
                conn.Dispose();
            }
            return allCities;
        }
    }
}
