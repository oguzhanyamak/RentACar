using Core.CrossCuttingConcerns.Exceptions.Handlers;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Serilog;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HttpExceptionHandler _exceptionHandler;
    private readonly LoggerServiceBase _loggerService;
    private readonly IHttpContextAccessor _contextAccessor;

    public ExceptionMiddleware(RequestDelegate next, LoggerServiceBase loggerService, IHttpContextAccessor contextAccessor)
    {
        _exceptionHandler = new();
        _next = next;
        _loggerService = loggerService;
        _contextAccessor = contextAccessor;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex) {
            await LogException(context,ex);
            await HandleExceptionAsync(context.Response,ex);

        }
    }

    private Task LogException(HttpContext context, Exception ex)
    {
        List<LogParameter> logParameters = new() { new LogParameter() {Type = context.GetType().Name,Value=ex.ToString() } };
        LogDetailWithException logDetailWithException = new() { ExceptionMesage = ex.ToString(),MethodName = _next.Method.Name, Parameters = logParameters, User = _contextAccessor.HttpContext?.User.Identity.Name ?? "?", };
        _loggerService.Error(JsonSerializer.Serialize(logDetailWithException));
        return Task.CompletedTask;
    }

    private Task HandleExceptionAsync(HttpResponse response,Exception exception)
    {
        response.ContentType = "application/json";
        _exceptionHandler.Response = response;
        return _exceptionHandler.HandleExceptionAsync(exception);
    }
}
/*
Invoke metodu çalıştığında, diğer middleware'lerin çalıştırılmasını sağlar.
Eğer bu süreçte bir hata (exception) meydana gelirse, catch bloğuna düşülür.
Hata yakalandığında, HandleExceptionAsync metodu çağrılır. Bu metot, hatayı JSON formatında istemciye döndürmek üzere yanıtın tipini ayarlar ve hatayı işlemeye devam eder.
*/
