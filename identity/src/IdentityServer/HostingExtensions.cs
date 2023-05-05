using System.Reflection;
using Duende.IdentityServer.Test;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServer;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddControllersWithViews();

        builder.Services.AddDbContext<IdentityDbContext>(
            options => options.UseNpgsql(
                "Host=localhost;Database=Identity;Username=postgres;Password=docker",
                opt =>
                    opt.MigrationsAssembly(Assembly.GetAssembly(typeof(HostingExtensions))!.GetName().Name))
        );

        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddIdentityServer(
                // options =>
                // {
                //     // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                //     options.EmitStaticAudienceClaim = true;
                //     // to fulfill RFC 9068
                //     // options.EmitScopesAsSpaceDelimitedStringInJwt = true; 
                // }
            )
            .AddDeveloperSigningCredential(
                false
            )
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryApiResources(Config.ApiResources)
            .AddInMemoryClients(Config.Clients)
            //.AddTestUsers(TestUsers.Users)
            .AddExtensionGrantValidator<TokenExchangeGrantValidator>()
            .AddProfileService<ProfileService>()
            .AddAspNetIdentity<IdentityUser>()
            ;


        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        CreateUser(app);

        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();

        app.UseAuthorization();
        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            }
        );

        return app;
    }

    private static void CreateUser(WebApplication app)
    {
        var serviceProvider = app.Services;
        using IServiceScope scope = serviceProvider
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        UserManager<IdentityUser> userMgr = scope.ServiceProvider
            .GetRequiredService<UserManager<IdentityUser>>();
        IdentityUser user = userMgr.FindByNameAsync(
                "alice"
            )
            .Result;

        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = "alice",
                Email = "alice@bob.com",
                EmailConfirmed = true
            };

            IdentityResult result = userMgr.CreateAsync(
                    user,
                    "Alice123$"
                )
                .Result;

            if (!result.Succeeded)
            {
                throw new Exception(
                    result.Errors.First()
                        .Description
                );
            }
        }
    }
}