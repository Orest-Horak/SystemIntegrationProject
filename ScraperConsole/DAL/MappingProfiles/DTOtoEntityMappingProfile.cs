using AutoMapper;
using DTO;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.MappingProfiles
{
    public class DTOtoEntityMappingProfile : Profile
    {
        public DTOtoEntityMappingProfile()
        {
            CreateMap<CommentDTO, CommentEntity>();
            CreateMap<NewsDTO, NewsEntity>();
//CreateMap<NewsDTO, NewsEntity>()
//    .ForMember(x => x.InternalId, opt => opt.MapFrom(source => source.Id))
/*                .ForMember(x => x.ExternalId, opt => opt.MapFrom(source => source.ID))*/;
            /*.ForMember(x => x.Comments,)*/
            //CreateMap<Address, AddressViewModel>()
            //    .ForMember(x => x.PersonAddressLineOne, opt => opt.MapFrom(source => source.FirstLine))
            //    .ForMember(x => x.PersonCountryOfResidence, opt => opt.MapFrom(source => source.Country));
        }
        public override string ProfileName
        {
            get { return "DTOtoEntityMappings"; }
        }

    }
}
