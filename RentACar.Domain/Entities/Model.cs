using Core.Persistence.Repositories;
using RentACar.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Domain.Entities;

public class Model : Entity<Guid>
{
    public Model()
    {
        Cars = new HashSet<Car>();
    }
    public Model(Guid id,Guid brandId,Guid transmissionId,CarFuel fuel,string name,decimal dailyPrice,string imageUrl):this()
    {
        Id = id;
        BrandId = brandId;
        TransmissionId = transmissionId;
        Fuel = fuel;
        Name = name;
        DailyPrice = dailyPrice;
        ImageUrl = imageUrl;
    }

    public Guid BrandId { get; set; }
    public Guid TransmissionId { get; set; }
    public string Name { get; set; }
    public decimal DailyPrice { get; set; }
    public string ImageUrl { get; set; }
    public CarFuel Fuel { get; set; }
    public virtual Brand? Brand { get; set; }
    public virtual Transmission? Transmission { get; set; }
    public virtual ICollection<Car> Cars { get; set; }
}

