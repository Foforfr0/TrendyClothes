using System.ComponentModel.DataAnnotations;

namespace WebPage.DTO.User.Auth {
    public class TwoFactorCodeDTO {
        public required string username {
            get; set;
        }
        public required string twoFactorCode {
            get; set;
        }
    }
}
