using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApplication1.Conventions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new GroupingByNamespaceConvention());
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    config =>
    {
        var titleBase = "Movies API";
        var description = "This is a Web API for Movies operations";
        var TermsOfService = new Uri("https://udemy.com/user/felipegaviln/");
        var License = new OpenApiLicense()
        {
            Name = "MIT"
        };
        var Contact = new OpenApiContact()
        {
            Name = "Felipe Gavilán",
            Email = "felipe_gavilan887@hotmail.com",
            Url = new Uri("https://gavilan.blog/")
        };

        config.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = titleBase + " v1",
            Description = description,
            TermsOfService = TermsOfService,
            License = License,
            Contact = Contact
        });

        config.SwaggerDoc("v2", new OpenApiInfo
        {
            Version = "v2",
            Title = titleBase + " v2",
            Description = description,
            TermsOfService = TermsOfService,
            License = License,
            Contact = Contact
        });

        //Boton Authorize (Swagger)
        config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authentication",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        config.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[]{}
            }
        });



    });


//< Adicional JWT>
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
//</Adicional JWT>

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    config.SwaggerEndpoint("/swagger/v1/swagger.json", "MoviesAPI v1");
    config.SwaggerEndpoint("/swagger/v2/swagger.json", "MoviesAPI v2");
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication(); //Adicional JWT

app.UseAuthorization();

app.MapControllers();

app.Run();
