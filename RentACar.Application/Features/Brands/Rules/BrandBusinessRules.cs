using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using RentACar.Application.Features.Brands.Constants;
using RentACar.Application.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Application.Features.Brands.Rules;

public class BrandBusinessRules :BaseBusinessRules
{
    private readonly IBrandRepository _brandRepository;

    public BrandBusinessRules(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }


    public async Task BrandNameCannotBeDuplicatedWhenInsertedAsync(string brandName)
    {
        if (await _brandRepository.AnyAsync(x => x.Name.ToLower() == brandName.ToLower()))
        {
            throw new BusinessException(BrandsMessages.BrandNameExists);
        }
    }
}
