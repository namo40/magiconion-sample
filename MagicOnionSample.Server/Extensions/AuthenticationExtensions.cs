using MagicOnionSample.Server.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MagicOnionSample.Server.Extensions;

public static class AuthenticationExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var jwtServiceOptions = builder.Configuration.GetJwtServiceOptions();

        builder.Services.Configure<JwtServiceOptions>(builder.Configuration.GetJwtServiceOptionsConfigurationSection());

        services.AddScoped<IAuthenticateService, AuthenticateService>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(jwtServiceOptions.AccessTokenSecretBytes),
                    ClockSkew = TimeSpan.Zero,

                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true
                };

#if DEBUG
                options.RequireHttpsMetadata = false;
#endif
            });
    }

    private static IConfigurationSection GetJwtServiceOptionsConfigurationSection(this IConfiguration configuration)
        => configuration.GetSection("MagicOnionSample.Server:JwtTokenService");

    private static JwtServiceOptions GetJwtServiceOptions(this IConfiguration configuration)
        => configuration.GetJwtServiceOptionsConfigurationSection().Get<JwtServiceOptions>();
}