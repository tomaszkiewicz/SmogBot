using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Azure.WebJobs.Host;

namespace SmogBot.Notifier
{
    public class ConsoleTextWriter : TraceWriter, IDisposable
    {
        private readonly TextWriter _writer;

        public ConsoleTextWriter(TextWriter writer, TraceLevel level) : base(level)
        {
            _writer = writer;
        }

        public override void Trace(TraceEvent traceEvent)
        {
            _writer.WriteLine(traceEvent.Message);
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}