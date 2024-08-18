using Microsoft.OpenApi.Models;

namespace DevAI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            

            builder.Services.AddControllers();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "DevAI API",
                    Description = "An ASP.NET Core Web API for interfacing with DevAI.\r\n\r\nThis AI is based on .NET Machine Learning and is currently trained to identify the difference between Cats and Dogs along with spotting the sentiment of a comment or message (positive/negative).",
                    Contact = new OpenApiContact
                    {
                        Name = "GitHub",
                        Url = new Uri("https://github.com/Bruce-Devlin/DevAI")
                    }
                });
            });

            var app = builder.Build();

            string hostURL = "http://[::]:5599/";
            app.Urls.Add(hostURL);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseAuthorization();
            app.MapControllers();

            

            app.Run();
        }
    }
}