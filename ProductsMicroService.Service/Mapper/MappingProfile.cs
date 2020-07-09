using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ProductsMicroService.Data.Models;

namespace ProductsMicroService.Service
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Audit, Audit>()
            //   .ForMember(v => v.Created, MappingOptions => MappingOptions.Ignore())
            //  .ForMember(v => v.CreatedBy, opt => opt.Ignore())
            //  .ForMember(v => v.Modified, opt => opt.Ignore())
            //  .ForMember(v => v.ModifiedBy, opt => opt.Ignore());
            
        }
    }
}
