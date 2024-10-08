﻿using Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Domain.Entities
{
    public class Brand : Entity<Guid>
    {
        public Brand()
        {
            Models = new HashSet<Model>();
        }
        public Brand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }
        public virtual ICollection<Model> Models { get; set; }


    }
}
