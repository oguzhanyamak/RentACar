﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Application.Features.Brands.Queries.GetById;

public class GetByIdBrandResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
