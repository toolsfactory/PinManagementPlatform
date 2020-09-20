namespace PinPlatform.Common.DataModels
{
    public class ErrorResponseModel
    {
        public int ErrorCode { get; set; }
        public string ErrorText { get; set; } = string.Empty;
    }

    public class ExtendedErrorResponseModel : ErrorResponseModel
    {
        public object? Data { get; set; }
    }
}
