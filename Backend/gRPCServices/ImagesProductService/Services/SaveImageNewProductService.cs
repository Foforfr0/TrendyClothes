using Grpc.Core;
using ImagesProductService.DAO;
using SaveImageNewProduct;

namespace ImagesProductService.Services {
    public class SaveNewImageServiceImpl : SaveNewImageService.SaveNewImageServiceBase {
        private readonly ImageProductDAO _SaveNewImageDAO;

        public SaveNewImageServiceImpl (ImageProductDAO SaveNewImageDAO) {
            _SaveNewImageDAO = SaveNewImageDAO;
        }

        public override async Task<SaveNewImageReply> SaveNewImage (SaveNewImageRequest request, ServerCallContext context) {
            if (request.ProductId == 0)
                return new SaveNewImageReply {
                    Message = "Id de producto no recibido.",
                    Success = false
                };

            if (string.IsNullOrEmpty (request.MimeType))
                return new SaveNewImageReply {
                    Message = "Mime de imagen no recibida.",
                    Success = false
                };

            bool response = await _SaveNewImageDAO.SaveOneImage (request.ProductId, Convert.FromBase64String (request.ImageBase64));

            if (response) {
                throw new RpcException (new Status (StatusCode.Internal, "Error interno del servidor."));
            }

            return new SaveNewImageReply {
                Message = "Imagen actualizada.",
                Success = true
            };
        }
    }
}
