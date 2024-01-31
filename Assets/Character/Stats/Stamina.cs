using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    public StatLevel statLevel;
    public StatValue statValue;
    private float lastSprintTime;
    private float staminaDelay = 2f;


    public Stamina(float exp = 0)
    {
        statLevel = new StatLevel(exp);
        statValue = new StatValue(100, 100);
    }

    public void StaminaChangeSprint() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            statValue.Sub(Time.deltaTime * 8f);
            lastSprintTime = Time.time;
        }
    }

    public void StaminaRecovery() {
        if (Time.time > lastSprintTime + staminaDelay) {
            statValue.Add(Time.deltaTime * 3f);
        }
    }

    public void StaminaChangeJump() {
        statValue.Sub(10);
        lastSprintTime = Time.time;
    }

    public bool isStaminaSprintChecked() {
        return statValue.currentValue > 0;
    }

    public bool isStaminaJumpChecked() {
        return statValue.currentValue >= 10f;
    }
}
