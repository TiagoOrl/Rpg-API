global using first_api.service.character_service;
global using first_api.service.user_service;
global using Microsoft.EntityFrameworkCore;
global using first_api.Data;

var builder = WebApplication.CreateBuilder(args);

var connStringName = "LocalConnection";
var connString = builder.Configuration.GetConnectionString(connStringName);

if (connString == null)
{
    throw new Exception($" connection string of name {connStringName} not found");
}


builder.Services.AddDbContext<DataContext>(options => 
    options.UseMySql(connString, ServerVersion.AutoDetect(connString))
);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

var app = builder.Build();

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
