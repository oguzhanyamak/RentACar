using AutoMapper;
using MediatR;
using RentACar.Application.Features.Brands.Commands.Update;
using RentACar.Application.Services.Repositories;
using RentACar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Application.Features.Brands.Commands.Delete
{
    public class DeleteBrandCommand :IRequest<DeletedBrandResponse>
    {
        public Guid Id { get; set; }



        public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, DeletedBrandResponse>
        {
            private readonly IBrandRepository _brandRepository;
            private readonly IMapper _mapper;

            public DeleteBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper)
            {
                _brandRepository = brandRepository;
                _mapper = mapper;
            }

            public async Task<DeletedBrandResponse> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
            {
                if (await _brandRepository.AnyAsync(x => x.Id == request.Id))
                {
                    Brand brand = await _brandRepository.GetAsync(x => x.Id == request.Id);
                    await _brandRepository.DeleteAsync(brand);
                    return _mapper.Map<DeletedBrandResponse>(brand);

                }
                return new() { };
            }
        }
    }
}
