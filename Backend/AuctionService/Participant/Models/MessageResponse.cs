/*using AuctionParticipantService.Models.Consult;

namespace AuctionParticipantService.Models {
    public class MessageResponse<T> {
        public bool IsError {
            get; set;
        }

        public string Message {
            get; set;
        }

        public T? DataRetrieved {
            get; set;
        }

        public MessageResponse (bool isError, string message, T? dataRetrieved) {
            this.IsError = isError;
            this.Message = message;
             this.DataRetrieved = dataRetrieved;
        }

        public static MessageResponse<T> Success (string message, T? dataRetrieved) =>
            new MessageResponse<T> (false, message, dataRetrieved);

        public static MessageResponse<T> Failure (string message) =>
            new MessageResponse<T> (true, message, default);

        internal class Success : MessageResponse<List<AuctionDTO>>
        {
            private string v;
            private List<AuctionDTO> auctions;

            public Success(string v, List<AuctionDTO> auctions)
            {
                this.v = v;
                this.auctions = auctions;
            }
        }

        internal class Failure : MessageResponse<List<AuctionDTO>>
        {
            private string v;

            public Failure(string v)
            {
                this.v = v;
            }
        }
    }
}
*/