using AutoMapper;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Paging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RentACar.Application.Services.Repositories;
using RentACar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Application.Features.Models.Queries.GetList;

public class GetListModelQuery : IRequest<GetListResponse<GetListModelListItemDto>>
{
    public PageRequest PageRequest { get; set; }


    public class GetListModelQueryHandler : IRequestHandler<GetListModelQuery, GetListResponse<GetListModelListItemDto>>
    {

        private readonly IMapper _mapper;
        private readonly IModelRepository _repository;

        public GetListModelQueryHandler(IModelRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListModelListItemDto>> Handle(GetListModelQuery request, CancellationToken cancellationToken)
        {
            Paginate<Model> models = await _repository.GetListAsync(index:request.PageRequest.PageIndex,size:request.PageRequest.PageSize,
                include:x => x.Include(i => i.Transmission).Include(i => i.Brand));

            return _mapper.Map<GetListResponse<GetListModelListItemDto>>(models);
        }
    }
}

