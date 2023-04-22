using Main.Models;
using Main.Options;
using Main.Services;
using Main.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Builder;
using Main.Validation;
using FluentValidation;
using System;
using Main.Contracts;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(5000);
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
        {

            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // укзывает, будет ли валидироваться издатель при валидации токена
                ValidateIssuer = true,
                // строка, представляющая издателя
                ValidIssuer = JWTOptions.ISSUER,

                // будет ли валидироваться потребитель токена
                ValidateAudience = true,
                // установка потребителя токена
                ValidAudience = JWTOptions.AUDIENCE,
                // будет ли валидироваться время существования
                ValidateLifetime = true,

                // установка ключа безопасности
                IssuerSigningKey = JWTOptions.GetSymmetricSecurityKey(),
                // валидация ключа безопасности
                ValidateIssuerSigningKey = true
            };
            options.Events = new JwtBearerEvents
            {
                //убираем требование "Bearer " с коротого начинается токен по умолчнию
                OnMessageReceived = context =>
                {
                    string authorizationToken = context.Request.Headers["Authorization"];
                    if (string.IsNullOrEmpty(authorizationToken))
                        context.NoResult();
                    else
                        context.Token = authorizationToken;

                    return Task.CompletedTask;
                }
            };

        });



        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "AdviserAPI", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey

            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
            });
        });


        var ConnectionString = Environment.GetEnvironmentVariable("ConnectionString");
        if (ConnectionString == null)
            ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<DBContext>(options => options.UseNpgsql(ConnectionString));

        builder.Services.AddScoped<IAuth, Auth>();
        builder.Services.AddScoped<IPasswordManager, PasswordManager>();
        builder.Services.AddCors(options =>

                options.AddDefaultPolicy(
                    policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                    )
                );
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


        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<DBContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

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