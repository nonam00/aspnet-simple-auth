﻿using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Web.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private const string UnhandledExceptionMsg = "An unhandled exception has occurred while executing the request.";

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = CreateProblemDetails(context, exception);
        var json = ToJson(problemDetails);

        const string contentType = "application/problem+json";
        context.Response.ContentType = contentType;
        await context.Response.WriteAsync(json, cancellationToken);

        return true;
    }
    private ProblemDetails CreateProblemDetails(in HttpContext context, in Exception exception)
    {
        var reasonPhrase = ReasonPhrases.GetReasonPhrase(context.Response.StatusCode);
        if (string.IsNullOrEmpty(reasonPhrase))
        {
            reasonPhrase = UnhandledExceptionMsg;
        }

        var problemDetails = new ProblemDetails
        {
            Status = context.Response.StatusCode,
            Title = reasonPhrase,
            Detail = exception.Message,
            Extensions =
            {
                ["traceId"] = Activity.Current?.Id,
                ["requestId"] = context.TraceIdentifier,
                ["data"] = exception.Data
            }
        };

        return problemDetails;
    }

    private string ToJson(in ProblemDetails problemDetails)
    {
        try
        {
            return JsonSerializer.Serialize(problemDetails);
        }
        catch (Exception)
        {
            return "An exception has occurred while serializing error to JSON";
        }
    }
}