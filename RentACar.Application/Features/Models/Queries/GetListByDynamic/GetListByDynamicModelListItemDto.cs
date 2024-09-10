using RentACar.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Application.Features.Models.Queries.GetListByDynamic;

public class GetListByDynamicModelListItemDto
{
    public Guid Id { get; set; }
    public string BrandName { get; set; }
    public CarFuel FuelName { get; set; }
    public string TransmissionName { get; set; }
    public string Name { get; set; }
    public decimal DailyPrice { get; set; }
    public string ImageUrl { get; set; }
}
