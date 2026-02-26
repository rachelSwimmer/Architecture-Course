using server.DTOs;
using server.Models;

namespace server.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketResponseDtos>> GetAll();
        Task<TicketResponseDtos> GetById(int id);
        Task<TicketResponseDtos> AddTicket(TicketCreateDtos ticket);
        Task<TicketResponseDtos> UpdateTicket(int ticketId,TicketUpdateDtos ticket);
        Task<bool> DeleteTicket(int id);
    }
}
