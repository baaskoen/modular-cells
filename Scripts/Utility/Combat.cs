using UnityEngine;

public static class Combat
{
    public static Entity getEntityFromCollision(Collision2D collision)
    {
        Entity otherEntity = null;

        if (collision.gameObject.CompareTag("Entity"))
        {
            otherEntity = Game.getEntityById(collision.gameObject.GetInstanceID().ToString());
        }

        if (collision.gameObject.CompareTag("EntityComponent"))
        {
            otherEntity = collision.gameObject.GetComponent<EntityComponent>().getEntity();
        }

        return otherEntity;
    }

    public static void takeHit(Entity entity, Entity otherEntity)
    {
        Vector2 pushDirection = Vector2.zero;

        if (entity.getRigidbody().mass >= otherEntity.getRigidbody().mass)
        {
            pushDirection = (otherEntity.transform.position - entity.transform.position).normalized;
            otherEntity.getRigidbody().AddForce(pushDirection * 1.5f, ForceMode2D.Impulse);
        }
        else
        {
            pushDirection = (entity.transform.position - otherEntity.transform.position).normalized;
            entity.getRigidbody().AddForce(pushDirection * 1.5f, ForceMode2D.Impulse);
        }

        otherEntity.takeDamage(entity.getStats().bumpDamage, entity);
    }
}