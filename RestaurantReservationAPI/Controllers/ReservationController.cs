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

        // GET: api/reservations/{id}
        [HttpGet("{id}")]
        public IActionResult GetReservationById(int id)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound(new { Message = "Reservation not found" });
            }
            
            return Ok(reservation);
        }

        // DELETE: api/reservations/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(int id)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound(new { Message = "Reservation not found" });
            }

            _context.Reservations.Remove(reservation);
            _context.SaveChanges();

            return Ok(new { Message = "Reservation deleted successfully" });
        }
    }
}
