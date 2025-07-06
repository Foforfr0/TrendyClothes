using GetImageProduct;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebPage.Connections;
using WebPage.DTO;
using WebPage.DTO.Auction;

namespace WebPage.Pages.Auction.Auctioneer {
    public class CreateAuctionModel : PageModel {
        public CreateAuctionModel () {
        }

        [BindProperty]
        public CreateAuctionDTO newAuction {
            get; set;
        } = new CreateAuctionDTO ();

        public void OnGet () {
        }
    }
}
