var builder = WebApplication.CreateBuilder(args); //
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
    {
        builder.WithOrigins("*");
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    }
    );
}
);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
