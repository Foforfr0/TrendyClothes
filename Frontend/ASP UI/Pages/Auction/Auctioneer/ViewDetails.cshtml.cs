using Microsoft.AspNetCore.Mvc.RazorPages;
using WebPage.DTO.Auction;

namespace WebPage.Pages.Auction.Auctioneer {
    public class ViewDetailsModel : PageModel {
        public MyAuctionDetailsDTO auction {
            get; set;
        }

        public void OnGet () {
        }
    }
}
