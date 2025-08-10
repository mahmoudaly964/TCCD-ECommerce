using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get;  set; } = Guid.NewGuid();
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get;  set; }

        public string? Description { get;  set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        [Required]
        public DateTime CreatedAt { get;  set; } = DateTime.UtcNow;
    }
}
