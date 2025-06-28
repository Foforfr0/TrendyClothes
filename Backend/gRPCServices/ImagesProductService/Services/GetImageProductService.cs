using GetImageProduct;
using Google.Protobuf;
using Grpc.Core;
using ImagesProductService.DAO;
using ImagesProductService.DTO;

namespace ImagesProductService.Services {
    public class GetImageServiceImpl : GetImageService.GetImageServiceBase {
        private readonly ImageProductDAO _getImageDAO;

        public GetImageServiceImpl (ImageProductDAO getImageDAO) {
            _getImageDAO = getImageDAO;
        }

        public override async Task<GetImageReply> GetImage (GetImageRequest request, ServerCallContext context) {
            OneImageDTO? image = await _getImageDAO.GetOneImage (request.ProductId);

            if (image == null || image.image.Length <= 0) {
                throw new RpcException (new Status (StatusCode.NotFound, "Producto o imagen no encontrada."));
            }

            return new GetImageReply {
                ImageData = ByteString.CopyFrom (image.image),
                ImageType = image.mime
            };
        }
    }
}
