namespace PinPlatform.Common.DataModels
{
    public class ApiResponse
    {
        public ApiResponse(int statuscode, string subCode, string traceId, string? message = default, string? detail = default, string? help = default, object? data = default)
        {
            StatusCode = statuscode;
            SubCode = subCode;
            TraceId = traceId;
            Message = message;
            Detail = detail;
            Help = help;
            Data = data;
        }

        public int StatusCode { get; set; }
        public string SubCode { get; set; }
        public string TraceId { get; }
        public string? Message { get; set; }
        public string? Detail { get; set; }
        public string? Help { get; set; }
        public object? Data { get; }
    }
}