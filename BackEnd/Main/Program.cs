using Main;
using Main.Services;
using Main.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
        // ��������, ����� �� �������������� �������� ��� ��������� ������
        ValidateIssuer = true,
        // ������, �������������� ��������
        ValidIssuer = JWTOptions.ISSUER,

        // ����� �� �������������� ����������� ������
        ValidateAudience = true,
        // ��������� ����������� ������
        ValidAudience = JWTOptions.AUDIENCE,
        // ����� �� �������������� ����� �������������
        ValidateLifetime = true,

        // ��������� ����� ������������
        IssuerSigningKey = JWTOptions.GetSymmetricSecurityKey(),
        // ��������� ����� ������������
        ValidateIssuerSigningKey = true
    };

    options.Events = new JwtBearerEvents
    {
        //������� ���������� "Bearer " � �������� ���������� ����� �� ��������
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
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "AdviserAPI", Version = "v1" });
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

builder.Services.AddScoped<IAuth, Auth>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
    app.UseDeveloperExceptionPage();
}


app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();
