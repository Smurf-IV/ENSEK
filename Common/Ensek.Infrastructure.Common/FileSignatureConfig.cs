namespace Ensek.Infrastructure.Common;

public class FileSignatureConfig
{
    public ushort CsvMBytes { get; set; } = 3;
    public string CsvExt { get; set; } = @".csv";
    public string CsvType { get; set; } = @"Csv";
}