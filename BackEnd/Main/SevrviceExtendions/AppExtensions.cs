//using Microsoft.Extensions.DependencyInjection;

using Main.Models;
using Main.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Main.SevrviceExtendions

{
    public static class AppExtensions
    {
        public static IServiceCollection AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
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
                            new OpenApiSecurityScheme                    {
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
            return services;
            //services.AddTransient<TimeService>();
        }
        public static IServiceCollection AddAuthenticationExtension(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

            return services;
        }
        public static IServiceCollection AddCorsExtension(this IServiceCollection services)
        {
            services.AddCors(options =>

                options.AddDefaultPolicy(
                    policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                    )
                );

            return services;
        }
        public static IServiceCollection AddDbContextExtension(this IServiceCollection services, IConfiguration config)
        {
            var ConnectionString = Environment.GetEnvironmentVariable("ConnectionString");
            if (ConnectionString == null)
                ConnectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<DBContext>(options => options.UseNpgsql(ConnectionString));

            return services;
        }
    }
}
