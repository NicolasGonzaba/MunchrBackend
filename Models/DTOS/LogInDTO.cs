using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MunchrBackend.Models.DTOS
{
    public class LogInDTO
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}