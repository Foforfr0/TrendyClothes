using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMAUI.Models.DTO.Auth
{
    public class TwoFactorResponseDTO
    {
        public string JwtToken { get; set; } = string.Empty;
    }
}
