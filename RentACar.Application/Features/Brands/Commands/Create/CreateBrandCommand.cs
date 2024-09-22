using AutoMapper;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Transaction;
using MediatR;
using RentACar.Application.Services.Repositories;
using RentACar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Application.Features.Brands.Commands.Create;

public class CreateBrandCommand : IRequest<CreateBrandCommandResponse>, ITransactionalRequest, ICacheRemoverRequest,ILoggableRequest
{
    public string Name { get; set; }

    public string? CacheKey => string.Empty;

    public string? CacheGroupKey => "GetBrands";

    public bool BypassCache => false;

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreateBrandCommandResponse>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;


        public CreateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<CreateBrandCommandResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            Brand brand = _mapper.Map<Brand>(request);
            brand.Id = Guid.NewGuid();
            await _brandRepository.AddAsync(brand);
            return _mapper.Map<CreateBrandCommandResponse>(brand);
        }
    }
}


