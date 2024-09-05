using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Application.Services.Repositories
{
    public interface IBrandRepository : IBrandRepository<Brand>,IAsyncRepository<Brand>
    {

    }
}
