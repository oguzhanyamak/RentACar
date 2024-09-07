using Core.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using RentACar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Persistence.Contexts;

public class BaseDbContext:DbContext
{

    protected IConfiguration _configuration;
    public DbSet<Brand> Brands { get; set; }



    public BaseDbContext(DbContextOptions dbContextOptions,IConfiguration configuration):base(dbContextOptions)
    {
        _configuration = configuration;
        Database.EnsureCreated(); // Database.EnsureCreated() ile veritabanı henüz yoksa veritabanının oluşturulmasını sağlar.
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}
