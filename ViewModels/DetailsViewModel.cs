using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhatNotToWatch.Models;

namespace WhatNotToWatch.ViewModels
{
    public class DetailsViewModel
    {
        public string Name { get; set; }
        public Genre Genre { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
    }
}
