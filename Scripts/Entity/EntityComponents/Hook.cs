using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : EntityComponent
{
    private float cooldown = 6;

    private float currentCooldown;

    private float range = 8;

    public HookProjectile projectile;

    void Start()
    {
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

        HookProjectile gameObject = Instantiate<HookProjectile>(projectile);
        gameObject.transform.position = this.transform.position;
        gameObject.GetComponent<SpriteRenderer>().color = entity.getSpecies().getColor();
        gameObject.setCauser(entity);
        gameObject.setTarget(target.gameObject);
    }
}
