using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebPage.Pages.Auction.Auctioneer {
    public class CreateAuctionModel : PageModel {

        [BindProperty (SupportsGet = true)]
        public int idProduct { get; set;
        }

        public void OnGet () {
        }
    }
}
