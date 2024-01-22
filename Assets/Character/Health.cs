using System.Collections.Generic;

public class Health
{
    public StatLevel statLevel;
    public StatValue statValue;

    public Health(float exp = 0)
    {
        statLevel = new StatLevel(exp);
        statValue = new StatValue(100, 100);
    }
}
