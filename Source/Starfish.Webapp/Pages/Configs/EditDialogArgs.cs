namespace Nerosoft.Starfish.Webapp.Pages.Configs;

public class EditDialogArgs
{
	public EditDialogArgs(string id)
	{
		Id = id;
	}

	public string Id { get; set; }

	public Dictionary<string, object> Properties { get; set; }
}