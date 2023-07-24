using System.Net;
using System.Text.Json;
using FlightOffers.Shared.Models.Constants;
using FlightOffers.Shared.Models.Exceptions;
using FlightOffers.Shared.Models.Response;
using Microsoft.AspNetCore.Http;

namespace FlightOffers.Shared.Models.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (BadRequestException b)
        {
            await HandleErrorResponse(httpContext, HttpStatusCode.BadRequest, b.Message);
        }
        catch (ServiceUnavailableException ex)
        {
            await HandleErrorResponse(httpContext, HttpStatusCode.ServiceUnavailable, ErrorMessages.ServiceUnavailable);
        }
        catch (Exception ex)
        {
            await HandleErrorResponse(httpContext, HttpStatusCode.InternalServerError, ErrorMessages.InternalServerError);
        }
    }

    private async Task HandleErrorResponse(HttpContext context, HttpStatusCode statusCode, string errorMessage)
    {
        context.Response.StatusCode = (int)statusCode;
        var errorResponse = new ErrorResponse()
        {
            Status = (int)statusCode,
            Message = errorMessage
        };

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreNullValues = true
        };
        context.Response.ContentType = "application/json";
        var jsonResponse = JsonSerializer.Serialize(errorResponse, jsonSerializerOptions);
        await context.Response.WriteAsync(jsonResponse);
    }



}