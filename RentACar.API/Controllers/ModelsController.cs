using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentACar.Application.Features.Brands.Queries.GetList;
using RentACar.Application.Features.Models.Queries.GetList;
using RentACar.Application.Features.Models.Queries.GetListByDynamic;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : BaseController
    {

        [HttpGet("list")]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListModelQuery query = new GetListModelQuery() { PageRequest = pageRequest };
            GetListResponse<GetListModelListItemDto> response = await Mediator.Send(query);
            return Ok(response);
        }
        [HttpPost("listDynamic")]
        public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest,[FromBody]DynamicQuery? dquery = null)
        {
            GetListByDynamicModelQuery query = new GetListByDynamicModelQuery() { PageRequest = pageRequest,DynamicQuery=dquery };
            GetListResponse<GetListByDynamicModelListItemDto> response = await Mediator.Send(query);
            return Ok(response);
        }

    }
}
