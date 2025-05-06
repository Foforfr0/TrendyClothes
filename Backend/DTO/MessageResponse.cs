namespace Backend.DTO {
    public class MessageResponse<T> {
        public bool isError {
            get; set;
        }

        public string message {
            get; set;
        }

        public T? dataRetrieved {
            get; set;
        }

        public MessageResponse () {
            
        }

        public MessageResponse (bool IsError, string Message, T? DataRetrieved) {
            this.isError = IsError;
            this.message = Message;
            this.dataRetrieved = DataRetrieved;
        }

        public static MessageResponse<T> Success (string message, T? dataRetrieved) =>
            new MessageResponse<T> (false, message, dataRetrieved);

        public static MessageResponse<T> Failure (string message) =>
            new MessageResponse<T> (true, message, default);
    }
}
