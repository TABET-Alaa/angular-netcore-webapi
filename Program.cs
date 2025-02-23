var builder = WebApplication.CreateBuilder(args);

//Json Serialiser
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.IncludeFields = true;
});

//Allow Cors
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policyBuilder =>
    {
        policyBuilder.AllowAnyHeader()
        .AllowAnyMethod().WithOrigins("http://localhost:4200");
    });
});


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


app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
