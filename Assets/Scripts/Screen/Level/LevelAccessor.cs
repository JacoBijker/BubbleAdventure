public class LevelAccessor
{
    private static Level currentLevel;

    public Level Level
    {
        get
        {
            return currentLevel;
        }
        set
        {
            currentLevel = value;
        }
    }
}
