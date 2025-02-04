﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RetailWeb_RazorPage.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Dispaly Order must be between 1-100")]
        public int DisplayOrder { get; set; }
    }
}
