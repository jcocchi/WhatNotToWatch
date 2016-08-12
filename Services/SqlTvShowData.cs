using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhatNotToWatch.Models;

namespace WhatNotToWatch.Services
{
    public class SqlTvShowData : ITvShowData
    {
        private TVShowDbContext _context;

        public SqlTvShowData(TVShowDbContext context)
        {
            _context = context;
        }

        public void Add(TVShow newTvShow)
        {
            _context.Add(newTvShow);
            _context.SaveChanges();
        }

        public TVShow Get(int id)
        {
            return _context.TVShows.FirstOrDefault(t => t.ID == id);
        }

        public IEnumerable<TVShow> GetAll()
        {
            return _context.TVShows.ToList();
        }
 
        public int Update()
        {
            return _context.SaveChanges();
        }
    }
}
