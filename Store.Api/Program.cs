using FastEndpoints;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Store.Api.Extensions;
using Store.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddConfigSettings(builder.Configuration);
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddFastEndpoints();
builder.Services.AddOcelot();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();
await app.UseOcelot();
app.Run();
