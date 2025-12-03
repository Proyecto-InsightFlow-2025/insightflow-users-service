
using insightflow_users_service.src.Data;
using insightflow_users_service.src.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()   // Firebase Hosting
                  .AllowAnyMethod()   // GET, POST, PATCH, DELETE
                  .AllowAnyHeader();  // X-User-Id header
        });
});
builder.Services.AddSingleton<ApplicationDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "Users Service is Running!");
app.Run();
