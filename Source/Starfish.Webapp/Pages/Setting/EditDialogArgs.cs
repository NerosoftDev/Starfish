namespace Nerosoft.Starfish.Webapp.Pages.Setting;

public class EditDialogArgs
{
	public EditDialogArgs(long appId, string environment)
	{
		AppId = appId;
		Environment = environment;
	}

	public long AppId { get; set; }

	public string Environment { get; set; }

	public Dictionary<string, object> Properties { get; set; }
}