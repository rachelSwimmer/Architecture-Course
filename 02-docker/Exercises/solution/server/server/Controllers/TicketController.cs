using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Interfaces;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ILogger<TicketController> _logger;


        public TicketController(ITicketService ticketService, ILogger<TicketController> logger)
        {
            _ticketService = ticketService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<TicketResponseDtos>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TicketResponseDtos>>> GetAll()
        {
            var ticket = await _ticketService.GetAll();
            return Ok(ticket);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TicketResponseDtos), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById(int id)
        {
            var ticket = await _ticketService.GetById(id);
            if (ticket == null)
            {
                return NotFound(new { message = $"Ticket with ID {id} not exist" });
            }
            return Ok(ticket);
        }
        [HttpPost]
        [ProducesResponseType(typeof(TicketResponseDtos), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddTicket([FromBody] TicketCreateDtos createDto)
        {
            try
            {
                var ticket = await _ticketService.AddTicket(createDto);
                //return CreatedAtAction(nameof(GetById), new { id = category.Id, category });
                return Ok(ticket);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { messege = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(TicketResponseDtos), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var reasult = await _ticketService.DeleteTicket(id);
            if (!reasult)
            {
                return NotFound(new { message = $"Ticket with Id {id} not found" });
            }
            return NoContent();
        }

        [HttpPut("{ticketId}")]
        [ProducesResponseType(typeof(TicketResponseDtos), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateTicktet([FromRoute] int ticketId, [FromBody] TicketUpdateDtos updateDto)
        {
            try
            {
                var ticket = await _ticketService.UpdateTicket(ticketId,updateDto);
                //return CreatedAtAction(nameof(GetById), new { id = category.Id, category });
                return Ok(ticket);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { messege = ex.Message });
            }
        }
    }
}
