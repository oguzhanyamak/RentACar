using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Exceptions.Extensions;

public static class ProblemDetailsExceptions
{
    public static string AsJson<TProblemDetail>(this TProblemDetail details) where TProblemDetail : ProblemDetails => JsonSerializer.Serialize(details);
}

/*
ProblemDetails ve ondan türetilen sınıfların JSON formatına serileştirilmesini kolaylaştırır.
JsonSerializer.Serialize kullanılarak, details parametresi JSON formatında bir string'e dönüştürülür.
C#'ın genişletme metotları sayesinde, nesneleri doğrudan AsJson() metodunu çağırarak JSON formatında elde edebiliriz.
Bu sayede, hata yönetimi yapılırken kullanıcıya JSON formatında anlamlı bir hata yanıtı dönmek kolaylaşır ve daha standart hale gelir.
*/