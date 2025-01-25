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
        public IActionResult GetReservations(DateTime? date, string? customerName)
        {
            var reservations = _context.Reservations.AsQueryable();

            if (date.HasValue)
            {
                reservations = reservations.Where(r => r.ReservationDate == date.Value.Date);
            }

            if (!string.IsNullOrEmpty(customerName))
            {
                reservations = reservations.Where(r => r.CustomerName.Contains(customerName));
            }

            return Ok(reservations.ToArray());

        }

        // GET: api/reservation/{id}
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

        // POST: api/reservation
        [HttpPost]
        public IActionResult CreateReservation([FromBody] Reservation newReservation)
        {
            // Validar os dados recebidos
            if (newReservation == null || string.IsNullOrWhiteSpace(newReservation.CustomerName) 
                || newReservation.ReservationDate == default || newReservation.ReservationTime == default
                || newReservation.TableNumber <= 0 || newReservation.NumberOfPeople <= 0)
            {
                return BadRequest(new { Message = "Invalid reservation data" });
            }

            // Adicionar a nova reserva à base de dados
            _context.Reservations.Add(newReservation);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetReservationById), new { id = newReservation.Id }, newReservation);
        }

        // DELETE: api/reservation/{id}
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
    }
}
