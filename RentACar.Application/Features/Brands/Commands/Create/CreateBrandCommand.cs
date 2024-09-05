using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Application.Features.Brands.Commands.Create;

public class CreateBrandCommand  : IRequest<CreateBrandCommandResponse>
{
    public string Name { get; set; }

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreateBrandCommandResponse>
    {
        public async Task<CreateBrandCommandResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            return new() { Id = new Guid(), Name = request.Name };
        }
    }
}


