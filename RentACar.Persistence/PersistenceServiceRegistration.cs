﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentACar.Application.Services.Repositories;
using RentACar.Persistence.Contexts;
using RentACar.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<BaseDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("NPSQL")));
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IModelRepository,ModelRepository>();
        return services;
    }
}
