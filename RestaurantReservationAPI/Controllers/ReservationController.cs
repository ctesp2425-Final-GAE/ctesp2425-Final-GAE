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
        /// <summary>
        /// Obtém todas as reservas filtradas por data e nome do cliente.
        /// </summary>
        /// <param name="dateTime">Data da reserva</param>
        /// <param name="customerName">Nome do cliente</param>
        /// <returns>Lista de reservas</returns>
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

        // GET: api/reservation/{id}
        /// <summary>
        /// Obtém uma reserva específica pelo ID.
        /// </summary>
        /// <param name="id">ID da reserva</param>
        /// <returns>Dados da reserva</returns>
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
        /// <summary>
        /// Cria uma nova reserva.
        /// </summary>
        /// <param name="newReservation">Objeto da reserva</param>
        /// <returns>Dados da reserva criada</returns>
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

        /// <summary>
        /// Remove uma reserva pelo ID.
        /// </summary>
        /// <param name="id">ID da reserva</param>
        /// <returns>Mensagem de sucesso</returns>
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

        /// <summary>
        /// Atualiza uma reserva existente.
        /// </summary>
        /// <param name="id">ID da reserva</param>
        /// <param name="updatedReservation">Dados atualizados da reserva</param>
        /// <returns>Reserva atualizada</returns>
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
