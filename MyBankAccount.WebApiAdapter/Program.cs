using Microsoft.EntityFrameworkCore;
using MyBankAccount.Core.Interfaces;
using MyBankAccount.Domain.Services;
using MyBankAccount.SQLiteAdapter;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddTransient<IBankAccountService, BankAccountService>();
builder.Services.AddTransient<IBankAccountRepository, BankAccountSQLiteRepository>();
builder.Services.AddDbContext<BankAccountDBContext>(options =>
                  options.UseSqlite(connectionString));


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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
