namespace Nerosoft.Starfish.Domain;

public class TeamNotFoundException : NotFoundException
{
	public TeamNotFoundException(int id)
		: base(string.Format(Resources.IDS_ERROR_TEAM_NOT_EXISTS, id))
	{
	}
}