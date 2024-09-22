using Core.CrossCuttingConcerns.Serilog.ConfigurationModels;
using Core.CrossCuttingConcerns.Serilog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Serilog.Loggers;

public class PostgreSqlLogger : LoggerServiceBase
{
    private readonly IConfiguration _configuration;

    public PostgreSqlLogger(IConfiguration configuration)
    {
        _configuration = configuration;

        PostgreSqlLogConfiguration logConfiguration = _configuration.GetSection("SerilogLogConfigurations:PostgreSqlLogConfiguration").Get<PostgreSqlLogConfiguration>() ?? throw new Exception(SerilogMessages.NullOptionsMessage);
        var loggerConfig = new LoggerConfiguration().WriteTo.PostgreSQL(logConfiguration.ConnectionString, logConfiguration.TableName, needAutoCreateTable: logConfiguration.AutoCreateSqlTable);

        if (!string.IsNullOrWhiteSpace(logConfiguration.Extras)) // Extras not null use seq
        {
            loggerConfig.WriteTo.Seq(logConfiguration.Extras);
        }
        _logger = loggerConfig.CreateLogger();
        
    }
}
// _logger = new LoggerConfiguration().WriteTo.PostgreSQL(logConfiguration.ConnectionString,logConfiguration.TableName,needAutoCreateTable:logConfiguration.AutoCreateSqlTable).WriteTo.Seq(logConfiguration.Extras).CreateLogger();

/*
Yapılandırma ve PostgreSQL: Bu sınıf, yapılandırma dosyasından PostgreSQL bağlantı ayarlarını (ConnectionString, TableName) alır ve logları PostgreSQL veritabanına yazar.
Otomatik Tablo Oluşturma: Yapılandırmaya göre, eğer tablo yoksa Serilog otomatik olarak logları kaydetmek için gerekli tabloyu oluşturabilir (AutoCreateSqlTable).
Seq Desteği (Opsiyonel): Yapılandırmadaki Extras değeri doluysa, loglar ayrıca Seq log sunucusuna da gönderilir.
Seq, logları merkezi bir yerde toplamak ve analiz etmek için kullanılan bir platformdur.
Hata Yönetimi: Eğer yapılandırma bilgileri eksikse, program bir hata fırlatır.
Bu sınıf, loglama işlemini PostgreSQL veritabanına kaydederken, ek bir opsiyon olarak Seq sunucusuna da logları gönderebilecek esneklikte tasarlanmıştır. 
*/
