namespace UI.DTO {
    public class ApiBody<T> {
        public object? contentType {
            get; set;
        }
        public object? serializerSettings {
            get; set;
        }
        public object? statusCode {
            get; set;
        }
        public T? value {
            get; set;
        }
    }
}
