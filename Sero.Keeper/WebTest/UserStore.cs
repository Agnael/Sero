using Microsoft.EntityFrameworkCore;
using Sero.Keeper.Ef;
using Sero.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTest.Entities;

namespace WebTest
{
    public class UserStore : Store<Models.User, Entities.User>
    {
        protected override IQueryable<User> DbSet => base.DbSet.Include(x => x.NickNames);

        public UserStore(AppKeeper keeper, 
                            Mapper mapper) 
            : base(keeper, mapper)
        {
        }
    }
}
