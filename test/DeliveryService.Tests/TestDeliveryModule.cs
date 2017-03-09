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
    public class TestDeliveryModule
    {
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
            var module = new DeliveryModule(date.Object, delivery.Object, user.Object);
            var browser = new Browser(b => b.Module(module));

            // Act
            var deliveryTitle = "new_delivery";
            var result = await browser.Post($"/delivery/create/{deliveryTitle}");

            // Assert
            Assert.Equal(result.StatusCode, HttpStatusCode.Created);
            delivery.Verify(dr => dr.Create(It.Is((string title) => title == deliveryTitle), It.Is((DateTime d) => d == dateTime), It.Is((TimeSpan ts) => ts == TimeSpan.FromDays(1))), Times.Once());
        }

        [Fact]
        public async void TestGetAvailableDeliveries()
        {
            // Arrange
            var objects = new List<DeliveryObject> {new DeliveryObject {Title = "1", Status=DeliveryStatus.Available},
                new DeliveryObject { Title = "2", Status = DeliveryStatus.Available },
                new DeliveryObject { Title = "3", Status = DeliveryStatus.Taken }
            };
            var delivery = new Mock<IDeliveryRepository>();
            delivery.Setup(r => r.AllDeliveries()).Returns(objects);

            var date = new Mock<IDateTime>();
            
            var userMock = new Mock<IUserRepository>();

            // Init module
            var module = new DeliveryModule(date.Object, delivery.Object, userMock.Object);
            var browser = new Browser(b => b.Module(module));

            // Act
            var result = await browser.Get($"/delivery/get_available");

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
            var dateTime = new DateTime(1999, 01, 01);

            // Arrange
            var deliveryObject = new DeliveryObject { Id = 101, Title = "Box of Shugar", Status = DeliveryStatus.Available, Lifetime = TimeSpan.MaxValue};
            var deliveryObject2 = new DeliveryObject { Id = 102, Title = "Box of Tea", Status = DeliveryStatus.Taken, Lifetime = TimeSpan.MaxValue };
            var deliveryObject3 = new DeliveryObject { Id = 103, Title = "Box of Coffee", Status = DeliveryStatus.Available, Lifetime = TimeSpan.Zero, CreationTime = dateTime + TimeSpan.FromSeconds(1)};
            var deliveryMock = new Mock<IDeliveryRepository>();
            deliveryMock.Setup(r => r.GetDelivery(It.Is<int>( i => i == 101))).Returns(deliveryObject);
            deliveryMock.Setup(r => r.GetDelivery(It.Is<int>( i => i == 102))).Returns(deliveryObject2);
            deliveryMock.Setup(r => r.GetDelivery(It.Is<int>( i => i == 103))).Returns(deliveryObject3);
            deliveryMock.Setup(r => r.UpdateDelivery(It.IsAny<DeliveryObject>()));

            var date = new Mock<IDateTime>();
            date.Setup(repo => repo.Now).Returns(dateTime);

            var user = new Person { Id = 42, Name = "Max Plank" };
            var userMock = new Mock<IUserRepository>();
            userMock.Setup(u => u.GetPerson(It.Is<int>(i => i== 42))).Returns(user);

            // Init module
            var module = new DeliveryModule(date.Object, deliveryMock.Object, userMock.Object);
            var browser = new Browser(b => b.Module(module));

            // Act
            var resultNotUser = await browser.Post($"/delivery/take/41.101");
            var resultNotDelivery = await browser.Post($"/delivery/take/42.100");
            var resultWrongStatus = await browser.Post($"/delivery/take/42.102");
            var resultBadRequest = await browser.Post($"/delivery/take/42.103");
            var resultOk = await browser.Post($"/delivery/take/42.101");

            // Assert
            Assert.Equal(resultNotUser.StatusCode, HttpStatusCode.Unauthorized);
            Assert.Equal(resultNotDelivery.StatusCode, HttpStatusCode.NotFound);
            Assert.Equal(resultWrongStatus.StatusCode, HttpStatusCode.UnprocessableEntity);
            Assert.Equal(resultBadRequest.StatusCode, HttpStatusCode.BadRequest);

            Assert.Equal(resultOk.StatusCode, HttpStatusCode.Accepted);
            deliveryMock.Verify(dr => dr.UpdateDelivery(It.Is(
                (DeliveryObject o) => 
                o.Id == deliveryObject.Id && 
                o.Lifetime == TimeSpan.Zero && 
                o.Status == DeliveryStatus.Taken && 
                o.PersonId == user.Id)), Times.Once());
        }
    }
}
