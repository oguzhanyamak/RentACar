using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Application.Features.Brands.Commands.Create;

public class CreateBrandCommanValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandCommanValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MinimumLength(3);
    }
}
