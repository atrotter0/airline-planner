using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirlinePlanner;

namespace AirlinePlanner.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string AirlinerCode { get; set; }
        public string ArriveDepart { get; set; }
        public DateTime Time { get; set; }
        public string Status { get; set; }
        public static Dictionary<string, string> StatusCodes = new Dictionary<string, string> { { "onTime", "On Time" }, { "delayed", "Delayed" }, { "cancelled", "Cancelled" } };

        public Flight(string code, string arriveDepart, DateTime time, string status, int id = 0)
        {
            AirlinerCode = code;
            ArriveDepart = arriveDepart;
            Time = time;
            Status = status;
            Id = id;
        }

        public override bool Equals(System.Object otherFlight)
        {
            if (!(otherFlight is Flight))
            {
                return false;
            }
            else
            {
                Flight flight = (Flight) otherFlight;
                bool idEquality = (this.Id == flight.Id);
                bool airlinerCodeEquality = (this.AirlinerCode == flight.AirlinerCode);
                bool arriveDepartEquality = (this.ArriveDepart == flight.ArriveDepart);
                bool timeEquality = (this.Time == flight.Time);
                bool statusEquality = (this.Status == flight.Status);
                return (idEquality && airlinerCodeEquality && arriveDepartEquality && timeEquality && statusEquality);
            }
        }

        public static List<Flight> GetAll()
        {
            List<Flight> allFlights = new List<Flight>() {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int flightId = rdr.GetInt32(0);
                string flightAirlineCode = rdr.GetString(1);
                string flightArriveDepart = rdr.GetString(2);
                DateTime flightDateTime = rdr.GetDateTime(3);
                string flightStatus = rdr.GetString(4);
                Flight flight = new Flight(flightAirlineCode, flightArriveDepart, flightDateTime, flightStatus, flightId);
                allFlights.Add(flight);
            }
            conn.Close();

            if (conn != null)
            {
                conn.Dispose();
            }
            return allFlights;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM flights;";
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO flights (airliner_code, arrive_depart, time, status) VALUES (@AirlinerCode, @ArriveDepart, @Time, @Status);";
            cmd.Parameters.AddWithValue("@AirlinerCode", this.AirlinerCode);
            cmd.Parameters.AddWithValue("@ArriveDepart", this.ArriveDepart);
            cmd.Parameters.AddWithValue("@Time", this.Time);
            cmd.Parameters.AddWithValue("@Status", this.Status);
            cmd.ExecuteNonQuery();
            this.Id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Flight Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights WHERE id = @SearchId;";
            cmd.Parameters.AddWithValue("@SearchId", id);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int flightId = 0;
            string flightAirlineCode = "";
            string flightArriveDepart = "";
            DateTime flightDateTime = new DateTime();
            string flightStatus = "";
            while (rdr.Read())
            {
                flightId = rdr.GetInt32(0);
                flightAirlineCode = rdr.GetString(1);
                flightArriveDepart = rdr.GetString(2);
                flightDateTime = rdr.GetDateTime(3);
                flightStatus = rdr.GetString(4);
            }
            Flight foundFlight = new Flight(flightAirlineCode, flightArriveDepart, flightDateTime, flightStatus, flightId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundFlight;
        }

        public void Update()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE flights SET airliner_code = @flightAirlinerCode, arrive_depart = @flightArriveDepart, time = @flightDateTime, status = @flightStatus WHERE id = @FlightId;";
            cmd.Parameters.AddWithValue("@flightAirlinerCode", this.AirlinerCode);
            cmd.Parameters.AddWithValue("@flightArriveDepart", this.ArriveDepart);
            cmd.Parameters.AddWithValue("@flightDateTime", this.Time);
            cmd.Parameters.AddWithValue("@flightStatus", this.Status);
            cmd.Parameters.AddWithValue("@FlightId", this.Id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM flights WHERE id = @FlightId;";
            cmd.Parameters.AddWithValue("@FlightId", this.Id);
            cmd.ExecuteNonQuery();
            // delete flights with same flight_id reference
            conn.Close();

            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
