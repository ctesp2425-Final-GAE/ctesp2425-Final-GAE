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
        public IActionResult GetReservations(DateTime? dateTime, string? customerName)
        {
            var reservations = _context.Reservations.AsQueryable();

            if (dateTime.HasValue)
            {
                reservations = reservations.Where(r =>
                    r.ReservationDate == dateTime.Value.Date &&
                    r.ReservationTime == dateTime.Value.TimeOfDay);
            }

            if (!string.IsNullOrEmpty(customerName))
            {
                reservations = reservations.Where(r => r.CustomerName.Contains(customerName));
            }

            return Ok(reservations.ToArray());
        }

        // PUT: api/reservation/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateReservation(int id, [FromBody] Reservation updatedReservation)
        {

            if (updatedReservation.TableNumber < 1 || updatedReservation.TableNumber > TableController.Tables.Count)
            {
                return BadRequest("O número da mesa não pode ser maior que o número de mesas existentes.");
            }

            Reservation reservation = _context.Reservations.FirstOrDefault(res => res.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            var conflictingReservation = _context.Reservations
            .Where(res => res.TableNumber == updatedReservation.TableNumber
                          && res.ReservationDate == updatedReservation.ReservationDate
                          && res.Id != id)
            .AsEnumerable()
            .FirstOrDefault(res => Math.Abs((res.ReservationTime - updatedReservation.ReservationTime).TotalMinutes) < 60);


            if (conflictingReservation != null)
            {
                return BadRequest("A mesa já está reservada em um intervalo de 1 hora para a data solicitada.");
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
