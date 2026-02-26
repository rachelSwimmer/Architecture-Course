using server.DTOs;
using server.Interfaces;
using server.Models;
using server.Repositories;
using System.Text;

namespace server.Services
{
    public class GiftService:IGiftService
    {
        private readonly IGiftRepository _giftRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly ILogger<GiftService> _logger;
        public GiftService(IGiftRepository giftRepository, ILogger<GiftService> logger, IPurchaseRepository purchaseRepository)
        {
            _giftRepository = giftRepository;
            _logger = logger;
            _purchaseRepository = purchaseRepository;

        }
        public async Task<GiftResponseDto> AddGift(CreateGiftDto giftDto)
        {
            _logger.LogInformation("Post /add gift called");

            var gift = new Gift
            {
                Name = giftDto.Name,
                Description = giftDto.Description,
                ImageUrl = giftDto.ImageUrl,
                CategoryId = giftDto.CategoryId,
                DonorId = giftDto.DonorId
                
            };
            try
            {
                var createdGift = await _giftRepository.AddGift(gift);
                return MapToResponeseDto(createdGift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding gift");
                throw;
            }
        }
            

        public async Task<bool> DeleteGift(int id)
        {
            _logger.LogInformation("Post / delete gift {giftId} deleted", id);
            try
            {
                return await _giftRepository.DeleteGift(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting gift");
                throw;
            }
            
        }

        public async Task<IEnumerable<GiftResponseDto>> GetAll()
        {
            _logger.LogInformation("Get / all gift called");
            try
            {
                var gifts = await _giftRepository.GetAll();
                return gifts.Select(MapToResponeseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving gifts");
                throw;
            }
            
        }

        public async Task<GiftResponseDto> GetById(int id)
        {
            _logger.LogInformation("Get / get gift : {giftId} called", id);
            try
            {
                var gift = await _giftRepository.GetById(id);
                return gift != null ? MapToResponeseDto(gift) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving gift by ID");
                throw;
            }
        }
            
        public async Task<GiftResponseDto> UpdateGift(int giftId, UpdateGiftDto giftDto)
        {
            _logger.LogInformation("PUT / update gift : {giftId} called", giftId);
            try
            {
                var existingGift = await _giftRepository.GetById(giftId);
                if (existingGift == null)
                    return null;
                existingGift.Name = giftDto.Name;
                existingGift.Description = giftDto.Description;
                existingGift.ImageUrl = giftDto.ImageUrl;
                existingGift.CategoryId = giftDto.CategoryId;
                existingGift.DonorId = giftDto.DonorId;
                //existingGift.Donor = giftDto.donor;
                if (existingGift.WinnerId == null)
                {
                    existingGift.WinnerId = giftDto.WinnerId;


                }
                existingGift.IsDrown = giftDto.IsDrown;

                var updatedGift = await _giftRepository.UpdateGift(existingGift);
                return MapToResponeseDto(updatedGift);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating gift");
                throw;
            }

            
        }
        public async Task<IEnumerable<GiftResponseDto>> FilterGifts(string? giftName, string? donorName, int? buyersCount, int? categoryId)
        {
            _logger.LogInformation("Get / filter gift called");
            try
            {
                var gifts = await _giftRepository.FilterGifts(giftName, donorName, buyersCount, categoryId);
                return gifts.Select(MapToResponeseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while filter gift");
                throw;
            }



        }


        private static GiftResponseDto MapToResponeseDto(Gift gift)
        {
            return new GiftResponseDto
            {
                Id = gift.Id,
                Name = gift.Name,
                Description = gift.Description,
                ImageUrl = gift.ImageUrl,
                CategoryId = gift.CategoryId,
                DonorId = gift.DonorId,
                Donor = gift.Donor,
                WinnerId = gift.WinnerId,
                IsDrown = gift.IsDrown,
                tickets = gift.Tickets,
                Winner = gift.Winner
                
            };
        }
        public async Task<GiftResponseDto> Lottery(int giftId)
        {
            _logger.LogInformation("Put/ lottery gift called");
            try
            {
                var gift = await _giftRepository.GetById(giftId);
                if (gift.IsDrown)
                    throw new InvalidOperationException("This gift already drawn");
                var tickets = gift.Tickets.Where(t => !t.Purchase.IsDraft).ToList();
                if (!tickets.Any())
                    throw new InvalidOperationException("No tickets available for lottery");
                var random = new Random();
                var randomTicket = tickets[random.Next(tickets.Count)];
                gift.WinnerId = randomTicket.Purchase.BuyerId;
                gift.IsDrown = true;
                var result = await _giftRepository.UpdateGift(gift);
                if (result == null)
                    throw new InvalidOperationException("error");
                _logger.LogInformation("Lottery completed successfully for gift Id {GiftId}", giftId);
                return MapToResponeseDto(gift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during lottery for gift Id {GiftId}", giftId);
                throw;
            }
        }

        public async Task<byte[]> GenerateWinnersReport()
        {
            _logger.LogInformation("Generating winners report with total revenue");

            // 1. הבאת הנתונים
            var gifts = await _giftRepository.GetAll(); //

            // שליפת כל הרכישות שאינן טיוטה וסיכום ה-TotalAmount שלהן
            // הערה: יש לוודא ש-PurchaseRepository מוזרק ב-Constructor
            var allPurchases = await _purchaseRepository.GetAll();
            decimal totalRevenue = allPurchases
                .Where(p => !p.IsDraft) // רק רכישות שאינן טיוטה
                .Sum(p => p.TotalAmount); // סיכום סך ההכנסות

            using (var memoryStream = new MemoryStream())
            {
                var encoding = Encoding.UTF8;
                await memoryStream.WriteAsync(encoding.GetPreamble(), 0, encoding.GetPreamble().Length);

                using (var sw = new StreamWriter(memoryStream, encoding, leaveOpen: true))
                {
                    // כתיבת כותרות הטבלה
                    await sw.WriteLineAsync("מזהה מתנה,שם מתנה,שם הזוכה,טלפון הזוכה");

                    // כתיבת שורות הזוכים
                    foreach (var gift in gifts)
                    {
                        if (gift.IsDrown && gift.Winner != null) //
                        {
                            string line = $"{gift.Id},{gift.Name},{gift.Winner.FullName},{gift.Winner.PhoneNumber}";
                            await sw.WriteLineAsync(line);
                        }
                    }

                    // הוספת שורות רווח וסיכום הכנסות בתחתית
                    await sw.WriteLineAsync(); // שורה ריקה להפרדה
                    await sw.WriteLineAsync($",,סך כל ההכנסות:,{totalRevenue:N2} ₪");
                }

                return memoryStream.ToArray();
            }
        }


    }
}
