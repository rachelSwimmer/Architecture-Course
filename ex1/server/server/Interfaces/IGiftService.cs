using server.DTOs;
using server.Models;

namespace server.Interfaces
{
    public interface IGiftService
    {
        Task<IEnumerable<GiftResponseDto>> GetAll();
        Task<GiftResponseDto> GetById(int id);
        Task<GiftResponseDto> AddGift(CreateGiftDto gift);
        Task<GiftResponseDto> UpdateGift(int giftId,UpdateGiftDto gift);
        Task<bool> DeleteGift(int id);
        Task<GiftResponseDto> Lottery(int giftId);
        Task<IEnumerable<GiftResponseDto>> FilterGifts(string? giftName, string? donorName, int? buyersCount,int? categoryId);

        Task<byte[]> GenerateWinnersReport();
    }
}
