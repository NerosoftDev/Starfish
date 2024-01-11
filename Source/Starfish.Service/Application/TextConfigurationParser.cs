namespace Nerosoft.Starfish.Application;

public partial class TextConfigurationParser : IConfigurationParser
{
	public IDictionary<string, string> Parse(string content)
	{
		using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
		{
			return Parse(stream);
		}
	}

	public IDictionary<string, string> Parse(Stream input) => new TextConfigurationParser().ParseStream(input);

	private Dictionary<string, string> ParseStream(Stream input)
	{
		var data = new Dictionary<string, string>();

		using (var reader = new StreamReader(input))
		{
			while (reader.Peek() > 0)
			{
				var line = reader.ReadLine();
				if (string.IsNullOrWhiteSpace(line))
				{
					continue;
				}

				var index = line.IndexOf('=');
				if (index == -1)
				{
					continue;
				}

				var key = line[..index].Trim();
				var value = line[(index + 1)..];
				if (string.IsNullOrWhiteSpace(key))
				{
					continue;
				}

				data[key] = value;
			}
		}

		return data;
	}
}

public partial class TextConfigurationParser
{
	public string InvertParse(IDictionary<string, string> data)
	{
		if (data == null || data.Count == 0)
		{
			return string.Empty;
		}

		var builder = new StringBuilder();
		foreach (var (key, value) in data)
		{
			builder.AppendLine($"{key}={value}");
		}

		return builder.ToString();
	}
}