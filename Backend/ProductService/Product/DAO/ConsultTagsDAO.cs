using Microsoft.EntityFrameworkCore;
using ProductService.Entities;
using ProductService.Models;

namespace ProductService.DAO {
    public class ConsultTagsDAO {
        private readonly TrendyClothesDBContext _context;
        readonly ILogger<ConsultTagsDAO> _logger;

        public ConsultTagsDAO (TrendyClothesDBContext context, ILogger<ConsultTagsDAO> logger) {
            _context = context;
            _logger = logger;
        }

        public async Task<MessageResponse<List<Entities.CategoriesProduct>>> GetCategoriesAsync () {
            try {
                List<Entities.CategoriesProduct> response = await _context.CategoriesProducts.ToListAsync ();

                return MessageResponse<List<Entities.CategoriesProduct>>.Success ("", response);
            } catch (Exception ex) {
                return MessageResponse<List<Entities.CategoriesProduct>>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<List<Entities.TypesProduct>>> GetTypesAsync () {
            try {
                List<Entities.TypesProduct> response = await _context.TypesProducts.ToListAsync ();

                return MessageResponse<List<Entities.TypesProduct>>.Success ("", response);
            } catch (Exception ex) {
                return MessageResponse<List<Entities.TypesProduct>>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }

        public async Task<MessageResponse<List<Entities.StatusesProduct>>> GetStatussesAsync () {
            try {
                List<Entities.StatusesProduct> response = await _context.StatusesProducts.ToListAsync ();

                return MessageResponse<List<Entities.StatusesProduct>>.Success ("", response);
            } catch (Exception ex) {
                return MessageResponse<List<Entities.StatusesProduct>>.Failure ($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
