namespace CivittaTest.API.Middlewares
{
    public class GlobalErrorHandler(RequestDelegate next, ILogger<GlobalErrorHandler> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex switch
                {
                    InvalidOperationException => StatusCodes.Status400BadRequest,
                    InvalidDataException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex switch
                    {
                        InvalidOperationException or InvalidDataException => ex.Message,
                        _ => "Internal Server Error"
                    }
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
