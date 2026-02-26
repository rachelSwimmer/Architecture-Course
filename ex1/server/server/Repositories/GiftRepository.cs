using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces;
using server.Models;
using System.Linq;

namespace server.Repositories
{
    public class GiftRepository:IGiftRepository
    {
        private readonly ApplicationDbContext _context;
        public GiftRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Gift> AddGift(Gift gift)
        {
            _context.Gifts.Add(gift);
            await _context.SaveChangesAsync();
            return gift;
        }

        public async Task<bool> DeleteGift(int id)
        {
            var gift = await _context.Gifts.FindAsync(id);
            if (gift == null) return false;
            _context.Gifts.Remove(gift);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Gift>> GetAll()
        {
            return await _context.Gifts
                .Include(g => g.Donor)
                .ToListAsync();
        }

        public async Task<Gift> GetById(int id)
        {
            return await _context.Gifts
                   .Where(g => g.Id == id)
                   .Include(g => g.Donor)       
                   .Include(g => g.Tickets.Where(t => !t.Purchase.IsDraft))
                   .ThenInclude(g => g.Purchase)
                   .Include(g=>g.Winner)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Gift> UpdateGift(Gift gift)
        {
            _context.Gifts.Update(gift);
            await _context.SaveChangesAsync();
            return gift;
        }

        public async Task<IEnumerable<Gift>> FilterGifts(string? giftName, string? donorName, int? buyersCount,int? categoryId)
        {

            var query = _context.Gifts
                .Include(g => g.Donor)
                .Include(g => g.Tickets)
                    .ThenInclude(t => t.Purchase)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(giftName))
            {
                query = query.Where(g => g.Name.Contains(giftName));
            }
            if (!string.IsNullOrWhiteSpace(donorName))
            {
                query = query.Where(g => g.Donor.Name.Contains(donorName));
            }
            if (categoryId.HasValue){
                query = query.Where(g => g.CategoryId == categoryId);
               
            }
            if (buyersCount.HasValue)
            {
                query = query.Where(g =>
                    g.Tickets
                    .Where(t => !t.Purchase.IsDraft)
                    .Select(t => t.PurchaseId)
                    .Distinct()
                    .Count() == buyersCount.Value

                );
            }
            return await query.ToListAsync();
        }


    }
}
