﻿using System.ComponentModel.DataAnnotations;

namespace BookShoppingCart.Models
{
    public class Book
    {
        
        public int Id { get; set; }
        [Required]
        [MaxLength(32)]
        public string? BookName { get; set; }
        [Required]
        public int GenreId { get; set; }
        public Genre Genre;
    }
}
