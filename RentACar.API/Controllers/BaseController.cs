﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RentACar.API.Controllers
{
    public class BaseController:ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator? Mediator => _mediator??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
