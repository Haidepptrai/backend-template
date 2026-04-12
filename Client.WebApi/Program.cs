using Application.Persistence;
using Application.Services.Category;
using Application.Services.Category.Request.CreateCategory;
using Application.Services.Product;
using Application.Services.Auth;
using Client.WebApi.Extensions;
using Client.WebApi.Middlewares;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Client.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Vincent API",
                Version = "v1"
            });
        });

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new ApplicationException("Connection String Not Found")));

        builder.Services
            .AddAuthentication("CookieAuth")
            .AddCookie("CookieAuth", options =>
            {
                options.Cookie.Name = "auth_cookie";

                options.Cookie.HttpOnly = true;

                // IMPORTANT: Local dev vs production
                options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
                    ? CookieSecurePolicy.None   // allow HTTP locally
                    : CookieSecurePolicy.Always;

                options.Cookie.SameSite = SameSiteMode.Lax;
                // Use Strict if no cross-site needs
                // Use None ONLY if frontend is on different domain (requires HTTPS)

                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;

                options.LoginPath = "/api/auth/login";
                options.LogoutPath = "/api/auth/logout";

                options.Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = async context =>
                    {
                        // Advanced: revalidate user (e.g., check DB for revoked session)
                        // Example:
                        // var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        // if (!await userService.IsSessionValid(context.Principal))
                        // {
                        //     context.RejectPrincipal();
                        //     await context.HttpContext.SignOutAsync();
                        // }
                    }
                };
            });

        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        builder.Services.AddValidatorsFromAssembly(typeof(CreateCategoryRequestValidator).Assembly);
        builder.Services.AddFluentValidationAutoValidation();

        builder.Services.AddAwsS3(builder.Configuration);

        var app = builder.Build();

        app.UseExceptionHandler();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
