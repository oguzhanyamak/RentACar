using Core.CrossCuttingConcerns.Exceptions.Extensions;
using Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationProblemDetails = Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails.ValidationProblemDetails;

namespace Core.CrossCuttingConcerns.Exceptions.Handlers;

public class HttpExceptionHandler : ExceptionHandler
{
    private HttpResponse? _response;
    public HttpResponse Response
    {
        get => _response ?? throw new ArgumentNullException(nameof(_response));
        set => _response = value;
    }
    protected override Task HandleException(BusinessException businessException)
    {
        Response.StatusCode = StatusCodes.Status400BadRequest;
        string details = new BusinessProblemDetails(businessException.Message).AsJson();
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(Exception exception)
    {
        Response.StatusCode = StatusCodes.Status500InternalServerError;
        string details = new InternalServerErrorProblemDetails(exception.Message).AsJson();
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(ValidationException validationException)
    {
        Response.StatusCode = StatusCodes.Status400BadRequest;
        string details = new ValidationProblemDetails(validationException.Errors).AsJson();
        return Response.WriteAsync(details);
    }
}

// H-3) Fırlatılmış olan ValidationException global hata yakalama sırasında ele alınarak ValidationProblemDetail tipine dönüştürülüp dönüşen tip json'dan string'e dönüştürülür ve response edilir
/*
ExceptionHandler sınıfı, yakalanan istisnaların türüne göre hangi metotla ele alınacağını belirler.
HttpExceptionHandler ise, iş kurallarıyla ilgili olan (BusinessException) ve genel (Exception) hataları işleyerek uygun HTTP yanıtlarını döner.
İş kurallarına aykırı bir hata (BusinessException) yakalandığında, istemciye 400 (Bad Request) durumu ve hata detayları döndürülür.
Genel bir istisna (Exception) yakalandığında, istemciye 500 (Internal Server Error) durumu ve hata detayları döndürülür.
Bu yapı, uygulamada hataların merkezi bir noktada ele alınmasını sağlar ve istemciye daha kullanıcı dostu hata mesajları gönderilmesine olanak tanır.
 */
