namespace Backend.DTO.Product {
    public class CategoriesDTO {
        public required int Id {
            get; set;
        }
        public required string Category {
            get; set;
        }
    }

    public class TypesDTO {
        public required int Id {
            get; set;
        }
        public required string Type {
            get; set;
        }
    }

    public class StatussesDTO {
        public required int Id {
            get; set;
        }
        public required string Status {
            get; set;
        }
    }
}
