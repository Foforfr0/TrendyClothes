namespace AuctionAuctioneerService.Models {
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
    }
}
