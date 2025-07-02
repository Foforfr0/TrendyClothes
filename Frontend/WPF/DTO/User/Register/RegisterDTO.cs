using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.DTO.User.Register
{
    public class RegisterDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? AreaCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
    }
}
