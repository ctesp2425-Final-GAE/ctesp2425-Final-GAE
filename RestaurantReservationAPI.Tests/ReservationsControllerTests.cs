using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservationAPI.Controllers;
using RestaurantReservationAPI.Models;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Castle.Components.DictionaryAdapter.Xml;

namespace RestaurantReservationAPI.Tests
{
    public class ReservationsControllerTests
    {
        private DbContextOptions<ApplicationDbContext> GetInMemoryDbContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void GetReservations_ReturnsAllReservations()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Reservations.Add(new Reservation
                {
                    Id = 1,
                    CustomerName = "John Doe",
                    ReservationDate = DateTime.Today,
                    ReservationTime = new TimeSpan(12, 0, 0),
                    TableNumber = 1,
                    NumberOfPeople = 4
                });

                context.Reservations.Add(new Reservation
                {
                    Id = 2,
                    CustomerName = "Jane Smith",
                    ReservationDate = DateTime.Today.AddDays(1),
                    ReservationTime = new TimeSpan(18, 0, 0),
                    TableNumber = 2,
                    NumberOfPeople = 2
                });

                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ReservationController(context);

                // Act
                var result = controller.GetReservations() as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var reservations = Assert.IsType<Reservation[]>(result.Value);

                Assert.Equal(2, reservations.Length);
                Assert.Equal("John Doe", reservations[0].CustomerName);
                Assert.Equal("Jane Smith", reservations[1].CustomerName);
            }
        }
    }
}
