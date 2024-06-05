using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace SupplyLink.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int TypeId { get; set; }

        [Required]
        public int UnitOfMeasureId { get; set; }

        [Required]
        public float Quantity { get; set; }

        public string? ImagePath { get; set; }

        public string Notes { get; set; }
        public DateTime OrderDate { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public string RequesterId { get; set; }

        // Method to set the RequesterId based on the current user
        public void SetRequesterId(UserManager<IdentityUser> userManager, ClaimsPrincipal user)
        {
            RequesterId = userManager.GetUserId(user);
        }
    }
}
