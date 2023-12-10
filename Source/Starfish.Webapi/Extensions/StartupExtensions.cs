using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Nerosoft.Starfish.Webapi;

internal static class StartupExtensions
{
	public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

		var bearerOptions = configuration.GetSection(nameof(JwtBearerOptions)).Get<JwtBearerOptions>();

		var tokenKey = configuration.GetValue<string>("JwtBearerOptions:TokenKey");
		var key = Encoding.UTF8.GetBytes(tokenKey.ToSha256());
		var issuer = configuration.GetValue<string>("JwtBearerOptions:TokenIssuer");

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(options =>
		{
			options.Authority = bearerOptions.Authority;
			options.RequireHttpsMetadata = bearerOptions.RequireHttpsMetadata;
			options.Audience = bearerOptions.Audience;

			options.Events = new JwtBearerEvents()
			{
				OnMessageReceived = context =>
				{
					context.Token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
					return Task.CompletedTask;
				},
				OnChallenge = _ => Task.CompletedTask
			};
			options.TokenValidationParameters = new TokenValidationParameters
			{
				NameClaimType = JwtClaimTypes.Name,
				RoleClaimType = JwtClaimTypes.Role,
				ValidIssuers = new[] { issuer },
				//ValidAudience = "api",
				ValidateIssuer = true,
				ValidateAudience = false,
				IssuerSigningKey = new SymmetricSecurityKey(key)
			};
		});

		return services;
	}
}