using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MunchrBackend.Models
{
    public interface ReviewModel
    {
        public int Id { get; set; }
        public string? Publisher { get; set; }
        public string? Content { get; set; }
        public int Rating { get; set; }
        public string? Buissness { get; set; }
        public string? Image {get; set;}
        public DateTime Date {get; set;}
        public bool IsPublished {get; set;}
        public bool IsDeleted {get; set;}
    }
}