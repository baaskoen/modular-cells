using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBomb : Projectile
{
    public Explosion explosionPrefab;

    protected override void onDestinationReached()
    {
        Explosion explosion = Instantiate<Explosion>(explosionPrefab);

        explosion.radius = this.explosionRadius;
        explosion.renderer.color = GetComponent<SpriteRenderer>().color;
        explosion.transform.position = transform.position;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, this.explosionRadius);

        foreach (Collider2D collider in colliders)
        {
            if (!collider.gameObject.CompareTag("Entity"))
            {
                continue;
            }

            Entity entity = Game.getEntityById(collider.gameObject.GetInstanceID().ToString());

            if (entity == this.causer || entity == null)
            {
                continue;
            }


            if (this.causer == null || entity == null || entity.getRigidbody() == null)
            {
                continue;
            }

            Vector2 pushDirection = (entity.transform.position - causer.transform.position).normalized;
            entity.getRigidbody().AddForce(pushDirection * 2f, ForceMode2D.Impulse);
            entity.takeDamage(entity.gameObject == targetGameobject ? 25 : 15);
        }

        GameObject.Destroy(gameObject);
    }
}
