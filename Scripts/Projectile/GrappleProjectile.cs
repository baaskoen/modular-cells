using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleProjectile : Projectile
{
    public List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    public override void Start()
    {
        base.Start();

        foreach (SpriteRenderer renderer in this.renderers)
        {
            renderer.color = causer.getSpecies().getColor();
        }

        Vector3 direction = (targetGameobject.transform.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
    }

    protected override void onDestinationReached()
    {
        if (targetGameobject && causer)
        {
            Vector2 direction = (targetGameobject.transform.position - causer.transform.position).normalized;
            causer.getRigidbody().AddForce(direction * 2, ForceMode2D.Impulse);
        }



        GameObject.Destroy(gameObject);
    }
}
