/// <summary>
/// disgostan, but will do for now
/// </summary>
/// 
/// //ryan forgive me, I needed to add in my rank stuff and I felt like this was the best place for that..
public static class GameState
{
    public static float CompanionGrievance;
    public static int ScrapTotal;
    public static int CurrentArea;
    public static int DashRank;
    public static int DamageRank;
    public static int SwingSpeedRank;
    public static int MoveSpeedRank;

    public static void ResetForNewGame()
    {
        CompanionGrievance = 0f;
        ScrapTotal = 1;
        CurrentArea = 0;
        DashRank = 0;
        DamageRank = 0;
        SwingSpeedRank = 0;
        MoveSpeedRank = 0;
    }
}
