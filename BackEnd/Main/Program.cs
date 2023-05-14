using FluentValidation;
using Main.Models;
using Main.Options;
using Main.Services;
using Main.Services.Interfaces;
using Main.SevrviceExtendions;
using Main.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(5000);
        });

        builder.Services.AddAuthenticationExtension();


        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddSwaggerExtension();

        builder.Services.AddDbContextExtension(builder.Configuration);

        builder.Services.AddScoped<IAuth, Auth>();
        builder.Services.AddScoped<IPasswordManager, PasswordManager>();
        builder.Services.AddCorsExtension();
        builder.Services.AddValidatorsFromAssemblyContaining<CustomerValidator>();
        var app = builder.Build();

        //if (app.Environment.IsDevelopment())

        app.UseSwagger();
        app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        app.UseDeveloperExceptionPage();


        app.UseDataMaseMigrations();

        app.UseStaticFiles();
        app.UseRouting();

        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}