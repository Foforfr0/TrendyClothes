using Microsoft.AspNetCore.Mvc.RazorPages;
using WebPage.DTO.Auction;

namespace WebPage.Pages.Auction.Auctioneer {
    public class ConsultMyAuctionsPartialModel : PageModel {
        public readonly MyAuctionsDTO auction;

        public void OnGet () {
        }
    }
}
