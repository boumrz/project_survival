using UnityEngine;

public class Stamina
{
    public StatLevel statLevel;
    public StatValue statValue;
    public readonly float staminaDelay = 2f;


    public Stamina(float exp = 0)
    {
        statLevel = new StatLevel(exp);
        statValue = new StatValue(100, 100);
    }

    public void StaminaChangeSprint(out float lastSprintTime) 
    {
        statValue.Sub(Time.deltaTime * 8f);
        lastSprintTime = Time.time;
    }

    public void StaminaRecovery() {
            statValue.Add(Time.deltaTime * 3f);
    }

    public void StaminaChangeJump(out float lastSprintTime) {
        statValue.Sub(10);
        lastSprintTime = Time.time;
    }
}
