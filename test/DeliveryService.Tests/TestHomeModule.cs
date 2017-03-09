using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Core;
using DeliveryService.Data.Repositories;
using DeliveryService.Modules;
using Moq;
using Nancy;
using Nancy.Testing;
using Xunit;

namespace DeliveryService.Tests
{
    public class TestHomeModule
    {

        [Fact]
        public async void TestPingName()
        {
            // Arrange
            var clock = new DateTime(1999, 01, 01);
            var date = new Mock<IDateTime>();
            date.Setup(repo => repo.Now).Returns(clock);
            var delivery = new Mock<IDeliveryRepository>();
            var user = new Mock<IUserRepository>();

            // Init module
            var module = new HomeModule(date.Object, delivery.Object, user.Object);
            var browser = new Browser(b => b.Module(module));

            // Act
            var testName = "test_name";
            var result = await browser.Get($"/ping_clock/{testName}");

            // Assert
            var response = result.Body.DeserializeJson<dynamic>();
            Assert.Equal(result.StatusCode, HttpStatusCode.OK);
            Assert.Equal(DateTime.Parse(response.clock), clock);
            Assert.Equal(response.person.Name, testName);
        }
    }
}
