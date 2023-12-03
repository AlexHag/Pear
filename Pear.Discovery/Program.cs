using Microsoft.EntityFrameworkCore;
using Pear.Discovery.Persistance;
using Pear.Discovery.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PearDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPearFriendsRepository, PearFriendsRepository>();
builder.Services.AddScoped<IPearMessageRepository, PearMessageRepository>();

builder.Services.AddSignalR();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<MainHub>("/main");

app.Run();
