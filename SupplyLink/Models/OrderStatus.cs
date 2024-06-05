using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplyLink.Models
{
    public class OrderStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
