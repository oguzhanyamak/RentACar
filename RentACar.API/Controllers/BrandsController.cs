using Core.Application.Requests;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentACar.Application.Features.Brands.Commands.Create;
using RentACar.Application.Features.Brands.Commands.Delete;
using RentACar.Application.Features.Brands.Commands.Update;
using RentACar.Application.Features.Brands.Queries.GetById;
using RentACar.Application.Features.Brands.Queries.GetList;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBrandCommand createBrandCommand)
        {
            CreateBrandCommandResponse response = await Mediator.Send(createBrandCommand);
            return Ok(response);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListBrandQuery query = new GetListBrandQuery() { pageRequest = pageRequest };
            GetListResponse<GetListBrandListItemDto> response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] GetByIdBrandQuery query)
        {
            GetByIdBrandResponse response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBrandCommand updateBrandCommand)
        {
            UpdateBrandResponse response = await Mediator.Send(updateBrandCommand);
            return Ok(response);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] string Id)
        {
            DeletedBrandResponse response = await Mediator.Send(new DeleteBrandCommand() { Id = Guid.Parse(Id) });
            return Ok(response);
        }
    }
}
