using Sero.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTest.Mappings
{
    public class EntityToModelMappings : IMappingSheet
    {
        public void MappingRegistration(MapperBuilder builder)
        {
            builder.CreateMap<Entities.User, Models.User>((src, dest) =>
            {
                dest.Id = src.Id;
                dest.Name = src.Name;

                if (src.NickNames != null)
                    dest.Nicknames = src.NickNames.Select(x => x.Description).ToList();
            });
        }
    }
}
