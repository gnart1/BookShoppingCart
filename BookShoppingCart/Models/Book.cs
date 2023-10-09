using System.ComponentModel.DataAnnotations;

namespace BookShoppingCart.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(32)]
        public string BookName { get; set; }
        [MaxLength(128)]
    }
}
