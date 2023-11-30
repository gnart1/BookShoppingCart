using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;
namespace BookShoppingCart.Models
{
	[Table("BookDetail")]
	public class BookDetail
	{
		public int Id { get; set; }
		[Required]
		public int BookId { get; set; }
		[Required]
		public string NCC { get; set; }
		public string NXB { get; set; }
		public string BookCover { get; set; }
		public int Quantity { get; set; }
		[Required]
		public double Price { get; set; }

		public Book Book { get; set; }
	}
}
