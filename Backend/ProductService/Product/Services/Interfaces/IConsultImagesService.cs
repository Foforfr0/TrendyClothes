using ProductService.Models;

namespace ProductService.Services.Interfaces {
    public interface IConsultImagesService {
        public Task<MessageResponse<byte[]>> GetImageProductId (int productId);
    }
}
