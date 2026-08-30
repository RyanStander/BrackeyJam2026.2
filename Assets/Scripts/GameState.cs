/// <summary>
/// disgostan, but will do for now
/// </summary>
public static class GameState
{
    public static float CompanionGrievance;
    public static int ScrapTotal;
    public static int CurrentArea;

    public static void ResetForNewGame()
    {
        CompanionGrievance = 0f;
        ScrapTotal = 0;
        CurrentArea = 0;
    }
}
