using server.DTOs;
using server.Models;

namespace server.Interfaces
{
    public interface IDonorService
    {
        Task<IEnumerable<DonorResponseDto>> GetAll();
        Task<DonorResponseDto> GetById(string id);
        Task<DonorResponseDto> AddDonor(DonorCreateDto donor);
        Task<DonorResponseDto> UpdateDonor(string id,DonorUpdateDto donor);
        Task<bool> DeleteDonor(string id);
        Task<IEnumerable<DonorResponseDto>> FilterDonors(string? name, string? email, int? giftId);

    }
}
