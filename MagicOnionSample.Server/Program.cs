using Grpc.Net.Client;
using MagicOnion.Server;
using MagicOnionSample.Server.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Accept HTTP/2 only to allow insecure HTTP/2 connections during development.
builder.WebHost.ConfigureKestrel(options =>
    options.ConfigureEndpointDefaults(endpointOptions => endpointOptions.Protocols = HttpProtocols.Http2));

// Grpc Service 등록
builder.Services.AddGrpc();
// MagicOnion Service 등록
builder.Services.AddMagicOnion();

// Jwt Authentication 등록
builder.Services.AddJwtAuthentication(builder);
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    // MagicOnion용 Swagger 맵핑
    {
        var magicOnionServiceDefinition = app.Services.GetService<MagicOnionServiceDefinition>();

        app.MapMagicOnionHttpGateway("_", magicOnionServiceDefinition.MethodHandlers,
            GrpcChannel.ForAddress("http://localhost:5000"));
        app.MapMagicOnionSwagger("swagger", magicOnionServiceDefinition.MethodHandlers, "/_/");
    }
}

// Asp.net의 인증을 사용하기 위한 설정 추가
app.UseAuthentication();
app.UseAuthorization();

// MagicOnion용 Grpc Service 맵핑
app.MapMagicOnionService();

app.MapGet("/", () => "Hello World!");

app.Run();