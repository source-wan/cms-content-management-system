using CMS.Api.Service;
using CMS.App.Common.Interface;
using CMS.Infrastructure;
using CMS.Infrastructure.Filters;
using CMS.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
});
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
//         options => builder.Configuration.Bind("JwtSettings", options))
//     .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
//         options => builder.Configuration.Bind("CookieSettings", options));

// Register http context
builder.Services.AddHttpContextAccessor();
// builder.Services.AddControllers(options=>{options.Filters.Add(typeof(AuditLogFliter));});
// builder.Services.AddControllers(options=>{options.Filters.Add(typeof(HTTPGlobalExceptionFilter));});
// Register user information service
builder.Services.AddTransient(typeof(IUserSession), typeof(UserSession));
builder.Services.AddTransient(typeof(IQueryHelper<>), typeof(QueryHelper<>));
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MigrateDatabase();



app.Run();
