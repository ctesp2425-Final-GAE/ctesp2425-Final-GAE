using Microsoft.AspNetCore.Mvc;
using RestaurantReservationAPI.Models;

namespace RestaurantReservationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/reservation
        [HttpGet]
        public IActionResult GetReservations()
        {
            return Ok(_context.Reservations.ToArray());
        }

        // PUT: api/reservation/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateReservation(int id, [FromBody] Reservation updatedReservation)
        {
            Reservation reservation = _context.Reservations.FirstOrDefault(res => res.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            reservation.CustomerName = updatedReservation.CustomerName;
            reservation.ReservationDate = updatedReservation.ReservationDate;
            reservation.ReservationTime = updatedReservation.ReservationTime;
            reservation.TableNumber = updatedReservation.TableNumber;
            reservation.NumberOfPeople = updatedReservation.NumberOfPeople;

            _context.SaveChanges();
            return Ok(reservation);
        }

        // GET: api/reservations?date={date}
        [HttpGet]
        public IActionResult GetReservationsByDate([FromQuery] DateTime date)
        {
            var reservations = _context.Reservations
                .Where(reservation => reservation.ReservationDate.Date == date.Date)
                .ToList();

            if (!reservations.Any())
            {
                return NotFound("No reservations found for the specified date.");
            }

            return Ok(reservations);
        }

    }
}
