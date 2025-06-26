using WebPage.Connections.REST;
using WebPage.Connections.gRPC;

namespace WebPage.Connections {
    public class ServicesConfig {
        public RestConfig REST {
            get; set;
        }
        public GrpcConfig gRPC {
            get; set;
        }
    }
}
