using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Cart
{
    public class CreateCartRequest
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
