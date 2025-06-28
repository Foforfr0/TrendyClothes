namespace WebPage.DTO.Product.Consult {
    public class CategoriesDTO {
        public required int id {
            get; set;
        }
        public required string category {
            get; set;
        }
    }

    public class TypesDTO {
        public required int id {
            get; set;
        }
        public required string type {
            get; set;
        }
    }

    public class StatussesDTO {
        public required int id {
            get; set;
        }
        public required string status {
            get; set;
        }
    }
}
