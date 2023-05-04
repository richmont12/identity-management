using Serilog;

namespace Backend.Api1;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        
        builder.Services
            .AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", 
                 options =>
            {
                options.Authority = "https://localhost:5001";
                options.Audience = "dataapi1";
            })
            .AddAuthorization();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        
        app.UseDeveloperExceptionPage();

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(x => x.MapControllers().RequireAuthorization());


        return app;
    }
}