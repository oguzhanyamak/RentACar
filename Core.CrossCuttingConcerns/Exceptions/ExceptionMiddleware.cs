using Core.CrossCuttingConcerns.Exceptions.Handlers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HttpExceptionHandler _exceptionHandler;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _exceptionHandler = new();
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex) {
            await HandleExceptionAsync(context.Response,ex);
        }
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
