using Microsoft.EntityFrameworkCore;
using PokemonReviewApp;
using PokemonReviewApp.Data;


var builder = WebApplication.CreateBuilder(args);

// 1) Manage NuGet Packages: Microsoft.EntityFrameworkCore : Tools Design ServerSQL
// 2) Add to Program.cs neccessarry tools
// 3) Connect db to vs framework:
// SQL Server Ogj Explorer => add server => Server: DESKTOP-RGNQQ5O\SQLEXPRESS
// 4) Set appsettings.sjon ConnectionStrings
// 5) Create models & table relations, DataContex
// 6) Manager console: Add-Migration InitialCreate
//                   : Update-Database
// 7) PowerShell: cd PokemonApp
//              : dotnet run seeddata




// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient<Seed>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();




////////////////////////////////////////////////////////////////////
if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedDataContext();
    }
}
////////////////////////////////////////////////////////////////////




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
