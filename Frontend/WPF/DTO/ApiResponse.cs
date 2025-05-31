namespace WpfApp.DTO {
    public class ApiResponse<T> {
        public string? message {
            get; set;
        }
        public T? body {
            get; set;
        }
    }
}
