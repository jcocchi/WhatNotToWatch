using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhatNotToWatch.Models;

namespace WhatNotToWatch.ViewModels
{
    public class BrowseReviewsViewModel
    {
        public IEnumerable<TVShow> Shows { get; set; }
        public string NameSortParam { get; set; }
        public string GenreSortParam { get; set; }
        public string RatingSortParam { get; set; }
        public string VoteSortParam { get; set; }
    }
}
