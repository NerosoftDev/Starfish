using System.Diagnostics;
using System.Text.Json;

namespace Nerosoft.Starfish.Application;

internal partial class JsonConfigurationFileParser
{
	private const string KEY_DELIMITER = ":";

	private JsonConfigurationFileParser()
	{
	}

	private readonly Dictionary<string, string> _data = new(StringComparer.OrdinalIgnoreCase);
	private readonly Stack<string> _paths = new();

	public static IDictionary<string, string> Parse(string json)
	{
		using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
		{
			return Parse(stream);
		}
	}

	public static IDictionary<string, string> Parse(Stream input) => new JsonConfigurationFileParser().ParseStream(input);

	private Dictionary<string, string> ParseStream(Stream input)
	{
		var jsonDocumentOptions = new JsonDocumentOptions
		{
			CommentHandling = JsonCommentHandling.Skip,
			AllowTrailingCommas = true,
		};

		using (var reader = new StreamReader(input))
		using (var doc = JsonDocument.Parse(reader.ReadToEnd(), jsonDocumentOptions))
		{
			if (doc.RootElement.ValueKind != JsonValueKind.Object)
			{
				throw new FormatException($"Top-level JSON element must be an object. Instead, '{doc.RootElement.ValueKind}' was found.");
			}

			VisitObjectElement(doc.RootElement);
		}

		return _data;
	}

	private void VisitObjectElement(JsonElement element)
	{
		var isEmpty = true;

		foreach (var property in element.EnumerateObject())
		{
			isEmpty = false;
			EnterContext(property.Name);
			VisitValue(property.Value);
			ExitContext();
		}

		SetNullIfElementIsEmpty(isEmpty);
	}

	private void VisitArrayElement(JsonElement element)
	{
		int index = 0;

		foreach (JsonElement arrayElement in element.EnumerateArray())
		{
			EnterContext(index.ToString());
			VisitValue(arrayElement);
			ExitContext();
			index++;
		}

		SetNullIfElementIsEmpty(isEmpty: index == 0);
	}

	private void SetNullIfElementIsEmpty(bool isEmpty)
	{
		if (isEmpty && _paths.Count > 0)
		{
			_data[_paths.Peek()] = null;
		}
	}

	private void VisitValue(JsonElement value)
	{
		Debug.Assert(_paths.Count > 0);

		switch (value.ValueKind)
		{
			case JsonValueKind.Object:
				VisitObjectElement(value);
				break;

			case JsonValueKind.Array:
				VisitArrayElement(value);
				break;

			case JsonValueKind.Number:
			case JsonValueKind.String:
			case JsonValueKind.True:
			case JsonValueKind.False:
			case JsonValueKind.Null:
				var key = _paths.Peek();
				if (_data.ContainsKey(key))
				{
					throw new FormatException($"A duplicate key '{key}' was found.");
				}

				_data[key] = value.ToString();
				break;

			case JsonValueKind.Undefined:
			default:
				throw new FormatException($"Unsupported JSON token '{value.ValueKind}' was found.");
		}
	}

	private void EnterContext(string context) => _paths.Push(_paths.Count > 0 ? _paths.Peek() + KEY_DELIMITER + context : context);

	private void ExitContext() => _paths.Pop();
}

internal partial class JsonConfigurationFileParser
{
	public static string InvertParsed(IDictionary<string, string> data)
	{
		var root = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

		foreach (var entry in data)
		{
			var path = entry.Key.Split(":");
			var current = root;

			for (var i = 0; i < path.Length - 1; i++)
			{
				if (!current.TryGetValue(path[i], out var child))
				{
					child = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
					current[path[i]] = child;
				}

				current = (Dictionary<string, object>)child;
			}

			current[path[^1]] = entry.Value;
		}

		var result = Rebuild(root);

		return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });

		object Rebuild(object source)
		{
			switch (source)
			{
				case IDictionary<string, object> dict:
					if (DetectArray(dict))
					{
						object array = ConvertToArray(dict);
						array = Rebuild(array);
						return array;
					}
					else
					{
						var keys = dict.Keys.Select(x => x).ToList();
						foreach (var key in keys)
						{
							var val = dict[key];
							dict[key] = Rebuild(val);
						}

						return dict;
					}

				case object[] array:
					for (var index = 0; index < array.Length; index++)
					{
						array[index] = Rebuild(array[index]);
					}

					return array;
			}

			return source;
		}

		bool DetectArray(IDictionary<string, object> dictionary)
		{
			var keys = Enumerable.Range(0, dictionary.Count).Select(t => t.ToString());
			return dictionary.Keys.All(key => keys.Contains(key));
		}

		object[] ConvertToArray(IDictionary<string, object> dictionary)
		{
			return dictionary.Values.ToArray();
		}
	}
}