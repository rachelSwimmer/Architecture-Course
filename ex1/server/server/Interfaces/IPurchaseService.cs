using server.DTOs;
using server.Models;

namespace server.Interfaces
{
    public interface IPurchaseService
    {
        Task<IEnumerable<PurchaseResponseDtos>> GetAll();
        Task<PurchaseResponseDtos> GetById(int id);
        Task<PurchaseResponseDtos> GetByUserId(string userId);
        Task<PurchaseResponseDtos> AddPurchase(PurchaseCreateDtos purchase);
        Task<PurchaseResponseDtos> UpdatePurchase(int purchaseId,PurchaseUpdateDtos purchase);
        Task<bool> DeletePurchase(int id);
        Task<PurchaseResponseDtos> AddTickeToPurchase( TicketCreateDtos tikcet);
        Task<PurchaseResponseDtos> DeleteTicketFromPurchase(int purchaseId, int tikcet);
        Task<PurchaseResponseDtos> AddPackageToPurchase(int purchaseId, int packageId);
        Task<PurchaseResponseDtos> DeletePackageFromPurchase(int purchaseId, int package);
        Task<PurchaseResponseDtos> CompletionPurchase(int purchaseId,PurchaseUpdateDtos purchase);

    }
}
