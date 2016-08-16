using System.ComponentModel.DataAnnotations;

namespace WhatNotToWatch.Models
{
    public enum Genre { None, Action, Comedy, Drama, Horror, Reality, SciFi };

    public class TVShow
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Show Name")]
        public string Name { get; set; }

        public Genre Genre { get; set; }

        [Display(Name = "Show Rating"), Range(0,10)]
        public int Rating { get; set; }

        [Required]
        [MaxLength(300)]
        public string Review { get; set; }

        [Range(-5, int.MaxValue)]
        public int Vote { get; set; }
    }

}
