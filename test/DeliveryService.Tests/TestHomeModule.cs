using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using DeliveryService.Core;
using DeliveryService.Modules;
using DeliveryService.Repositories;
using Nancy;
using Nancy.Extensions;
using Nancy.Testing;

namespace DeliveryService.Tests
{
    public class TestHomeModule
    {
        public TestHomeModule()
        {
        }

        public void Dispose()
        {

        }

        [Fact]
        public async void TestPingName()
        {
            // Arrange
            var date = new Mock<IDateTime>();
            var clock = new DateTime(1999, 01, 01);
            date.Setup(repo => repo.Now).Returns(clock);
            var delivery = new Mock<IDeliveryRepository>();
            var user = new Mock<IUserRepository>();

            // Init module
            var module = new HomeModule(date.Object, delivery.Object, user.Object);
            var browser = new Browser(b => b.Module(module));

            // Act
            var testName = "test_name";
            var result =  await browser.Get($"/ping_clock/{testName}");

            // Assert
            var response = result.Body.DeserializeJson<dynamic>(); 
            Assert.Equal(result.StatusCode, HttpStatusCode.OK);
            var t = DateTime.Parse(response.clock);
            Assert.Equal(t, clock);
            var person = response.person;
            Assert.Equal(person.Name, testName);
        }

        [Fact]
        public void TestCreateDelivery()
        {
            
        }

        [Fact]
        public void TestGetAvailableDeliveries()
        {
        }

        [Fact]
        public void TestTakeDelivery()
        {
            
        }
    }
}
