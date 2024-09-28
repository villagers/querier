using Querier.Extensions;
using Querier.Web.Tests.Shared.Entites;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("MySQL");
builder.Services.AddQuerier(o =>
{
    o.UseMySql(connectionString);
    o.LocalStoragePath = "/home/queries";
});

var app = builder.Build();
//app.UseQuerier();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.UseQuerier(e => e.Types.LoadType(typeof(string)).LoadAssembly(""));
app.Run();

public partial class Program { }