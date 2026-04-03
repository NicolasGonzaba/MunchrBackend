using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MunchrBackend.Models
{
    public class BusinessModel
    {
        [Key]
        public int BusinessId { get; set; }
        public string? BusinessName { get; set; }
        public string? BusinessHours { get; set; }
        public string? BusinessPhoneNumber { get; set; }
        public string? BusinessDescription { get; set; }
        public string? Category { get; set; }
        public string? StreetName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int ZipCode { get; set; }
        public ICollection<ReviewModel> BusinessReviews { get; set;} = [];
    }
}