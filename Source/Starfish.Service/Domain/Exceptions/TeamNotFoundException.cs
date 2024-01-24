namespace Nerosoft.Starfish.Domain;

public class TeamNotFoundException : NotFoundException
{
	public TeamNotFoundException(long id)
		: base(string.Format(Resources.IDS_ERROR_TEAM_NOT_EXISTS, id))
	{
	}
}