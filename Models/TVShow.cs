using System.ComponentModel.DataAnnotations;

namespace WhatNotToWatch.Models
{
    public enum Genre { None, Action, Comedy, Drama, Horror, Reality, SciFi };

    public class TVShow
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "TV Show Name")]
        public string Name { get; set; }

        public Genre Genre { get; set; }

        [Required]
        [Range(1,5)]
        public int Rating { get; set; }

        [MaxLength(300)]
        public string Review { get; set; }

        /*[Range(-5, int.MaxValue)]
        public int Vote { get; set; }*/
    }

}
