using ArbitraryPixel.Common;
using ArbitraryPixel.Platform2D.Logging;
using System;
using System.Text;

namespace ArbitraryPixel.CodeLogic.Common
{
    // TODO: At some point in the future, replace this with something that logs to the file system.
    public class ConsoleLogger : ILogger
    {
        private IDateTimeFactory _dateTimeFactory;
        private IDebug _debug;

        public ConsoleLogger(IDateTimeFactory dateTimeFactory, IDebug debug)
        {
            _dateTimeFactory = dateTimeFactory ?? throw new ArgumentNullException();
            _debug = debug ?? throw new ArgumentNullException();
        }

        public bool UseTimeStamps { get; set; }

        private string GetPrefix()
        {
            StringBuilder prefix = new StringBuilder();

            prefix.Append("CLDBG: ");

            if (this.UseTimeStamps)
            {
                prefix.Append("[");
                prefix.Append(_dateTimeFactory.Now.ToString("yyyy.MM.dd @ HH.mm.ss.fff"));
                prefix.Append("] ");
            }

            return prefix.ToString();
        }

        public void WriteLine(string message)
        {
            _debug.WriteLine(this.GetPrefix() + message);
        }
    }
}
