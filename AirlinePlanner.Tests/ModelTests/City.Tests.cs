using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using AirlinePlanner.Models;

namespace AirlinePlanner.Tests
{
    [TestClass]
    public class CityTest : IDisposable
    {
        public void Dispose()
        {
            City.DeleteAll();
        }

        public CityTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_planner_test;";
        }

        [TestMethod]
        public void SetGetProperties_SetsGetsProperties_True()
        {
            City city = new City("PDX");
            city.AirportCode = "SEA";
            city.Id = 2;
            Assert.AreEqual("SEA", city.AirportCode);
            Assert.AreEqual(2, city.Id);
        }

        [TestMethod]
        public void Equals_ReturnsTrueIfPropertiesAreSame_True()
        {
            City city = new City("PDX");
            City cityTwo = new City("PDX");
            Assert.AreEqual(cityTwo, city);
        }

        [TestMethod]
        public void GetAll_DbStartsEmpty_True()
        {
            int result = City.GetAll().Count;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Save_SavesCityToDatabase_CityList()
        {
            City newCity = new City("PDX");
            newCity.Save();
            List<City> expectedList = new List<City> { newCity };
            List<City> actualList = City.GetAll();
            CollectionAssert.AreEqual(expectedList, actualList);
        }

        [TestMethod]
        public void DeleteAll_DeletesAllCitiesFromDatabase_CityList()
        {
            City newCity1 = new City("PDX");
            newCity1.Save();
            City newCity2 = new City("LAX");
            newCity2.Save();
            City.DeleteAll();
            List<City> expectedList = new List<City> { };
            List<City> actualList = City.GetAll();
            CollectionAssert.AreEqual(expectedList, actualList);
        }

        [TestMethod]
        public void Find_FindsCityInDb_City()
        {
            City city = new City("PDX");
            city.Save();
            City foundCity = City.Find(city.Id);
            Assert.AreEqual(city, foundCity);
        }

        [TestMethod]
        public void Update_UpdatesColumnInDatabase_CityList()
        {
            City city = new City("PDX", 1);
            city.Save();
            city.AirportCode = "SEA";
            city.Update();
            List<City> allCities = City.GetAll();
            List<City> expectedList = new List<City>{ city };
            CollectionAssert.AreEqual(expectedList, allCities);
        }

        [TestMethod]
        public void Delete_DeletesCityFromDb_CityList()
        {
            City city = new City("PDX");
            City city2 = new City("SEA");
            city.Save();
            city2.Save();
            city.Delete();
            List<City> expectedList = new List<City> { city2 };
            List<City> actualList = City.GetAll();
            CollectionAssert.AreEqual(expectedList, actualList);
        }

        [TestMethod]
        public void AddFlight_AddsFlightToJoinTable_FlightList()
        {
            City city = new City("PDX");
            DateTime dateAndTime = new DateTime(2018, 07, 16, 12, 25, 0);
            Flight flight = new Flight("11X", "Arrive", dateAndTime, Flight.StatusCodes["onTime"]);
            city.Save();
            flight.Save();
            city.AddFlight(flight);
            List<Flight> actualList = city.GetFlights();
            List<Flight> expectedList = new List<Flight> { flight };
            CollectionAssert.AreEqual(expectedList, actualList);
        }

        [TestMethod]
        public void GetFlights_ReturnsAllCityFlights_FlightList()
        {
            City city = new City("PDX");
            DateTime dateAndTime = new DateTime(2018, 07, 16, 12, 25, 0);
            Flight flight = new Flight("11X", "Arrive", dateAndTime, Flight.StatusCodes["onTime"]);
            Flight flight2 = new Flight("12X", "Arrive", dateAndTime, Flight.StatusCodes["onTime"]);
            city.Save();
            flight.Save();
            flight2.Save();
            city.AddFlight(flight);
            List<Flight> actualList = city.GetFlights();
            List<Flight> expectedList = new List<Flight> { flight };
            CollectionAssert.AreEqual(expectedList, actualList);
        }
    }
}
