using System;
using UnityEngine;

public class StatLevel
{
    public int level { get; private set; }
    public float exp { get; private set; }
    public float expToNextLevel { get; private set; }

    private float expTotal;
    
    public void AddExp(int value)
    {
        expTotal += value;
        if(exp > expToNextLevel) LvlUp();
    }
    
    private void LvlUp()
    {
        while (exp >= expToNextLevel)
        {
            exp -= expToNextLevel;
            level++;
            expToNextLevel = MathF.Round(Mathf.Pow(level + 1, 1.5f) / 2 + 0.5f, 1);
            if (level > 100)
            {
                level = 100;
                expToNextLevel = MathF.Round(Mathf.Pow(level + 1, 1.5f) / 2 + 0.5f, 1);
                exp = expToNextLevel;
            }
        }
    }

    public StatLevel(float totalExp = 0)
    {
        expTotal = totalExp;
        exp = expTotal;
        level = 0;
        expToNextLevel = MathF.Round(Mathf.Pow(level + 1, 1.5f) / 2 + 0.5f, 1);
        while (exp >= expToNextLevel)
        {
            exp -= expToNextLevel;
            level++;
            expToNextLevel = MathF.Round(Mathf.Pow(level + 1, 1.5f) / 2 + 0.5f, 1);
        }

        if (level > 100)
        {
            level = 100;
            expToNextLevel = MathF.Round(Mathf.Pow(level + 1, 1.5f) / 2 + 0.5f, 1);
            exp = expToNextLevel;
        }
    }
}