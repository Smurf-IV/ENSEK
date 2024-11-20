using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Ensek.DataAccess.Design;
using Ensek.Database.Tables;
using Ensek.Dto.Common;

using Microsoft.Extensions.Logging;

namespace Ensek.Service.Services;

public class MeterReadingService : IMeterReadingService
{
    private readonly ILogger<MeterReadingService> _logger;
    private readonly ICSVService _csvService;
    private readonly IAccountReader _accountReader;
    private readonly IMeterReadingReader _meterReadingReader;
    private readonly IMeterReadingWriter _meterReadingWriter;
    private readonly IFailedMeterReadingWriter _failedMeterReadingWriter;

    public MeterReadingService(ILogger<MeterReadingService> logger,
        ICSVService csvService, // TODO: As the number of DI items is large,I would have flipped to using delayed GetService<> as needed
        IAccountReader accountReader,
        IMeterReadingReader meterReadingReader,
        IMeterReadingWriter meterReadingWriter,
        IFailedMeterReadingWriter failedMeterReadingWriter)
    {
        _logger = logger;
        _csvService = csvService;
        _accountReader = accountReader;
        _meterReadingReader = meterReadingReader;
        _meterReadingWriter = meterReadingWriter;
        _failedMeterReadingWriter = failedMeterReadingWriter;
    }

    public async Task<ImportResultDto> ImportAsync(long tsId, Stream file,
        CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("{TsId} ImportAsync IN", tsId);
        var results = new ImportResultDto
        {
            TsId = tsId
        };
        try
        {
            // TODO: CSVHelper does not return an invalid line if it throws an exception
            //await foreach (MeterReadingDto line in _csvService.ReadCSVAsync<MeterReadingDto, MeterReadingDto>(file)
            //                   .WithCancellation(cancellationToken))
            var reader = new StreamReader(file);

            // ReSharper disable once RedundantAssignment
            var csvLine = await reader.ReadLineAsync(cancellationToken); // Skip Header
            while (!reader.EndOfStream)
            {
                csvLine = await reader.ReadLineAsync(cancellationToken);
                if (string.IsNullOrWhiteSpace(csvLine))
                {
                    continue;
                }
                var bits = csvLine.Split(',', StringSplitOptions.TrimEntries);
                var line = new MeterReadingDto
                {
                    AccountId = -1,
                    MeterReadingDateTime = bits[1],
                    MeterReadValue = -1
                };
                if (int.TryParse(bits[0], out var accountId))
                {
                    line.AccountId = accountId;
                }
                if (int.TryParse(bits[2], out var meterReadValue))
                {
                    line.MeterReadValue = meterReadValue;
                }

                Account? exists = await _accountReader.GetAccountAsync(line.AccountId);
                if (exists is null)
                {
                    results.AddError(line, "Failed to retrieve Account");
                    await _failedMeterReadingWriter.CreateFailedMeterReadingAsync(tsId, line);
                    continue;
                }
                // TODO: Add resolver / Checker codeblock!
                if (line.MeterReadValue.GetValueOrDefault(-1) < 0)
                {
                    results.AddError(line, "Failed value to range Check");
                    await _failedMeterReadingWriter.CreateFailedMeterReadingAsync(tsId, line);
                    continue;
                }
                // `dd/MM/yyyy hh:mm` enUk 24hour UTC zero padding!
                if (!DateTime.TryParseExact(line.MeterReadingDateTime, "dd/MM/yyyy hh:mm", new CultureInfo("en-UK"),
                        DateTimeStyles.AssumeUniversal, out DateTime dateTime))
                {
                    results.AddError(line, "Failed DateTime conversion");
                    await _failedMeterReadingWriter.CreateFailedMeterReadingAsync(tsId, line);
                    continue;
                }

                MeterReading? lastReadAccount = await _meterReadingReader.GetLastReadAsync(line.AccountId);
                if (lastReadAccount is not null
                    && lastReadAccount.ReadingDateTimeUTC >= dateTime)
                {
                    results.AddError(line, "Failed This time is less than a last");
                    await _failedMeterReadingWriter.CreateFailedMeterReadingAsync(tsId, line);
                    continue;
                }

                // All good: so lets see if it can be added to the readings table
                await _meterReadingWriter.CreateMeterReadingAsync(line, dateTime);
                results.AddSuccess();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{TsId} ImportAsync FAILED", tsId);
            results.AddError(null, ex.Message);
        }
        finally
        {
            _logger.LogTrace("{TsId} ImportAsync OUT", tsId);
        }
        return results;
    }
}