using AuctionStatistics.Entities;
using AuctionStatistics.Models;
using AuctionStatistics.Models.Consult;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AuctionStatistics.DAO {
    public class StatisticsDAO {
        private readonly TrendyClothesDBContext _context;

        public StatisticsDAO (TrendyClothesDBContext context) {
            _context = context;
        }

        public async Task<MessageResponse<Entities.AuctionsProduct>> GetAuction (int idAuction) {
            try {
                Entities.AuctionsProduct? response = await _context.AuctionsProducts
                    .Include (a => a.Seller)
                    .Include (a => a.BidsAuctions)
                    .Where (a => a.Id == idAuction)
                    .FirstOrDefaultAsync ();
                if (response == null) {
                    return MessageResponse<Entities.AuctionsProduct>.Success ("No se ha registrado una subasta todavía.", default);
                }
                return MessageResponse<Entities.AuctionsProduct>.Success ("Subasta encontrada.", response);
            } catch (Exception ex) {
                return MessageResponse<Entities.AuctionsProduct>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<int>> GetNumberAuctionsAsync (string username, DateTime? dateStart, DateTime? dateEnd) {
            try {
                int response = 0;
                if (dateStart == null || dateEnd == null) {
                    response = await _context.AuctionsProducts
                        .Include (p => p.Seller)
                        .Where (a => a.Seller.Username.Equals (username))
                        .CountAsync ();
                } else {
                    response = await _context.AuctionsProducts
                        .Include (p => p.Seller)
                        .Where (a =>
                            a.DateStart >= dateStart &&
                            a.DateEnd <= dateEnd &&
                            a.Seller.Username.Equals (username))
                        .CountAsync ();
                }
                return MessageResponse<int>.Success ($"Número de subastas realizadas {response}.", response);
            } catch (Exception ex) {
                return MessageResponse<int>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<List<StatusesAuctionDTO>>> GetNumberAuctionsByStatus (string username, DateTime? dateStart, DateTime? dateEnd) {
            try {
                IQueryable<AuctionsProduct>? query = _context.AuctionsProducts
                    .Include (a => a.Seller)
                    .Include (a => a.Status)
                    .Where (a => a.Seller.Username == username);

                if (dateStart.HasValue && dateEnd.HasValue) {
                    query = query.Where (a =>
                        a.DateStart >= dateStart.Value &&
                        a.DateEnd <= dateEnd.Value);
                }

                List<StatusesAuctionDTO>? response = await query
                    .GroupBy (a => a.Status.Status)
                    .Select (g => new StatusesAuctionDTO {
                        Name = g.Key,
                        Count = g.Count ()
                    })
                    .ToListAsync ();

                return MessageResponse<List<StatusesAuctionDTO>>.Success ("Número de subastas por status obtenidos.", response);
            } catch (Exception ex) {
                return MessageResponse<List<StatusesAuctionDTO>>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<int>> GetNumberBidsAuction (int idAuction) {
            try {
                int response = await _context.BidsAuctions
                    .Include (b => b.Auction)
                    .Where (b =>
                        b.AuctionId == idAuction)
                    .CountAsync ();

                return MessageResponse<int>.Success ($"Número de pujas realizadas en la subasta: {response}.", response);
            } catch (Exception ex) {
                return MessageResponse<int>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<List<AuctionsProduct>>> GetAuctionsUser (string username, DateTime? dateStart, DateTime? dateEnd) {
            try {
                DateTime minSqlDate = new DateTime (1753, 1, 1);

                List<AuctionsProduct> response;
                if (!dateStart.HasValue || !dateEnd.HasValue || dateStart < minSqlDate || dateEnd < minSqlDate) {
                    response = await _context.AuctionsProducts
                        .Include (p => p.Seller)
                        .Include (p => p.Status)
                        .Include (p => p.BidsAuctions)
                        .Where (a => a.Seller.Username.Equals (username))
                        .ToListAsync ();
                } else {
                    response = await _context.AuctionsProducts
                        .Include (p => p.Seller)
                        .Include (p => p.Status)
                        .Include (p => p.BidsAuctions)
                        .Where (a =>
                            a.DateStart >= dateStart &&
                            a.DateEnd <= dateEnd &&
                            a.Seller.Username.Equals (username))
                        .ToListAsync ();
                }
                return MessageResponse<List<AuctionsProduct>>.Success ($"Número de subastas realizadas {response.Count()}.", response);
            } catch (Exception ex) {
                return MessageResponse<List<AuctionsProduct>>.Failure ($"Error interno del servidor: {ex.Message}");

            }
        }
    }
}
