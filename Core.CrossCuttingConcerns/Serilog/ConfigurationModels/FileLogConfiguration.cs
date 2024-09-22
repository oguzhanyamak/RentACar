using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Serilog.ConfigurationModels;

//Yapılandırma Konfigurasyon Dosyası

public class FileLogConfiguration
{
    public string Path { get; set; }
    public FileLogConfiguration()
    {
        Path = string.Empty;    
    }

    public FileLogConfiguration(string path)
    {
        Path=path;
    }

}
