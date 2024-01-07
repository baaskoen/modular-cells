using UnityEngine;

public class Core : EntityComponent
{
    public void Start()
    {
        EntityStats stats = this.entity.getStats();

        stats.maxHealth += 50;
        stats.maxHunger += 20;
        stats.currentHealth = stats.maxHealth;
        // stats.currentHunger = stats.maxHunger;

        if (this.transform.localPosition.x == 0 && this.transform.localPosition.y > 0)
        {
            CapsuleCollider2D collider = this.entity.GetComponent<CapsuleCollider2D>();
            Vector2 offset = collider.offset;
            Vector2 size = collider.size;

            offset.y += this.positionOffset.y / 2;
            collider.offset = offset;

            size.y += this.positionOffset.y;
            collider.size = size;
        }
    }

    override public void beforeDestroy()
    {
        EntityStats stats = this.entity.getStats();

        stats.maxHealth -= 50;
        stats.maxHunger -= 20;
    }
}