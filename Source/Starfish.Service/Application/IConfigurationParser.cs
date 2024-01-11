namespace Nerosoft.Starfish.Application;

public interface IConfigurationParser
{
	IDictionary<string, string> Parse(string content);

	IDictionary<string, string> Parse(Stream input);

	string InvertParse(IDictionary<string, string> data);
}