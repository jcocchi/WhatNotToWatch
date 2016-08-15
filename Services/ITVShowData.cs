using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhatNotToWatch.Models;

namespace WhatNotToWatch.Services
{
    public interface ITvShowData
    {
        IEnumerable<TVShow> GetAll();
        TVShow Get(int id);
        void Add(TVShow newTvShow);
        void Delete(TVShow showToDelete);
        int Update();
    }

}
