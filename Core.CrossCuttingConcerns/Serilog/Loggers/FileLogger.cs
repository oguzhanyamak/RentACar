using Core.CrossCuttingConcerns.Serilog.ConfigurationModels;
using Core.CrossCuttingConcerns.Serilog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Serilog.Logger;

public class FileLogger : LoggerServiceBase
{
    private readonly IConfiguration _configuration;

    public FileLogger(IConfiguration configuration)
    {
        _configuration = configuration;

        FileLogConfiguration logConfiguration = _configuration.GetSection("SerilogLogConfigurations:FileLogConfiguration").Get<FileLogConfiguration>()?? throw new Exception(SerilogMessages.NullOptionsMessage);

        string logFilePath = string.Format(format: "{0}{1}", arg0: Directory.GetCurrentDirectory() + logConfiguration.Path, arg1: ".txt");

        _logger = new LoggerConfiguration().WriteTo.File(logFilePath ,rollingInterval:RollingInterval.Day,retainedFileCountLimit:null, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}"
            ).CreateLogger();
    }  

}

/*
Bu sınıf, yapılandırma dosyasından log dosyasının yolunu ve diğer ayarları alır.
Ardından Serilog kütüphanesini kullanarak her gün yeni bir log dosyası oluşturan ve belirli bir formatta (zaman damgası, log seviyesi, mesaj vb.) log yazan bir logger nesnesi oluşturur.
Eğer yapılandırma dosyasındaki bilgiler eksikse bir hata fırlatır.
*/
