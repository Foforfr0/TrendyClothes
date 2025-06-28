using Grpc.Core;
using ImagesProductService.DAO;
using SaveImageProduct;

namespace ImagesProductService.Services {
    public class SaveImageServiceImpl : SaveImageService.SaveImageServiceBase {
        private readonly ImageProductDAO _saveImageDAO;

        public SaveImageServiceImpl (ImageProductDAO saveImageDAO) {
            _saveImageDAO = saveImageDAO;
        }

        public override async Task<SaveImageReply> SaveImage (SaveImageRequest request, ServerCallContext context) {
            if(request.ProductId == 0)
                return new SaveImageReply {
                    Message = "Id de producto no recibido.",
                    Success = false
                };

            if(string.IsNullOrEmpty(request.MimeType))
                return new SaveImageReply {
                    Message = "Mime de imagen no recibida.",
                    Success = false
                };

            bool response = await _saveImageDAO.SaveOneImage (request.ProductId, Convert.FromBase64String (request.ImageBase64));

            if (response) {
                throw new RpcException (new Status (StatusCode.Internal, "Error interno del servidor."));
            }   

            return new SaveImageReply {
                Message = "Imagen actualizada.",
                Success = true
            };
        }
    }
}
