using AutoMapper;
using DTO;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace DAL.MappingProfiles
{
    public class EntityToDTOMappingProfile : Profile
    {
        public EntityToDTOMappingProfile()
        {
            CreateMap<CommentEntity, CommentDTO>();
            CreateMap<NewsEntity, NewsDTO>();
//CreateMap<NewsEntity, NewsDTO>()
//    .ForMember(x => x.Id, opt => opt.MapFrom(source => source.InternalId))
/*                .ForMember(x => x.ID, opt => opt.MapFrom(source => source.ExternalId))*/;
            //Mapper.CreateMap<AddressViewModel, Address>()
            //    .ForMember(x => x.FirstLine, opt => opt.MapFrom(source => source.PersonAddressLineOne))
            //    .ForMember(x => x.Country, opt => opt.MapFrom(source => source.PersonCountryOfResidence));
        }

        public override string ProfileName
        {
            get { return "EntityToDTOMappings"; }
        }

    }
}
