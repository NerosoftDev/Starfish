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

	private DateTime IssueTime { get; set; } = DateTime.UtcNow;

	private TimeSpan ExpireTime { get; set; } = TimeSpan.FromDays(1);

	private string Audience { get; set; }

	private string Issuer { get; set; }

	private string SigningKey { get; set; }

	private string Algorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;

	internal TokenGeneratorBuilder()
	{
		//generate token id
		_claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
	}

	public TokenGeneratorBuilder AddClaim(string type, string value)
	{
		_claims.Add(new Claim(type, value));
		return this;
	}

	public TokenGeneratorBuilder AddClaim(string type, string value, string valueType)
	{
		_claims.Add(new Claim(type, value, valueType));
		return this;
	}

	public TokenGeneratorBuilder AddScope(params string[] scopes)
	{
		foreach (var scope in scopes)
		{
			AddClaim(JwtClaimTypes.Scope, scope);
		}

		return this;
	}

	public TokenGeneratorBuilder AddAudience(string audience)
	{
		return AddClaim(JwtRegisteredClaimNames.Aud, audience);
	}

	public TokenGeneratorBuilder AddRole(params string[] roles)
	{
		if (roles?.Length > 0)
		{
			foreach (var role in roles)
			{
				AddClaim(JwtClaimTypes.Role, role);
			}
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

	public TokenGeneratorBuilder ExpiresIn(TimeSpan time)
	{
		ExpireTime = time;
		return this;
	}

	public TokenGeneratorBuilder IssuedAt(DateTime time)
	{
		// if (time != null)
		// {
		// 	AddClaim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(time.Value).ToString(), ClaimValueTypes.Integer64);
		// }

		IssueTime = time;

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
		var handler = new JwtSecurityTokenHandler();
		var key = Encoding.UTF8.GetBytes(SigningKey.ToSha256());
		var descriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(_claims),
			Expires = IssueTime.Add(ExpireTime),
			Issuer = Issuer,
			Audience = Audience,
			IssuedAt = IssueTime,
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};

		var token = handler.CreateToken(descriptor);
		var accessToken = handler.WriteToken(token);
		return accessToken;
	}
}