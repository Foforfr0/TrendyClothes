using AuctionStatistics.DAO;
using AuctionStatistics.Entities;
using AuctionStatistics.Models;
using AuctionStatistics.Models.Consult;
using AuctionStatistics.Services.Interfaces;
using System;

namespace AuctionStatistics.Services.Implements {
    public class StatisticsService : IStatisticsService {
        private readonly StatisticsDAO _statisticsDAO;

        public StatisticsService (StatisticsDAO statisticsDAO) {
            _statisticsDAO = statisticsDAO;
        }

        public async Task<MessageResponse<StatisticsAuctionDTO>> GetStatisticsAuction (int idAuction) {
            MessageResponse<Entities.AuctionsProduct> response = await _statisticsDAO.GetAuction (idAuction);

            if (response.IsError)
                return MessageResponse<StatisticsAuctionDTO>.Failure (response.Message);
            if (response.DataRetrieved == null)
                return MessageResponse<StatisticsAuctionDTO>.Success ("No se ha registrado una subasta todavía.", default);

            StatisticsAuctionDTO statistics = new StatisticsAuctionDTO {
                IdAuction = response.DataRetrieved.Id,
                FirstPrice = response.DataRetrieved.FirstPrice ?? 0,
                LastPrice = response.DataRetrieved.LastPrice ?? 0,
                NumberBids = response.DataRetrieved.BidsAuctions.Count,
                PercentageGain = (response.DataRetrieved.LastPrice > 0 ?
                    (response.DataRetrieved.LastPrice - response.DataRetrieved.FirstPrice) / response.DataRetrieved.FirstPrice * 100 : 0) ?? 0
            };

            return MessageResponse<StatisticsAuctionDTO>.Success (response.Message, statistics);
        }

        public async Task<MessageResponse<int>> GetNumberAuctions (string username, DateTime? dateStart, DateTime? dateEnd) {
            MessageResponse<int> response = await _statisticsDAO.GetNumberAuctionsAsync (username, dateStart, dateEnd);

            if (response.IsError)
                return MessageResponse<int>.Failure (response.Message);
            if (response.DataRetrieved == 0)
                return MessageResponse<int>.Success ("No se ha registrado una subasta todavía.", default);

            return MessageResponse<int>.Success (response.Message, response.DataRetrieved);
        }

        public async Task<MessageResponse<List<StatusesAuctionDTO>>> GetNumberAuctionsByStatus (string username, DateTime? dateStart, DateTime? dateEnd) {
            MessageResponse<List<StatusesAuctionDTO>> response = await _statisticsDAO.GetNumberAuctionsByStatus (username, dateStart, dateEnd);

            if (response.IsError)
                return MessageResponse<List<StatusesAuctionDTO>>.Failure (response.Message);
            if (response.DataRetrieved == null || response.DataRetrieved.Count <= 0)
                return MessageResponse<List<StatusesAuctionDTO>>.Success ("No se ha registrado una subasta todavía.", default);

            return MessageResponse<List<StatusesAuctionDTO>>.Success (response.Message, response.DataRetrieved);
        }

        public async Task<MessageResponse<int>> GetNumberBidsAuction (int idAuction) {
            MessageResponse<int> response = await _statisticsDAO.GetNumberBidsAuction (idAuction);

            if (response.IsError)
                return MessageResponse<int>.Failure (response.Message);
            if (response.DataRetrieved == 0)
                return MessageResponse<int>.Success ("No se ha registrado una subasta todavía.", default);

            return MessageResponse<int>.Success (response.Message, response.DataRetrieved);
        }

        public async Task<MessageResponse<GeneralReportDTO>> GetGeneralReport (string username, DateTime dateStart, DateTime dateEnd) {
            MessageResponse<List<AuctionsProduct>> response = await _statisticsDAO.GetAuctionsUser (username, dateStart, dateEnd);

            if (response.IsError)
                return MessageResponse<GeneralReportDTO>.Failure (response.Message);
            if (response.DataRetrieved == null || response.DataRetrieved.Count <= 0)
                return MessageResponse<GeneralReportDTO>.Success ("No se ha registrado una subasta todavía.", default);

            int totalBids = response.DataRetrieved.Sum (a => a.BidsAuctions.Count);
            List<TimeSpan>? durations = response.DataRetrieved.Select (a => a.DateEnd - a.DateStart).ToList ();
            var gains = response.DataRetrieved
                .Where (a => a.LastPrice.HasValue && a.FirstPrice.HasValue)
                .Select (a => a.LastPrice.Value - a.FirstPrice.Value)
                .ToList ();

            GeneralReportDTO? report = new GeneralReportDTO {
                TotalAuctionsCreated = response.DataRetrieved.Count,
                AverageBidsPerAuction = totalBids / (double)response.DataRetrieved.Count,
                TotalBids = totalBids,
                MaxBidsInAuction = response.DataRetrieved.Max (a => a.BidsAuctions.Count),
                MinBidsInAuction = response.DataRetrieved.Min (a => a.BidsAuctions.Count),

                HighestBid = response.DataRetrieved.Max (a => (decimal?)a.Bid),
                LowestBid = response.DataRetrieved.Min (a => (decimal?)a.Bid),

                AverageAuctionDuration = TimeSpan.FromTicks ((long)durations.Average (d => d.Ticks)),
                LongestAuctionDuration = durations.Max (),
                ShortestAuctionDuration = durations.Min (),

                MostRecentAuction = response.DataRetrieved.Max (a => a.DateStart),
                OldestAuction = response.DataRetrieved.Min (a => a.DateStart),

                HighestAuctionGain = gains.Any () ? gains.Max () : (decimal?)null,
                LowestAuctionGain = gains.Any () ? gains.Min () : (decimal?)null
            };

            return MessageResponse<GeneralReportDTO>.Success (response.Message, report);
        }
    }
}
