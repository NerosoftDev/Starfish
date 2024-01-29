namespace Nerosoft.Starfish.Webapp.Pages.Configs;

public class EditDialogArgs
{
	public EditDialogArgs(string appId, string environment)
	{
		AppId = appId;
		Environment = environment;
	}

	public string AppId { get; set; }

	public string Environment { get; set; }

	public Dictionary<string, object> Properties { get; set; }
}