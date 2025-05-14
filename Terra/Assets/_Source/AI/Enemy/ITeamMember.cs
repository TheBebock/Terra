namespace _Source.AI.Enemy
{
    public interface ITeamMember
    {
        TeamType GetTeam();
    }

    public enum TeamType
    {
        Player,
        Enemy
    }
}