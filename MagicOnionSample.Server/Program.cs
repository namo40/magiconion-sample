using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Accept HTTP/2 only to allow insecure HTTP/2 connections during development.
builder.WebHost.ConfigureKestrel(options =>
    options.ConfigureEndpointDefaults(endpointOptions => endpointOptions.Protocols = HttpProtocols.Http2));

// Grpc Service 등록
builder.Services.AddGrpc();
// MagicOnion Service 등록
builder.Services.AddMagicOnion();

var app = builder.Build();

// MagicOnion용 Grpc Service 맵핑
app.MapMagicOnionService();

app.MapGet("/", () => "Hello World!");

app.Run();