using System.ComponentModel.DataAnnotations;
using WhatNotToWatch.Models;

namespace WhatNotToWatch.ViewModels
{
    public class CreateShowViewModel
    {
        [Required]
        public string Name { get; set; }

        public Genre Genre { get; set; }

        [Required]
        [MaxLength(300)]
        public string Review { get; set; }
    }
}
