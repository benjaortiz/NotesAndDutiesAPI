using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NotesAndDutiesAPI;

var builder = WebApplication.CreateBuilder(args);

//configure database
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=nad.db"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>{
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt: Issuer"],
            ValidAudience = builder.Configuration[ "Jwt: Audience"],
            IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes(builder.Configuration
                ["Jwt:Key"]))
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
        {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Notes and Duties API V1");
        options.RoutePrefix = string.Empty;
        });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRouting();

app.MapControllers();
app.UseAuthorization();
app.Run();
