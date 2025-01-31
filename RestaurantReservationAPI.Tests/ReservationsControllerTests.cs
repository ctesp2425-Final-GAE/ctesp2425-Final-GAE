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

                var result = controller.GetReservations() as OkObjectResult;

                Assert.NotNull(result);
                var reservations = Assert.IsType<Reservation[]>(result.Value);

                Assert.Equal(2, reservations.Length);
                Assert.Equal("John Doe", reservations[0].CustomerName);
                Assert.Equal("Jane Smith", reservations[1].CustomerName);
            }
        }

        [Fact]
        public void GetReservationById_ReturnsCorrectReservation()
        {
            var options = GetInMemoryDbContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                context.Reservations.Add(new Reservation { Id = 1, CustomerName = "John Doe", ReservationDate = DateTime.Today, ReservationTime = new TimeSpan(12, 0, 0), TableNumber = 1, NumberOfPeople = 4 });
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ReservationController(context);
                var result = controller.GetReservationById(1) as OkObjectResult;
                Assert.NotNull(result);
                var reservation = Assert.IsType<Reservation>(result.Value);
                Assert.Equal("John Doe", reservation.CustomerName);
            }
        }

        [Fact]
        public void CreateReservation_AddsReservation()
        {
            var options = GetInMemoryDbContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ReservationController(context);
                var newReservation = new Reservation { CustomerName = "Alice Brown", ReservationDate = DateTime.Today, ReservationTime = new TimeSpan(19, 0, 0), TableNumber = 3, NumberOfPeople = 5 };
                var result = controller.CreateReservation(newReservation) as CreatedAtActionResult;
                Assert.NotNull(result);
                Assert.Equal(1, context.Reservations.Count());
            }
        }

        [Fact]
        public void DeleteReservation_RemovesReservation()
        {
            var options = GetInMemoryDbContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                context.Reservations.Add(new Reservation { Id = 1, CustomerName = "John Doe", ReservationDate = DateTime.Today, ReservationTime = new TimeSpan(12, 0, 0), TableNumber = 1, NumberOfPeople = 4 });
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ReservationController(context);
                var result = controller.DeleteReservation(1) as OkObjectResult;
                Assert.NotNull(result);
                Assert.Empty(context.Reservations);
            }
        }

        [Fact]
        public void UpdateReservation_UpdatesReservationDetails()
        {
            var options = GetInMemoryDbContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                context.Reservations.Add(new Reservation { Id = 1, CustomerName = "John Doe", ReservationDate = DateTime.Today, ReservationTime = new TimeSpan(12, 0, 0), TableNumber = 1, NumberOfPeople = 4 });
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ReservationController(context);
                var updatedReservation = new Reservation { CustomerName = "John Doe Updated", ReservationDate = DateTime.Today, ReservationTime = new TimeSpan(13, 0, 0), TableNumber = 2, NumberOfPeople = 6 };
                var result = controller.UpdateReservation(1, updatedReservation) as OkObjectResult;
                Assert.NotNull(result);
                var reservation = context.Reservations.First();
                Assert.Equal("John Doe Updated", reservation.CustomerName);
                Assert.Equal(6, reservation.NumberOfPeople);
            }
        }


        [Fact]
        public void GetReservationById_ReturnsNotFound_WhenReservationDoesNotExist()
        {
            var options = GetInMemoryDbContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ReservationController(context);
                var result = controller.GetReservationById(999) as NotFoundObjectResult;
                Assert.NotNull(result);
            }
        }

        [Fact]
        public void CreateReservation_ReturnsBadRequest_WhenInvalidData()
        {
            var options = GetInMemoryDbContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ReservationController(context);
                var newReservation = new Reservation { CustomerName = "", ReservationDate = default, ReservationTime = default, TableNumber = 0, NumberOfPeople = 0 };
                var result = controller.CreateReservation(newReservation) as BadRequestObjectResult;
                Assert.NotNull(result);
            }
        }

        [Fact]
        public void DeleteReservation_ReturnsNotFound_WhenReservationDoesNotExist()
        {
            var options = GetInMemoryDbContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ReservationController(context);
                var result = controller.DeleteReservation(999) as NotFoundObjectResult;
                Assert.NotNull(result);
            }
        }

        [Fact]
        public void UpdateReservation_ReturnsNotFound_WhenReservationDoesNotExist()
        {
            var options = GetInMemoryDbContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ReservationController(context);
                var updatedReservation = new Reservation { CustomerName = "New Name", ReservationDate = DateTime.Today, ReservationTime = new TimeSpan(13, 0, 0), TableNumber = 2, NumberOfPeople = 4 };
                var result = controller.UpdateReservation(999, updatedReservation) as NotFoundResult;
                Assert.NotNull(result);
            }
        }

        [Fact]
        public void GetReservations_FiltersByDateAndCustomerName()
        {
            var options = GetInMemoryDbContextOptions();
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

                context.Reservations.Add(new Reservation
                {
                    Id = 3,
                    CustomerName = "John Doe",
                    ReservationDate = DateTime.Today.AddDays(2),
                    ReservationTime = new TimeSpan(19, 0, 0),
                    TableNumber = 3,
                    NumberOfPeople = 6
                });

                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ReservationController(context);

                var result = controller.GetReservations(DateTime.Today.AddHours(12), "John Doe") as OkObjectResult;

                Assert.NotNull(result);
                var reservations = Assert.IsType<Reservation[]>(result.Value);

                Assert.Single(reservations);
                Assert.Equal("John Doe", reservations[0].CustomerName);
                Assert.Equal(DateTime.Today, reservations[0].ReservationDate);
                Assert.Equal(new TimeSpan(12, 0, 0), reservations[0].ReservationTime); 
            }
        }


        [Fact]
        public void GetReservations_FiltersByCustomerNameOnly()
        {
            var options = GetInMemoryDbContextOptions();
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

                var result = controller.GetReservations(null, "Jane Smith") as OkObjectResult;

                Assert.NotNull(result);
                var reservations = Assert.IsType<Reservation[]>(result.Value);

                Assert.Single(reservations);
                Assert.Equal("Jane Smith", reservations[0].CustomerName);
                Assert.Equal(DateTime.Today.AddDays(1), reservations[0].ReservationDate);
            }
        }

        [Fact]
        public void GetReservations_ReturnsAllReservations_WhenNoFiltersApplied()
        {
            var options = GetInMemoryDbContextOptions();
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

                var result = controller.GetReservations(null, null) as OkObjectResult;

                Assert.NotNull(result);
                var reservations = Assert.IsType<Reservation[]>(result.Value);

                Assert.Equal(2, reservations.Length);
            }
        }
    }
}