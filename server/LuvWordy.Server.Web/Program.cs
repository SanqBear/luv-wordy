using LuvWordy.Server.Web.Utils.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                });
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
    config.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.OperationFilter<RemoveVersionParameterFilter>();
    config.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();

    var descriptionFileName = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var modelDescriptionFileName = $"{typeof(LuvWordy.Server.Model.Repositories.WordRepository).Assembly.GetName().Name}.xml";
    config.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, descriptionFileName));
    config.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, modelDescriptionFileName));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Configure the HTTP request pipeline.
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
