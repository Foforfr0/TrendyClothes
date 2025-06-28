using Microsoft.AspNetCore.Mvc.RazorPages;
using WebPage.DTO.Product.Consult;

namespace WebPage.Pages.Product.Buyer {
    public class ConsultProductsPartialModel : PageModel {
        public readonly ProductsDTO product;

        public void OnGet () {
        }
    }
}
