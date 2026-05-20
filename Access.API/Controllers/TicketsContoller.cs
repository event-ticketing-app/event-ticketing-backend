namespace Acces.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService=ticketService;
        }


        [Authorize]
        [HttpPost("reserve")]

        public async Task<IActionResult> Reserve(int EventId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var reservedTicket = await _ticketService.ReserveTicket( EventId, userId);

            return CreatedAtAction(nameof(Reserve), reservedTicket);

        }
    }

}