namespace Bookshop.SharedKernel.Application.Common
{
    public class Result
    {
        public required string Message { get; set; }
        public ResultStatus ResultStatus { get; set; }
    }

    public class Result<T> : Result
    {
        public required T Data { get; set; }
    }
}
