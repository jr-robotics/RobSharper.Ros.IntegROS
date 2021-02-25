using Xunit.Sdk;

namespace IntegROS.XunitExtensions
{
    /// <summary>
    /// Diagnostics messages which outputs message in call to ToString()
    /// </summary>
    public class PrintableDiagnosticMessage : DiagnosticMessage
    {
        public PrintableDiagnosticMessage()
        {
        }

        public PrintableDiagnosticMessage(string message) : base(message)
        {
        }

        public PrintableDiagnosticMessage(string format, params object[] args) : base(format, args)
        {
        }

        public override string ToString()
        {
            return Message ?? base.ToString();
        }
    }
}