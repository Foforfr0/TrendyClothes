using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMAUI.Models.DTO.Auth
{
    public class CodeTwoFactorDTO
    {
        public string Username { get; set; } = string.Empty;
        public string TwoFactorCode { get; set; } = string.Empty;
    }
}
