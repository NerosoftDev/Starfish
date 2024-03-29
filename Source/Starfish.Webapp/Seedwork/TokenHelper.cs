﻿using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Nerosoft.Starfish.Webapp;

internal static class TokenHelper
{
	public static JwtSecurityToken Resolve(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return null;
		}

		try
		{
			var handler = new JwtSecurityTokenHandler();
			var token = handler.ReadJwtToken(value);
			return token;
		}
		catch
		{
			return null;
		}
	}

	public static long GetSubject(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return 0;
		}

		try
		{
			var token = Resolve(value);
			return long.Parse(token.Subject);
		}
		catch
		{
			return 0;
		}
	}

	public static bool Validate(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}

		try
		{
			var token = Resolve(value);
			return token.ValidTo > DateTime.UtcNow;
		}
		catch (Exception exception)
		{
			Debug.WriteLine(exception.Message);
			return false;
		}
	}

	public static DateTime GetExpiration(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return DateTime.MinValue;
		}

		try
		{
			var token = Resolve(value);
			return token.ValidTo;
		}
		catch (Exception exception)
		{
			Debug.WriteLine(exception.Message);
			return DateTime.MinValue;
		}
	}

	public static DateTime? GetIssueTime(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return null;
		}

		try
		{
			var token = Resolve(value);
			return token.IssuedAt;
		}
		catch (Exception exception)
		{
			Debug.WriteLine(exception.Message);
			return null;
		}
	}

	public static string Generate(DateTime expires, Dictionary<string, object> claims)
	{
		return Generate(expires, claims.Select(t => new Claim(t.Key, t.Value?.ToString() ?? "")));
	}

	public static string Generate(DateTime expires, IEnumerable<Claim> claims)
	{
		if (expires <= DateTime.UtcNow.AddDays(1))
		{
			expires = DateTime.UtcNow.AddDays(1);
		}

		var identity = new ClaimsIdentity(claims);

		return Generate(expires, identity);
	}

	public static string Generate(DateTime expires, ClaimsIdentity identity)
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new string('a', 256)));

		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
		var handler = new JwtSecurityTokenHandler();
		var token = handler.WriteToken(handler.CreateJwtSecurityToken(notBefore: new DateTime(1), expires: expires, issuedAt: DateTime.UtcNow, subject: identity, signingCredentials: credentials));

		return token;
	}
}
