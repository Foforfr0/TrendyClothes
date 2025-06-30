using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.DAO.Auction
{
    public class AuctionDAO
    {
        private readonly TrendyClothesDBContext _context;

        public AuctionDAO(TrendyClothesDBContext context)
            {
                _context = context;
            }

            public async Task<AuctionsProduct?> GetByIdAsync(int id)
            {
                return await _context.AuctionsProducts
                    .Include(a => a.Product)
                    .Include(a => a.Status)
                    .Include(a => a.Buyer)
                    .FirstOrDefaultAsync(a => a.Id == id);
            }

            public async Task<bool> PlaceBidAsync(int auctionId, int userId, decimal amount)
            {
                var auction = await GetByIdAsync(auctionId);
                if (auction == null || auction.Status.Status != "Active") return false;
                if (auction.LastPrice.HasValue && amount <= auction.LastPrice) return false;
                if (!auction.LastPrice.HasValue && amount < auction.FirstPrice) return false;

                auction.LastPrice = amount;
                auction.BuyerId = userId;
                await _context.SaveChangesAsync();
                return true;
            }
        }
}
