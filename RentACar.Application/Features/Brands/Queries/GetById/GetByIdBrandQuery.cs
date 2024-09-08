using AutoMapper;
using MediatR;
using RentACar.Application.Services.Repositories;
using RentACar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Application.Features.Brands.Queries.GetById
{
    public class GetByIdBrandQuery:IRequest<GetByIdBrandResponse>
    {
        public Guid Id { get; set; }
        public bool WithDeleted { get; set; } = false;

        public class GetByIdQueryHandler : IRequestHandler<GetByIdBrandQuery, GetByIdBrandResponse>
        {
            private readonly IMapper _mapper;
            private readonly IBrandRepository _repository;

            public GetByIdQueryHandler(IBrandRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<GetByIdBrandResponse> Handle(GetByIdBrandQuery request, CancellationToken cancellationToken)
            {
                return _mapper.Map<GetByIdBrandResponse>(await _repository.GetAsync(predicate:x => x.Id == request.Id,withDeleted:request.WithDeleted));
            }
        }
    }
}
