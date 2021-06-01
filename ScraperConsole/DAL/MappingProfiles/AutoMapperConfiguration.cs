using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DAL.MappingProfiles
{

    //public static class AutoMapperConfiguration
    //{
    //    public static MapperConfiguration InitializeAutoMapper()
    //    {
    //        MapperConfiguration config = new MapperConfiguration(cfg =>
    //        {
    //            cfg.AddProfile(new WebMappingProfile());  //mapping between Web and Business layer objects
    //            cfg.AddProfile(new BLProfile());  // mapping between Business and DB layer objects
    //        });

    //        return config;
    //    }
    //}

    public static class MappingProfile
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AddProfile(new DTOtoEntityMappingProfile());
                cfg.AddProfile(new EntityToDTOMappingProfile());
            });

            return config;
        }
    }
}
