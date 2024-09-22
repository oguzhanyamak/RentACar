using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Serilog.ConfigurationModels;
//Yapılandırma Konfigurasyon Dosyası
public class PostgreSqlLogConfiguration 
{
    public string ConnectionString { get; set; }
    public string TableName { get; set; }
    public bool AutoCreateSqlTable { get; set; }
    public string? Extras { get; set; } //use with seq

    public PostgreSqlLogConfiguration()
    {
        ConnectionString = string.Empty;
        TableName = string.Empty;
        Extras = string.Empty;
    }

    public PostgreSqlLogConfiguration(string connectionString,string tableName,bool autoCreateTable,string extras)
    {
        ConnectionString = connectionString;
        TableName = tableName;
        AutoCreateSqlTable = autoCreateTable;
        Extras = extras;
    }
}
