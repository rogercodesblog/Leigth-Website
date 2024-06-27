using Leigth_Website.Services.EmailService;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Dependency Injection
builder.Services.AddTransient<IEmailService, EmailService>();

// GZip Compression
builder.Services.AddResponseCompression();
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

//Cors
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7149/index.html",
                                              "http://localhost:7149/index.html",
                                              "https://www.leigth.live",
                                              "https://www.leigth.live/",
                                              "https://leigth.live",
                                              "https://leigth.live/"
                                              );
                      });
});


builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

//Configuring Index.html
var options = new DefaultFilesOptions();
options.DefaultFileNames.Clear();
options.DefaultFileNames.Add("index.html");

app.UseDefaultFiles(options);

app.UseStaticFiles();
app.UseRouting();
//Cors
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
