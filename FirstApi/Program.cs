using FirstApi.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//tạo file log

// cài 2 package
// 1. serilog.aspnetcore
// 2. serilog.sink.files (package tạo file log)

// sử dụng thư viện: using Serilog;
// mã lệnh tạo file log

//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().
//	WriteTo.File("log/userslog.txt", rollingInterval: RollingInterval.Day).CreateLogger();

//builder.Host.UseSerilog();

//

builder.Services.AddControllers(option =>
{
	//option.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ILogging, LoggingV2>();

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
