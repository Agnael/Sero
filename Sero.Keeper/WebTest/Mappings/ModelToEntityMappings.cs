using Sero.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTest.Entities;

namespace WebTest.Mappings
{
    public class ModelToEntityMappings : IMappingSheet
    {
        public void MappingRegistration(MapperBuilder builder)
        {
            builder.CreateMap<Models.User, Entities.User>((src, dest) =>
            {
                dest.Id = src.Id;
                dest.Name = src.Name;

                if (src.Nicknames != null)
                {
                    dest.NickNames = src.Nicknames
                                        .Select(x => new UserNickName { Description = x })
                                        .ToList();
                }
            });
        }
    }
}
