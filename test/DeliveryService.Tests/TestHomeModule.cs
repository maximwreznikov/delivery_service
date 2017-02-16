using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using DeliveryService.Core;
using DeliveryService.Data.Repositories;
using DeliveryService.Models;
using DeliveryService.Modules;
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
            var result =  await browser.Get($"/ping_clock/{testName}");

            // Assert
            var response = result.Body.DeserializeJson<dynamic>(); 
            Assert.Equal(result.StatusCode, HttpStatusCode.OK);
            Assert.Equal(DateTime.Parse(response.clock), clock);
            Assert.Equal(response.person.Name, testName);
        }

        [Fact]
        public async void TestCreateDelivery()
        {
            // Arrange
            var objects = new List<DeliveryObject> { new DeliveryObject { Title = "1" }, new DeliveryObject { Title = "2" } };
            var delivery = new Mock<IDeliveryRepository>();
            delivery.Setup(r => r.AllDeliveries()).Returns(objects);

            var dateTime = new DateTime(1999, 01, 01);
            var date = new Mock<IDateTime>();
            date.Setup(repo => repo.Now).Returns(dateTime);

            var user = new Mock<IUserRepository>();

            // Init module
            var module = new HomeModule(date.Object, delivery.Object, user.Object);
            var browser = new Browser(b => b.Module(module));

            // Act
            var deliveryTitle = "new_delivery";
            var result = await browser.Post($"/CreateDelivery/{deliveryTitle}");

            // Assert
            Assert.Equal(result.StatusCode, HttpStatusCode.Created);
            delivery.Verify(dr => dr.Add(It.Is((DeliveryObject o) => o.Title == deliveryTitle)), Times.Once());
        }

        [Fact]
        public async void TestGetAvailableDeliveries()
        {
            // Arrange
            var objects = new List<DeliveryObject> {new DeliveryObject {Title = "1" , Status=DeliveryStatus.Available},
                new DeliveryObject { Title = "2",Status = DeliveryStatus.Available },
                new DeliveryObject { Title = "3",Status = DeliveryStatus.Taken }
            };
            var delivery = new Mock<IDeliveryRepository>();
            delivery.Setup(r => r.AllDeliveries()).Returns(objects);

            var date = new Mock<IDateTime>();
            
            var userMock = new Mock<IUserRepository>();

            // Init module
            var module = new HomeModule(date.Object, delivery.Object, userMock.Object);
            var browser = new Browser(b => b.Module(module));

            // Act
            var result = await browser.Get($"/GetAvailableDeliveries");

            // Assert
            Assert.Equal(result.StatusCode, HttpStatusCode.OK);
            var response = result.Body.DeserializeJson<List<DeliveryObject>>();

            Assert.Equal(response.Count, 2);
            Assert.Equal(response[0].Title, "1");
            Assert.Equal(response[1].Title, "2");
        }

        [Fact]
        public async void TestTakeDelivery()
        {
            // Arrange
            var deliveryObject = new DeliveryObject { Id = 101, Title = "Box of Shugar", Status = DeliveryStatus.Available, ModificationTime = DateTime.MinValue};
            var deliveryObject2 = new DeliveryObject { Id = 101, Title = "Box of Tea", Status = DeliveryStatus.Taken, ModificationTime = DateTime.MinValue};
            var deliveryMock = new Mock<IDeliveryRepository>();
            deliveryMock.Setup(r => r.GetDelivery(It.Is<int>( i => i == 101))).Returns(deliveryObject);
            deliveryMock.Setup(r => r.GetDelivery(It.Is<int>( i => i == 102))).Returns(deliveryObject2);
            deliveryMock.Setup(r => r.UpdateDelivery(It.IsAny<DeliveryObject>()));

            var dateTime = new DateTime(1999, 01, 01);
            var date = new Mock<IDateTime>();
            date.Setup(repo => repo.Now).Returns(dateTime);

            var user = new Person { Id = 42, Name = "Max Plank" };
            var userMock = new Mock<IUserRepository>();
            userMock.Setup(u => u.GetPerson(It.Is<int>(i => i== 42))).Returns(user);

            // Init module
            var module = new HomeModule(date.Object, deliveryMock.Object, userMock.Object);
            var browser = new Browser(b => b.Module(module));

            // Act
            var resultNotUser = await browser.Post($"/TakeDelivery/41.101");
            var resultNotDelivery = await browser.Post($"/TakeDelivery/42.100");
            var resultWrongStatus = await browser.Post($"/TakeDelivery/42.102");
            var resultOk = await browser.Post($"/TakeDelivery/42.101");

            // Assert
            Assert.Equal(resultNotUser.StatusCode, HttpStatusCode.Unauthorized);
            Assert.Equal(resultNotDelivery.StatusCode, HttpStatusCode.NotFound);
            Assert.Equal(resultWrongStatus.StatusCode, HttpStatusCode.UnprocessableEntity);

            Assert.Equal(resultOk.StatusCode, HttpStatusCode.Accepted);
            deliveryMock.Verify(dr => dr.UpdateDelivery(It.Is(
                (DeliveryObject o) => 
                o.Id == deliveryObject.Id && 
                o.ModificationTime == dateTime && 
                o.Status == DeliveryStatus.Taken && 
                o.PersonId == user.Id)), Times.Once());
        }
    }
}
