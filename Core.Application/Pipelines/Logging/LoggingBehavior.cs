using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Serilog;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Application.Pipelines.Logging
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ILoggableRequest
    {

        private readonly IHttpContextAccessor _httpContextAccessor; // ASP.NET Core'da HTTP bağlamına (HttpContext) erişimi sağlar. Bu, isteği yapan kullanıcının kimlik bilgilerini alabilmek için gereklidir.
        private readonly LoggerServiceBase _loggerService;

        public LoggingBehavior(LoggerServiceBase loggerService, IHttpContextAccessor httpContextAccessor)
        {
            _loggerService = loggerService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            List<LogParameter> parameters = new() { new LogParameter { Type = request.GetType().Name,Value = request} };
            LogDetail logDetail = new LogDetail() {  MethodName= next.Method.Name, Parameters = parameters,User = _httpContextAccessor.HttpContext.User.Identity?.Name ?? "?"};
            _loggerService.Info(JsonSerializer.Serialize(logDetail));
            return await next();
        }
    }
}

/*
Bu sınıf, MediateR pipeline'ında bir davranış (behavior) olarak çalışır ve her istek işlendiğinde istekle ilgili bilgileri (istek tipi, parametreler, işlemi yapan kullanıcı) loglar.
Loglama işlemi, yapılandırılan log servisi (LoggerServiceBase) aracılığıyla yapılır.
Ayrıca, HTTP isteği yapan kullanıcının kimlik bilgileri de loglanır.
İstek işlenmeye devam ettirilmeden önce loglanır ve ardından next() çağrısı ile asıl işleyici çalıştırılır. 
*/
