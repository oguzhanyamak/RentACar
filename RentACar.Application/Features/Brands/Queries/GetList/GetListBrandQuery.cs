using AutoMapper;
using Core.Application.Pipelines.Caching;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Paging;
using MediatR;
using RentACar.Application.Services.Repositories;
using RentACar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Application.Features.Brands.Queries.GetList
{
    public class GetListBrandQuery:IRequest<GetListResponse<GetListBrandListItemDto>>, ICachableRequest
    {
        public PageRequest pageRequest { get; set; }

        public string CacheKey => $"GetListBrandQuery({pageRequest.PageIndex},{pageRequest.PageSize})";

        public bool BypassCache { get; }

        public TimeSpan? SlidingExpiration { get; }

        public string? CacheGroupKey => "GetBrands";

        public class GetListBrandQueryHandler : IRequestHandler<GetListBrandQuery, GetListResponse<GetListBrandListItemDto>>
        {

            private readonly IBrandRepository _brandRespository;
            private readonly IMapper _mapper;

            public GetListBrandQueryHandler(IBrandRepository brandRespository, IMapper mapper)
            {
                _brandRespository = brandRespository;
                _mapper = mapper;
            }

            public async Task<GetListResponse<GetListBrandListItemDto>> Handle(GetListBrandQuery request, CancellationToken cancellationToken)
            {
                Paginate<Brand> brands = await _brandRespository.GetListAsync(index:request.pageRequest.PageIndex,size:request.pageRequest.PageSize);
                GetListResponse<GetListBrandListItemDto> response = _mapper.Map<GetListResponse<GetListBrandListItemDto>>(brands);
                return response;
        }
        }
    }
}
