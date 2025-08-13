using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.HttpLogging;
using System.Reflection;
using TicketDepot.Shared;

namespace TicketDepot.TicketManagement.WebApi
{
    /// <summary>
    /// Starting point for the web api.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method for the application.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
                logging.CombineLogs = true;
            });

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();

            builder.Configuration.AddEnvironmentVariables(Constants.EnvConfigPrefix);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                // Configure Swagger document details
                //options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // Enable XML comments for Swagger documentation
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            builder.Services.AddApplicationInsightsTelemetry(options =>
            {
                options.ConnectionString = builder.Configuration["ApplicationInsights:InstrumentationKey"];
                options.EnableDebugLogger = true;
            });

            builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
                .AddNegotiate();

            builder.Services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy.
                options.FallbackPolicy = options.DefaultPolicy;
            });


            // Add AutoMapper to the service collection
            // builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddAutoMapper(typeof(ModelMapper));

            builder.Services.AddMiscServices();
            builder.Services.AddCosmosDbClients(builder.Configuration);
            builder.Services.AddBusinessProviders();

            WebApplication app = builder.Build();

            //app.UseMiddleware<ExceptionHandlingMiddleware>();
            //app.UseMiddleware<ApiKeyMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    // options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
                    // Optionally set the Swagger UI at the root URL
                    // options.RoutePrefix = string.Empty; 
                });
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseHttpLogging();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
