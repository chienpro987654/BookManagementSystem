using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BookManagementSystem.Models
{
    public class User : IdentityUser
    {
        [Display(Name = "Full Name")]
        [AllowNull]
        public string? FullName { get; set; } = string.Empty;
        public int Gender { get; set; } = 0;
        [AllowNull]
        public string? AvatarUri { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public int CartId { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public static IEnumerable<SelectListItem> getGenderList()
        {
            yield return new SelectListItem { Text = "Không xác định", Value = "0", Selected = true };
            yield return new SelectListItem { Text = "Nam", Value = "1" };
            yield return new SelectListItem { Text = "Nữ", Value = "2" };
        }
    }
}