using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Interfaces;

namespace TopTenBites.Web.Infrastructure
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;
        private readonly IViewRenderService _viewRenderService;
        private readonly IAppSettingsService _appSettingsService;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger, 
            IViewRenderService viewRenderService, IAppSettingsService appSettingsService)
        {
            _next = next;
            _logger = logger;
            _viewRenderService = viewRenderService;
            _appSettingsService = appSettingsService;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode == 404)
                {
                    await HandleExceptionAsync(httpContext, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            if (httpContext.Request.IsApiCall())
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var json = JsonConvert.SerializeObject(new
                {
                    httpContext.Response.StatusCode,
                    exception.Message
                });
                await httpContext.Response.WriteAsync(json);
            }
            else if (httpContext.Response.StatusCode == 404)
            {
                httpContext.Response.ContentType = "text/html";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var html = await _viewRenderService.RenderToStringAsync(_appSettingsService.Error404ViewPath, null);
                await httpContext.Response.WriteAsync(html);
            }
            else
            {
                httpContext.Response.ContentType = "text/html";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var html = await _viewRenderService.RenderToStringAsync(_appSettingsService.Error500ViewPath, null);
                await httpContext.Response.WriteAsync(html);
            }
        }
    }
}
