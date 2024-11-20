using System.Collections.Generic;
using System.Globalization;
using System.IO;

using CsvHelper;

namespace Ensek.Service.Services;

public class CSVService : ICSVService
{
    // taken from https://www.syncfusion.com/blogs/post/handling-csv-files-in-asp-net-core-web-apis
    public IAsyncEnumerable<T> ReadCSVAsync<T>(Stream file)
    {
        var reader = new StreamReader(file);
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        return csv.GetRecordsAsync<T>();
    }
}