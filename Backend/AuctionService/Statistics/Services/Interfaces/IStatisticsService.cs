using AuctionStatistics.Models;
using AuctionStatistics.Models.Consult;
using Microsoft.AspNetCore.Mvc;

namespace AuctionStatistics.Services.Interfaces {
    public interface IStatisticsService {
        public Task<MessageResponse<StatisticsAuctionDTO>> GetStatisticsAuction (int idAuction);
        public Task<MessageResponse<int>> GetNumberAuctions (string username, DateTime? dateStart, DateTime? dateEnd);
        public Task<MessageResponse<List<StatusesAuctionDTO>>> GetNumberAuctionsByStatus  (string username, DateTime? dateStart, DateTime? dateEnd);
        public Task<MessageResponse<int>> GetNumberBidsAuction (int idAuction);
        public Task<MessageResponse<GeneralReportDTO>> GetGeneralReport (string username, DateTime dateStart, DateTime dateEnd);
    }
}
