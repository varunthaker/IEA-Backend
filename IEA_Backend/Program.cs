global using IEA_Backend.models;
using IEA_Backend.Services.BookService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("http://localhost:5173") // Adjust this to your React frontend origin
            .AllowAnyMethod()
            .AllowAnyHeader());
});
builder.Services.AddSingleton<IBookService, BookService>();


var app = builder.Build();

//var bookService = app.Services.GetRequiredService<IBookService>();
//bookService.ProcessBooksAsync().GetAwaiter().GetResult();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();
app.MapControllers();




await app.RunAsync();

