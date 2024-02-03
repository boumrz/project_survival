public class Character
{
    public Health health;
    public Stamina stamina;
    public Starvation starvation;
    public Character()
    {
        health = new Health(100);
        stamina = new Stamina(100);
        starvation = new Starvation();
    }
}