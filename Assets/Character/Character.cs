using System;
using System.Collections.Generic;

public class Character
{
    public Health health;
    public Stamina stamina;
    public Character()
    {
        health = new Health(100);
        stamina = new Stamina(100);
    }
}