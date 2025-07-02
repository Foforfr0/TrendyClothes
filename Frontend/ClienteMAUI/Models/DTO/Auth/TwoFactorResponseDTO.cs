using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClienteMAUI.Models.DTO.Auth
{
    public class TwoFactorResponseDTO
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = "";

        [JsonPropertyName("jwtToken")]
        public string JwtToken { get; set; } = "";
    }

}
