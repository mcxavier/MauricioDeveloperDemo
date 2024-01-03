namespace Core.SharedKernel
{
    public class Error
    {
        public int Code { get; set; } = 0;
        public string Message { get; set; }

        public Error() { }

        public Error(string message)
        {
            this.Message = message;
        }

        public Error(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
    }
}