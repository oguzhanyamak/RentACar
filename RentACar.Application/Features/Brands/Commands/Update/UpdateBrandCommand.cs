using AutoMapper;
using MediatR;
using RentACar.Application.Services.Repositories;
using RentACar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Application.Features.Brands.Commands.Update
{
    public class UpdateBrandCommand:IRequest<UpdateBrandResponse>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, UpdateBrandResponse>
        {
            private readonly IBrandRepository _brandRepository;
            private readonly IMapper _mapper;

            public UpdateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper)
            {
                _brandRepository = brandRepository;
                _mapper = mapper;
            }

            

            public async Task<UpdateBrandResponse> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
            {
                if(await _brandRepository.AnyAsync(x => x.Id == request.Id))
                {
                    Brand brand = await _brandRepository.GetAsync(x => x.Id == request.Id);
                    brand.Name = request.Name;
                    await _brandRepository.UpdateAsync(brand);
                    return _mapper.Map<UpdateBrandResponse>(brand);
                    
                }
                return new() { };
            }
        }
    }
}
