using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace Ensek.Library.Tests;

// TODO: Move to Test Util project
[ExcludeFromCodeCoverage]
public static class TestLogger
{
    public static ILogger<T> Create<T>()
    {
        var logger = new NUnitLogger<T>();
        return logger;
    }

    private class NUnitLogger<T> : ILogger<T>, IDisposable
    {
        private readonly Action<string> output = Console.WriteLine;

        public NUnitLogger()
        {
        }

        public void Dispose()
        {
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter) => output(formatter(state, exception));

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => this;
    }
}