﻿namespace Nerosoft.Starfish.Webapp.Rest;

internal class RestServiceOptions
{
	private readonly Dictionary<string, string> _urls = new();

	public string BaseUrl { get; set; }

	public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);

	public Func<Task<string>> TokenFactory { get; set; }

	public void SetUrl(string name, string url)
	{
		_urls[name] = url;
	}

	public string GetUrl(string name)
	{
		return _urls.GetValueOrDefault(name, BaseUrl);
	}
}