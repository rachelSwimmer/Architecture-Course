using server.DTOs;
using server.Interfaces;
using server.Models;
using server.Repositories;

namespace server.Services
{
    public class TicketService:ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IGiftService _giftService;
        private readonly ILogger<TicketService> _logger;


        public TicketService(ITicketRepository ticketRepository,IGiftService giftService, ILogger<TicketService> logger)
        {
            _ticketRepository = ticketRepository;
            _giftService = giftService;
            _logger = logger;
        }
        public async Task<TicketResponseDtos> AddTicket(TicketCreateDtos ticketDto)
        {
            _logger.LogInformation("Post /add ticket called");
            try
            {
                var gift = await _giftService.GetById(ticketDto.GiftId);
                if (gift.IsDrown)
                    throw new InvalidOperationException("this gift alredy drown");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while get gift by Id");
                throw;
            }


            var ticket = new Ticket
            {
                PurchaseId = ticketDto.PurchaseId,
                GiftId = ticketDto.GiftId,
                Quantity = ticketDto.Quantity
            };
            try
            {
                var createdTicket = await _ticketRepository.AddTicket(ticket);

                return MapToResponeseDto(createdTicket);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding ticket");
                throw;
            }
            
        }

        public async Task<bool> DeleteTicket(int id)
        {
            _logger.LogInformation("Post / delete ticket {ticketId} deleted", id);
            try
            {
                return await _ticketRepository.DeleteTicket(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting ticket");
                throw;
            }
            
        }

        public async Task<IEnumerable<TicketResponseDtos>> GetAll()
        {
            _logger.LogInformation("Get / all ticket called");
            try
            {
                var tickets = await _ticketRepository.GetAll();
                return tickets.Select(MapToResponeseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving ticket");
                throw;
            }
            
        }

        public async Task<TicketResponseDtos> GetById(int id)
        {
            _logger.LogInformation("Get / get ticket : {ticketId} called", id);
            try
            {
                var ticket = await _ticketRepository.GetById(id);
                return ticket != null ? MapToResponeseDto(ticket) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving ticket by ID");
                throw;
            }
            
        }
        public async Task<TicketResponseDtos> UpdateTicket(int ticketId,TicketUpdateDtos ticketDto)
        {
            _logger.LogInformation("PUT / update ticket : {ticketId} called", ticketId);
            try
            {
                var existingTicket = await _ticketRepository.GetById(ticketId);
                if (existingTicket == null)
                    return null;
                existingTicket.PurchaseId = ticketDto.PurchaseId;
                existingTicket.GiftId = ticketDto.GiftId;
                existingTicket.Quantity = ticketDto.Quantity;
                var updatedTicket = await _ticketRepository.UpdateTicket(existingTicket);
                return MapToResponeseDto(updatedTicket);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating ticket");
                throw;
            }
            
        }



        private static TicketResponseDtos MapToResponeseDto(Ticket ticket)
        {
            return new TicketResponseDtos
            {
                Id = ticket.Id,
                PurchaseId = ticket.PurchaseId,
                GiftId = ticket.GiftId,
                Quantity = ticket.Quantity
            };
        }
    }
}
