﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShoppingCart.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {
        public int Id { get; set; }
        [Required]
        public int ShoppingCartId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public Book Book { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}