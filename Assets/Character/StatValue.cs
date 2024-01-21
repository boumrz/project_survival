using System;

public class StatValue
{
    public float currentValue { get; private set; }
    public float maxValue { get; private set; }

    public void Add(float value)
    {
        var newValue = currentValue + value;
        currentValue = newValue > maxValue ? maxValue : newValue;
    }

    public void Sub(float value)
    {
        var newValue = currentValue - value;
        currentValue = newValue < 0 ? 0 : newValue;
    }

    public StatValue(float currentValue, float maxValue)
    {
        this.currentValue = currentValue;
        this.maxValue = maxValue;
    }
}