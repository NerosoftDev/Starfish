namespace Nerosoft.Starfish.Application;

public class TextConfigurationFileParser
{
	private TextConfigurationFileParser()
	{
	}

	public static IDictionary<string, string> Parse(string json)
	{
		using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
		{
			return Parse(stream);
		}
	}

	public static IDictionary<string, string> Parse(Stream input) => new TextConfigurationFileParser().ParseStream(input);

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