using System.ComponentModel.DataAnnotations;

namespace BookShoppingCart.Areas.Admin.Models
{
    public class GenreViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(32)]
        public string GenreName { get; set; }
    }
}
