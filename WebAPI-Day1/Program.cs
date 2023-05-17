using WebAPI_Day1.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

// Count All Requests
app.Use(async(context, next)=>{
    //Request
    ReqeustsCounterContainer.Count++;
    await next();   
    //Response
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


