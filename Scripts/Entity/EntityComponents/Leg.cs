using System.Collections.Generic;
using UnityEngine;

public class Leg : EntityComponent
{
    public List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    public void Start()
    {
        EntityStats stats = this.entity.getStats();

        stats.movementSpeed += 0.5f;

        foreach (SpriteRenderer renderer in this.renderers)
        {
            renderer.color = entity.getSpecies().getColor();
        }
    }

    override public void beforeDestroy()
    {
        EntityStats stats = this.entity.getStats();

        stats.movementSpeed -= 0.5f;
    }
}