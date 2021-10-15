using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductWebApp.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Price { get; set; }
        public string Pic { get; set; }

        public DateTime DateStamp { get; set; }
        
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
