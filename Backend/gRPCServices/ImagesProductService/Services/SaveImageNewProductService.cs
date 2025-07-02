using Grpc.Core;
using ImagesProductService.DAO;
using SaveImageNewProduct;

namespace ImagesProductService.Services {
    public class SaveNewImageServiceImpl : SaveNewImageService.SaveNewImageServiceBase {
        private readonly ImageProductDAO _saveImageDAO;

        public SaveNewImageServiceImpl (ImageProductDAO saveImageDAO) {
            _saveImageDAO = saveImageDAO;
        }

        public override async Task<SaveNewImageReply> SaveNewImage (SaveNewImageRequest request, ServerCallContext context) {
            if (request.ProductId <= 0) {
                return new SaveNewImageReply {
                    Message = "Id de producto no recibido o inválido.",
                    Success = false
                };
            }

            if (string.IsNullOrWhiteSpace (request.ImageBase64)) {
                return new SaveNewImageReply {
                    Message = "Imagen no recibida.",
                    Success = false
                };
            }

            if (string.IsNullOrWhiteSpace (request.MimeType)) {
                return new SaveNewImageReply {
                    Message = "Mime de imagen no recibida.",
                    Success = false
                };
            }

            try {
                byte[] imageBytes = Convert.FromBase64String (request.ImageBase64);
                bool saved = await _saveImageDAO.SaveNewImage (request.ProductId, imageBytes, request.MimeType);

                if (!saved) {
                    return new SaveNewImageReply {
                        Message = "No se pudo guardar la imagen.",
                        Success = false
                    };
                }

                return new SaveNewImageReply {
                    Message = "Imagen guardada correctamente.",
                    Success = true
                };
            } catch (FormatException) {
                return new SaveNewImageReply {
                    Message = "Formato de imagen inválido (Base64 incorrecto).",
                    Success = false
                };
            } catch (Exception ex) {
                Console.WriteLine ($"[ERROR SaveNewImage] {ex.Message}");
                throw new RpcException (new Status (StatusCode.Internal, "Error interno del servidor."));
            }
        }
    }
}
