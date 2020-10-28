using Microsoft.AspNetCore.Http;
using PinPlatform.Domain.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace PinPlatform.Services.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainInvalidOperationException ex)
            {
                await HandleDomainInvalidOperationExceptionAsync(context, ex);
            }
            catch (DomainUnknownResourceException ex)
            {
                await HandleDomainUnknownResourceExceptionAsync(context, ex);
            }
            catch (DomainException ex)
            {
                await HandleDomainExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleDomainInvalidOperationExceptionAsync(HttpContext context, DomainException exception)
        {
            return SendErrorResponseAsync(
                context,
                HttpStatusCode.BadRequest,
                new ProblemDetails()
                {
                    Title = "Invalid Operation",
                    Detail = exception.Message
                });
        }

        private static Task HandleDomainUnknownResourceExceptionAsync(HttpContext context, DomainException exception)
        {
            return SendErrorResponseAsync(
                context,
                HttpStatusCode.NotFound,
                new ProblemDetails()
                {
                    Title = "Resource not found",
                    Detail = exception.Message
                });
        }


        private static Task HandleDomainExceptionAsync(HttpContext context, DomainException exception)
        {
            return SendErrorResponseAsync(
                context,
                HttpStatusCode.BadRequest,
                new ProblemDetails()
                {
                    Title = "Bad request",
                    Detail = exception.Message
                });
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            return SendErrorResponseAsync(
                context,
                HttpStatusCode.InternalServerError,
                new ProblemDetails()
                {
                    Title = "Internal Server Error",
                    Detail = exception.Message
                });
        }

        private static Task SendErrorResponseAsync<T>(HttpContext context, HttpStatusCode statusCode, T body) where T : ProblemDetails
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            body.Status = (int)statusCode;
            return context.Response.WriteAsync(JsonSerializer.Serialize<T>(body));
        }
    }
}