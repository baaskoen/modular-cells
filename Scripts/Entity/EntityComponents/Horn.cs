using UnityEngine;

public class Horn : EntityComponent
{
    public void Start()
    {
        EntityStats stats = this.entity.getStats();

        stats.bumpDamage += 20;
    }


    override public void beforeDestroy()
    {
        EntityStats stats = this.entity.getStats();

        stats.bumpDamage -= 20;
    }
}