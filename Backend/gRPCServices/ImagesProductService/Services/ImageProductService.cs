using Google.Protobuf;
using Grpc.Core;
using ImageProduct;
using ImagesProductService.DAO;
using ImagesProductService.DTO;

namespace ImagesProductService.Services {
    public class ImageProductServiceImpl : ImageProductService.ImageProductServiceBase {
        private readonly ImageProductDAO _imageProductDAO;

        public ImageProductServiceImpl (ImageProductDAO imageProductDAO) {
            _imageProductDAO = imageProductDAO;
        }

        public override async Task<ImageProductReply> GetImage (ImageProductRequest request, ServerCallContext context) {
            OneImageDTO? image = await _imageProductDAO.GetOneImage (request.ProductId);

            if (image == null || image.image.Length <= 0) {
                throw new RpcException (new Status (StatusCode.NotFound, "Producto o imagen no encontrada."));
            }

            return new ImageProductReply {
                ImageData = ByteString.CopyFrom (image.image),
                ImageType = image.mime
            };
        }
    }
}
