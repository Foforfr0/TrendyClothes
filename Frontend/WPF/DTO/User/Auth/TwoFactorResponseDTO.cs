using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.DTO.User.Auth
{
    public class TwoFactorResponseDTO
    {
        public string jwtToken { get; set; }
        public string role { get; set; }
    }
}
