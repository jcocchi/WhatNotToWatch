using Microsoft.EntityFrameworkCore;

namespace WhatNotToWatch.Models
{
    public class TVShowDbContext : DbContext
    {
        public TVShowDbContext(DbContextOptions<TVShowDbContext> options) : base(options) { }

        public DbSet<TVShow> TVShows { get; set; }
    }
}
