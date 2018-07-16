using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using AirlinePlanner.Models;

namespace AirlinePlanner.Tests
{
    [TestClass]
    public class FlightTest : IDisposable
    {
        public void Dispose()
        {
            Flight.DeleteAll();
        }

        public FlightTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_planner_test;";
        }

        [TestMethod]
        public void SetGetProperties_SetsGetsProperties_True()
        {
            DateTime dateAndTime = new DateTime(2018, 07, 16, 11, 25, 0);
            DateTime dateAndTime2 = new DateTime(2018, 07, 16, 12, 25, 0);
            Flight flight = new Flight("11X", "Arrive", dateAndTime, Flight.StatusCodes["onTime"]);
            flight.AirlinerCode = "12X";
            flight.ArriveDepart = "Depart";
            flight.Time = dateAndTime2;
            flight.Status = Flight.StatusCodes["delayed"];
            flight.Id = 2;
            Assert.AreEqual("12X", flight.AirlinerCode);
            Assert.AreEqual("Depart", flight.ArriveDepart);
            Assert.AreEqual(dateAndTime2, flight.Time);
            Assert.AreEqual("Delayed", flight.Status);
            Assert.AreEqual(2, flight.Id);
        }

        [TestMethod]
        public void Equals_ReturnsTrueIfPropertiesAreSame_True()
        {
            DateTime dateAndTime = new DateTime(2018, 07, 16, 11, 25, 0);
            Flight flight = new Flight("11X", "Arrive", dateAndTime, Flight.StatusCodes["onTime"]);
            Flight flight2 = new Flight("11X", "Arrive", dateAndTime, Flight.StatusCodes["onTime"]);
            Assert.AreEqual(flight2, flight);
        }

        [TestMethod]
        public void GetAll_DbStartsEmpty_True()
        {
            int result = Flight.GetAll().Count;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Save_SavesFlightToDatabase_FlightList()
        {
            DateTime dateAndTime = new DateTime(2018, 07, 16, 11, 25, 0);
            Flight flight = new Flight("11X", "Arrive", dateAndTime, Flight.StatusCodes["onTime"]);
            flight.Save();
            List<Flight> expectedList = new List<Flight> { flight };
            List<Flight> actualList = Flight.GetAll();
            CollectionAssert.AreEqual(expectedList, actualList);
        }

        [TestMethod]
        public void DeleteAll_DeletesAllCitiesFromDatabase_FlightList()
        {
            DateTime dateAndTime = new DateTime(2018, 07, 16, 11, 25, 0);
            Flight flight = new Flight("11X", "Arrive", dateAndTime, Flight.StatusCodes["onTime"]);
            flight.Save();
            Flight.DeleteAll();
            List<Flight> expectedList = new List<Flight> { };
            List<Flight> actualList = Flight.GetAll();
            CollectionAssert.AreEqual(expectedList, actualList);
        }

        [TestMethod]
        public void Find_FindsFlightInDb_Flight()
        {
            DateTime dateAndTime = new DateTime(2018, 07, 16, 11, 25, 0);
            Flight flight = new Flight("11X", "Arrive", dateAndTime, Flight.StatusCodes["onTime"]);
            flight.Save();
            Flight foundFlight = Flight.Find(flight.Id);
            Assert.AreEqual(flight, foundFlight);
        }

        [TestMethod]
        public void Update_UpdatesColumnInDatabase_FlightList()
        {
            DateTime dateAndTime = new DateTime(2018, 07, 16, 11, 25, 0);
            Flight flight = new Flight("11X", "Arrive", dateAndTime, Flight.StatusCodes["onTime"]);
            flight.Save();
            flight.AirlinerCode = "12X";
            flight.Update();
            List<Flight> allFlights = Flight.GetAll();
            List<Flight> expectedList = new List<Flight>{ flight };
            CollectionAssert.AreEqual(expectedList, allFlights);
        }

        [TestMethod]
        public void Delete_DeletesFlightFromDb_FlightList()
        {
            DateTime dateAndTime = new DateTime(2018, 07, 16, 11, 25, 0);
            Flight flight = new Flight("11X", "Arrive", dateAndTime, Flight.StatusCodes["onTime"]);
            flight.Save();
            Flight flight2 = new Flight("11X", "Arrive", dateAndTime, Flight.StatusCodes["onTime"]);
            flight2.Save();
            flight.Delete();
            List<Flight> expectedList = new List<Flight> { flight2 };
            List<Flight> actualList = Flight.GetAll();
            CollectionAssert.AreEqual(expectedList, actualList);
        }

        [TestMethod]
        public void AddCity_AddsCityToJoinTable_CityList()
        {
            DateTime dateAndTime = new DateTime(2018, 07, 16, 12, 25, 0);
            Flight flight = new Flight("11X", "Arrive", dateAndTime, Flight.StatusCodes["onTime"]);
            City city = new City("PDX");
            flight.Save();
            city.Save();
            flight.AddCity(city);
            List<City> actualList = flight.GetCities();
            List<City> expectedList = new List<City> { city };
            CollectionAssert.AreEqual(expectedList, actualList);
        }
    }
}
