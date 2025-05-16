namespace UI.DTO {
    public class ApiResponse<T> {
        public string? message {
            get; set;
        }
        public ApiBody<T>? body {
            get; set;
        }
    }
}
