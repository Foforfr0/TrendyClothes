namespace Backend.DTO {
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

        private MessageResponse (bool IsError, string Message, T? DataRetrieved) {
            this.IsError = IsError;
            this.Message = Message;
            this.DataRetrieved = DataRetrieved;
        }

        public static MessageResponse<T> Success (string Message, T DataRetrieved) =>
            new MessageResponse<T> (false, Message, DataRetrieved);

        public static MessageResponse<T> Failure (string Message) =>
            new MessageResponse<T> (true, Message, default);
    }
}
