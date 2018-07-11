using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ConsultaVeiculo.API.Filters
{
    public class ExceptionServiceFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ExceptionServiceFilter(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (_hostingEnvironment.IsDevelopment())
            {
                await context.HttpContext.Response.WriteAsync($"{context.Exception.Message}/{context.Exception.StackTrace}");
            }
            else
            {
                await context.HttpContext.Response.WriteAsync("Erro interno no servidor");
            }
        }
    }
}
