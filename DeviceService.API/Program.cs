using DeviceService.API;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAPIServices();

var app = builder.Build();

app.UseCors("CorsPolicy");
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Device Service API v1"));
app.MapControllers();

app.Run();
