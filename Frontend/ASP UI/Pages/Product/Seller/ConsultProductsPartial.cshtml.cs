using Microsoft.AspNetCore.Mvc.RazorPages;
using WebPage.DTO.Product.MyProducts;

namespace WebPage.Pages.Product {
    public class ConsultProductsPartialModel : PageModel {
        public readonly MyProductsDTO product;

        public void OnGet () {
        }
    }
}
