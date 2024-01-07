using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : EntityComponent
{
    private float cooldown = 6;

    private float currentCooldown;

    private float range = 8;

    public GrappleProjectile projectile;

    public List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    void Start()
    {
        foreach (SpriteRenderer renderer in this.renderers)
        {
            renderer.color = entity.getSpecies().getColor();
        }

        this.currentCooldown = 0;
    }

    void Update()
    {
        this.currentCooldown -= Time.deltaTime;
    }


    public override void triggerGrab(GameObject target)
    {
        if (this.currentCooldown > 0 || Vector2.Distance(entity.transform.position, target.transform.position) > range)
        {
            return;
        }

        this.currentCooldown = this.cooldown;

        GrappleProjectile gameObject = Instantiate<GrappleProjectile>(projectile);
        gameObject.transform.position = this.transform.position;
        gameObject.GetComponent<SpriteRenderer>().color = entity.getSpecies().getColor();
        gameObject.setCauser(entity);
        gameObject.setTarget(target.gameObject);
    }
}
