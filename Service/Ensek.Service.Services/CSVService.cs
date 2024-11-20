using System.Collections.Generic;
using System.Globalization;
using System.IO;

using CsvHelper;
using CsvHelper.Configuration;

namespace Ensek.Service.Services;

public class CSVService : ICSVService
{
    // taken from https://www.syncfusion.com/blogs/post/handling-csv-files-in-asp-net-core-web-apis
    public IAsyncEnumerable<T> ReadCSVAsync<T>(Stream file)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            IgnoreBlankLines = true,
            ExceptionMessagesContainRawData = true,
            BadDataFound = null, // Ignore Bad data
            /* Add any conversion exceptions that occur while the CSV Reader
             * parses the file. Fluent Validation will handle other
             * validations.
             */
            ReadingExceptionOccurred = x =>
            {
                // Catch any conversion/null errors here and create
                // a new RecordValidationResult for each and add it
                // to the errorsList.
                return false;
            }
        };
        var reader = new StreamReader(file);
        var csv = new CsvReader(reader, config);
        return csv.GetRecordsAsync<T>();
    }
}