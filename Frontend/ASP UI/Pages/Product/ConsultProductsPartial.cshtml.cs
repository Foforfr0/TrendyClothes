using Microsoft.AspNetCore.Mvc.RazorPages;
using WebPage.DTO.Product.Consult;

namespace WebPage.Pages.Product {
    public class ConsultProductsPartialModel : PageModel {
        public readonly SearchProductsDTO product;

        public void OnGet () {
        }
    }
}
