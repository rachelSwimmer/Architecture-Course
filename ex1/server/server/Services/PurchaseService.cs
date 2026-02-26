using server.DTOs;
using server.Interfaces;
using server.Models;
using System.Text;

namespace server.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly ITicketService _ticketService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configurtaion;
        private readonly ILogger<PurchaseService> _logger;
        
        
        public PurchaseService(IConfiguration configuration, IPurchaseRepository purchaseRepository,ITicketService ticketService,ILogger<PurchaseService> logger,IUserService userService)
        {
            _purchaseRepository = purchaseRepository;
            _ticketService = ticketService;
            _logger = logger;
            _userService = userService;
            _configurtaion = configuration;
           
        }
        public async Task<PurchaseResponseDtos> AddPurchase(PurchaseCreateDtos PurchaseDto)
        {
            _logger.LogInformation("Post /add purchase called");

            var purchase = new Purchase
            {
                BuyerId = PurchaseDto.BuyerId,
                TotalAmount = PurchaseDto.TotalAmount,
                OrderDate = PurchaseDto.OrderDate,
                
            };
            try
            {
                var createdPurchase = await _purchaseRepository.AddPurchase(purchase);
                return MapToResponeseDto(createdPurchase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding purchase");
                throw;
            }
            
        }

        public async Task<bool> DeletePurchase(int id)
        {
            _logger.LogInformation("Post / delete purchase {purchaseId} deleted", id);
            try
            {
                return await _purchaseRepository.DeletePurchase(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting purchase");
                throw;
            }
            
        }

        public async Task<IEnumerable<PurchaseResponseDtos>> GetAll()
        {
            _logger.LogInformation("Get / all purchase called");
            try
            {
                var purchases = await _purchaseRepository.GetAll();
                return purchases.Select(MapToResponeseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving purchase");
                throw;
            }
           
        }

        public async Task<PurchaseResponseDtos> GetById(int id)
        {
            _logger.LogInformation("Get / get purchase : {purchaseId} called", id);
            try
            {
                var purchase = await _purchaseRepository.GetById(id);
                return purchase != null ? MapToResponeseDto(purchase) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving purchase by ID");
                throw;
            }
            
        }

        public async Task<PurchaseResponseDtos> GetByUserId(string userId)
        {
            _logger.LogInformation("Get / get purchase by user : {userId} called", userId);
            try
            {
                var purchase = await _purchaseRepository.GetByUserId(userId);
                return purchase != null ? MapToResponeseDto(purchase) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving purchase by userID");
                throw;
            }
        }
           
        public async Task<PurchaseResponseDtos> UpdatePurchase(int purchaseId, PurchaseUpdateDtos purchaseDto)
        {
            _logger.LogInformation("PUT / update purchase : {purchaseId} called", purchaseId);
            try
            {
                var existingPurchase = await _purchaseRepository.GetById(purchaseId);
                if (existingPurchase == null)
                    return null;
                existingPurchase.BuyerId = purchaseDto.BuyerId;
                existingPurchase.TotalAmount = purchaseDto.TotalAmount;
                existingPurchase.OrderDate = purchaseDto.OrderDate;
                existingPurchase.IsDraft = purchaseDto.IsDraft;
                var updatedPurchase = await _purchaseRepository.UpdatePurchase(existingPurchase);
                return MapToResponeseDto(updatedPurchase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating purchase");
                throw;
            }
            
        }

        public async Task<PurchaseResponseDtos> AddTickeToPurchase( TicketCreateDtos ticketDto)
        {
            _logger.LogInformation("POST / add ticket to purchase : {purchaseId} called", ticketDto.PurchaseId);
            try
            {
                var purchase = await _purchaseRepository.GetById(ticketDto.PurchaseId);
                if (purchase == null) return null;

                if (!purchase.IsDraft)
                    throw new InvalidOperationException("cannot modigy a finalized purchase");
                int remainig = GetRemainingTicketsCount(purchase);

                if (remainig < ticketDto.Quantity)
                {
                    throw new InvalidOperationException("you need to buy a new package");
                }
                var ticket = await _ticketService.AddTicket(ticketDto);
                var existTicket = purchase.Tickets.FirstOrDefault(t => t.Id == ticket.Id);
                if (existTicket != null)
                {       
                 
                    await _ticketService.UpdateTicket(existTicket.Id, new TicketUpdateDtos
                    {
                        GiftId = existTicket.GiftId,
                        PurchaseId = existTicket.PurchaseId,
                        Quantity = existTicket.Quantity

                    });
                   
                    return MapToResponeseDto(purchase);
                }
                var update = await _purchaseRepository.AddTicketToPurchase( new Ticket
                {
                    Id = ticket.Id,
                    GiftId = ticket.GiftId,
                    PurchaseId = ticket.PurchaseId,
                    Quantity = ticket.Quantity
                });
                return MapToResponeseDto(update);



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while add ticket to purchase");
                throw;
            }
            
        }
        public async Task<PurchaseResponseDtos> DeleteTicketFromPurchase(int purchaseId,int ticketId)
        {
            _logger.LogInformation("POST / delete ticket from purchase : {purchaseId} called", purchaseId);
            try
            {
                var purchase = await _purchaseRepository.GetById(purchaseId);
                if (purchase == null) return null;

                if (!purchase.IsDraft)
                    throw new InvalidOperationException("cannot modigy a finalized purchase");

                var update = await _purchaseRepository.DeleteTicketFromPurchase(purchaseId, ticketId);
                return MapToResponeseDto(update);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while delete ticket from purchase");
                throw;
            }
           
                
        }
        public async Task<PurchaseResponseDtos> AddPackageToPurchase(int purchaseId, int  packageId)
        {
            _logger.LogInformation("POST / add package to purchase : {purchaseId} called", purchaseId);
            try
            {
                var purchase = await _purchaseRepository.GetById(purchaseId);
                if (purchase == null) return null;

                if (!purchase.IsDraft)
                    throw new InvalidOperationException("cannot modigy a finalized purchase");


                var update = await _purchaseRepository.AddPackageToPurchase(purchaseId, packageId);
               
                return MapToResponeseDto(update);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while add package to purchase");
                throw;
            }
            
        }
        public async Task<PurchaseResponseDtos> DeletePackageFromPurchase(int purchaseId, int packageId)
        {
            _logger.LogInformation("POST / delet package from purchase : {purchaseId} called", purchaseId);
            try
            {
                var purchase = await _purchaseRepository.GetById(purchaseId);
                if (purchase == null) return null;

                if (!purchase.IsDraft)
                    throw new InvalidOperationException("cannot modigy a finalized purchase");



                var update = await _purchaseRepository.DeletePackageFromPurchase(purchaseId, packageId);
                return MapToResponeseDto(update);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while delet package from purchase");
                throw;
            }


        }

        private int GetRemainingTicketsCount(Purchase purchase)
        {
            _logger.LogInformation("GET / Get Remaining Tickets Count");
            try
            {

                int remainingTicketsCount = purchase.PurchasePackages?
                    .Where(pp => pp.Package != null)
                    .Sum(pp => pp.Package.Quentity * (pp.Quantity)) ?? 0;
                int useTicket = purchase.Tickets?.Count ?? 0;
                int remaining = remainingTicketsCount - useTicket;
                return remaining > 0 ? remaining : 0;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Get Remaining Tickets Count");
                throw;
            }
        }
         public async Task<PurchaseResponseDtos> CompletionPurchase(int purchaseId,PurchaseUpdateDtos purchase)
        {
            _logger.LogInformation("PUT complete purchase");
            try
            {
                var existPurchase = await _purchaseRepository.GetById(purchaseId);
                var sum = existPurchase.PurchasePackages?
                    .Where(pp => pp.Package != null)
                    .Sum(pp => pp.Package.Price * (pp.Quantity)) ?? 0;

                var updatePurchase = await UpdatePurchase(purchaseId, new PurchaseUpdateDtos
                {
                    BuyerId = purchase.BuyerId,
                    IsDraft = false,
                    TotalAmount = sum,

                });
                var newPurchase =await _purchaseRepository.AddPurchase(new Purchase
                {

                    BuyerId = purchase.BuyerId

                });
               

                var user =await _userService.GetById(purchase.BuyerId);

                if (user == null)
                {
                    throw new Exception("user does not exist");
                }
                else
                {
                    try
                    {
                        sendEmail(updatePurchase, user, sum);

                    }
                    catch (Exception ex)
                    {

                        _logger.LogError(ex, "Failed to send email or ");
                    }
                }
                return updatePurchase; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while completing purchase");
                throw;
            }
        }






        private  PurchaseResponseDtos MapToResponeseDto(Purchase purchase)
        {
            return new PurchaseResponseDtos
            {
                Id = purchase.Id,
                BuyerId = purchase.BuyerId,
                Buyer = purchase.Buyer,
                TotalAmount = purchase.TotalAmount,
                OrderDate = purchase.OrderDate,
                IsDraft = purchase.IsDraft,
                PurchasePackages = purchase.PurchasePackages,
                Tickets = purchase.Tickets
            };
        }

        private  async void sendEmail(PurchaseResponseDtos purchase,UserResponseDto user,int sum)
        {
            try
            {
                var mailText = $"שלום {user.FullName}, הרכישה שלך על סך {sum} שח הושלמה בהצלחה.";

                var emailData = new
                {
                    personalizations = new[]
                        {
                                    new { to = new[] { new { email = user.Email } } }
                                },
                    from = new { email = "rivki0259@gmail.com" },
                    subject = $"אישור רכישה - הזמנה {purchase.Id}",
                    content = new[]
                        {
                                     new { type = "text/plain", value = $"שלום {user.FullName}, הרכישה שלך על סך {sum} שח הושלמה בהצלחה." }
                                 }
                };

                var jsonPayload = System.Text.Json.JsonSerializer.Serialize(emailData);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var apiKey = _configurtaion["SendGrid:ApiKey"]; // ודאי איות נכון!

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                    var response = await client.PostAsync("https://api.sendgrid.com/v3/mail/send", content);

                    var responseBody = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("SendGrid Status: {0}, Body: {1}", response.StatusCode, responseBody);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email");
            }


        }
        


    }
}
