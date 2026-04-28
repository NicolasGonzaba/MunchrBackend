using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MunchrBackend.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Buissness { get; set; }
        public string? ProfilePic { get; set; }
        public string? Salt { get; set; }
        public string? Hash { get; set; }
    }
}