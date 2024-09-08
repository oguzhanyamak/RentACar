using AutoMapper;
using Core.Application.Responses;
using Core.Persistence.Paging;
using RentACar.Application.Features.Brands.Commands.Create;
using RentACar.Application.Features.Brands.Commands.Delete;
using RentACar.Application.Features.Brands.Commands.Update;
using RentACar.Application.Features.Brands.Queries.GetById;
using RentACar.Application.Features.Brands.Queries.GetList;
using RentACar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Application.Features.Brands.Profiles
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            CreateMap<Brand, CreateBrandCommandResponse>().ReverseMap();
            CreateMap<Brand,CreateBrandCommand>().ReverseMap();

            CreateMap<Brand, GetListBrandListItemDto>().ReverseMap();
            CreateMap<Paginate<Brand>, GetListResponse<GetListBrandListItemDto>>().ReverseMap();

            CreateMap<Brand,GetByIdBrandResponse>().ReverseMap();

            CreateMap<Brand, UpdateBrandResponse>().ReverseMap();

            CreateMap<Brand, DeletedBrandResponse>().ReverseMap();
        }
    }
}
