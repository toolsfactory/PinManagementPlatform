using Microsoft.AspNetCore.Http;

namespace PinPlatform.Common.DataModels
{
    public class ApiResponse
    {
        public ApiResponse(int statuscode, string subCode, string traceId, string? message = default, string? detail = default, string? help = default)
        {
            StatusCode = statuscode;
            SubCode = subCode;
            TraceId = traceId;
            Message = message;
            Detail = detail;
            Help = help;
        }

        public int StatusCode { get; }
        public string SubCode { get; }
        public string TraceId { get; }
        public string? Message { get;  }
        public string? Detail { get; }
        public string? Help { get; }
    }

    public class ApiResponse<T> : ApiResponse
        where T : class
    {
        public T Data { get; }
        public ApiResponse(T data, int statuscode, string subCode, string traceId, string? message = null, string? detail = null, string? help = null) : base(statuscode, subCode, traceId, message, detail, help)
        {
            Data = data;
        }
    }
}