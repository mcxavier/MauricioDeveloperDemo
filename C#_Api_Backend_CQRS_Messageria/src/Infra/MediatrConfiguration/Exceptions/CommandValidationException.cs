namespace Infra.QueryCommands._Kernel.Exceptions
{
    public class CommandValidationException : System.Exception
    {
        public string[] ValidationErrors { get; set; }

        public CommandValidationException() { }

        public CommandValidationException(string message) : base(message) { }

        public CommandValidationException(string message, string[] validationErrors) : base(message)
        {
            ValidationErrors = validationErrors;
        }
    }
}