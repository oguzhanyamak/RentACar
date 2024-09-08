using Core.Persistence.Repositories;
using RentACar.Domain.Enums;

namespace RentACar.Domain.Entities;

public class Car : Entity<Guid>
{
    public Guid ModelId { get; set; }
    public int Kilometer { get; set; }
    public short ModelYear { get; set; }
    public string Plate { get; set; }
    public CarState State { get; set; }
    public virtual Model? Model { get; set; }

    public Car(Guid id,Guid modelId, int kilometer,string plate,CarState state ):this()
    {
        Id = id;
        ModelId = modelId;
        Kilometer = kilometer;
        Plate= plate;
        State = state;
    }

    public Car()
    {

    }


}

