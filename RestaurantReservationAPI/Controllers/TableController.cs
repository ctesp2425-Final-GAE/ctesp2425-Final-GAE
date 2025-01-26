using Microsoft.AspNetCore.Mvc;
using RestaurantReservationAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantReservationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        // Lista estática de mesas (apenas em memória)
        public static readonly List<Table> Tables = new List<Table>
        {
            new Table { Id = 1, Name = "Table 1" },
            new Table { Id = 2, Name = "Table 2" },
            new Table { Id = 3, Name = "Table 3" },
            new Table { Id = 4, Name = "Table 4" },
            new Table { Id = 5, Name = "Table 5" },
            new Table { Id = 6, Name = "Table 6" },
            new Table { Id = 7, Name = "Table 7" },
            new Table { Id = 8, Name = "Table 8" },
            new Table { Id = 9, Name = "Table 9" },
            new Table { Id = 10, Name = "Table 10" },
            new Table { Id = 11, Name = "Table 11" },
            new Table { Id = 12, Name = "Table 12" },
            new Table { Id = 13, Name = "Table 13" },
            new Table { Id = 14, Name = "Table 14" },
            new Table { Id = 15, Name = "Table 15" }
        };

        // GET: api/Table
        [HttpGet]
        public IActionResult GetTables()
        {
            return Ok(Tables);
        }

        // GET: api/Tables/{id}
        [HttpGet("{id}")]
        public IActionResult GetTable(int id)
        {
            var table = Tables.FirstOrDefault(t => t.Id == id);
            if (table == null)
            {
                return NotFound(new { message = "Table not found" });
            }

            return Ok(table);
        }

        // POST: api/Tables
        [HttpPost]
        public IActionResult AddTable([FromBody] Table newTable)
        {
            if (newTable == null)
            {
                return BadRequest(new { message = "Invalid table data" });
            }

            newTable.Id = Tables.Count > 0 ? Tables.Max(t => t.Id) + 1 : 1; // Gerar um novo Id
            Tables.Add(newTable);

            return CreatedAtAction(nameof(GetTable), new { id = newTable.Id }, newTable);
        }

        // DELETE: api/Tables/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteTable(int id)
        {
            var table = Tables.FirstOrDefault(t => t.Id == id);
            if (table == null)
            {
                return NotFound(new { message = "Table not found" });
            }

            Tables.Remove(table);

            return NoContent();
        }
    }
}
