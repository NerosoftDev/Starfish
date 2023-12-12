using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;

// ReSharper disable All

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Utility for generating tokens programmatically
/// </summary>
internal static class TokenGenerator
{
	/// <summary>
	/// Create a new token with a specified subject claim
	/// </summary>
	public static TokenGeneratorBuilder Create(string subject)
	{
		return Create(subject, null, null);
	}

	/// <summary>
	/// Create a new token with a specified subject and name claims
	/// </summary>
	public static TokenGeneratorBuilder Create(string subject, string name)
	{
		return Create(subject, name, null);
	}

	/// <summary>
	/// Create a new token with a specified subject, name and audience claims
	/// </summary>
	/// <param name="subject"></param>
	/// <param name="name"></param>
	/// <param name="audience"></param>
	/// <returns></returns>
	public static TokenGeneratorBuilder Create(string subject, string name, string audience)
	{
		var builder = new TokenGeneratorBuilder();
		if (subject != null)
		{
			builder.AddClaimSubject(subject);
		}

		if (name != null)
		{
			builder.AddClaimName(name);
		}

		if (audience != null)
		{
			builder.WithAudience(audience);
		}

		return builder;
	}
}

internal class TokenGeneratorBuilder
{
	private readonly List<Claim> _claims = [];

	private TimeSpan? ExpireTime { get; set; }

	private string Audience { get; set; }

	private string Issuer { get; set; }

	private string SigningKey { get; set; }

	private string Algorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;

	public TokenGeneratorBuilder()
	{
		//generate token id
		_claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
	}

	public TokenGeneratorBuilder AddClaim(string type, string value)
	{
		_claims.Add(new Claim(type, value));
		return this;
	}

	public TokenGeneratorBuilder AddScope(string scope)
	{
		return AddClaim("scope", scope);
	}

	public TokenGeneratorBuilder AddScope(params string[] scopes)
	{
		foreach (var scope in scopes)
			AddScope(scope);
		return this;
	}

	public TokenGeneratorBuilder AddAudience(string audience)
	{
		return AddClaim(JwtRegisteredClaimNames.Aud, audience);
	}

	public TokenGeneratorBuilder AddRole(params string[] roles)
	{
		foreach (var role in roles)
		{
			AddClaim(JwtClaimTypes.Role, role);
		}

		return this;
	}

	public TokenGeneratorBuilder AddClaimName(string name, string type = JwtRegisteredClaimNames.Name)
	{
		AddClaim(type, name);
		return this;
	}

	public TokenGeneratorBuilder AddClaimEmail(string value, string type = JwtRegisteredClaimNames.Email)
	{
		AddClaim(type, value);
		return this;
	}

	public TokenGeneratorBuilder AddClaimSubject(string value, string type = JwtRegisteredClaimNames.Sub)
	{
		return AddClaim(type, value);
	}

	public TokenGeneratorBuilder ExpiresIn(TimeSpan? time)
	{
		ExpireTime = time;
		return this;
	}

	public TokenGeneratorBuilder WithSigningKey(string key)
	{
		SigningKey = key;
		return this;
	}

	public TokenGeneratorBuilder WithAlgorithm(string algorithm)
	{
		Algorithm = algorithm;
		return this;
	}

	public TokenGeneratorBuilder WithAudience(string audience)
	{
		Audience = audience;
		return this;
	}

	public TokenGeneratorBuilder WithIssuer(string issuer)
	{
		Issuer = issuer;
		return this;
	}

	public string Build()
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

		//create security token
		var token = new JwtSecurityToken(issuer: "localhost",
			audience: Audience,
			claims: _claims,
			expires: DateTime.UtcNow + (ExpireTime ?? TimeSpan.FromDays(365)),
			signingCredentials: credentials
		);

		//sign and serialize token to string
		var strToken = new JwtSecurityTokenHandler().WriteToken(token);

		//return token string
		return strToken;
	}
}