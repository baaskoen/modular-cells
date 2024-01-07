using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBomber : EntityComponent
{
    private float cooldown = 10;

    private float range = 5;

    private float currentCooldown;

    public FireBomb projectile;

    void Start()
    {
        this.currentCooldown = 3;
    }

    void Update()
    {
        this.currentCooldown -= Time.deltaTime;
    }

    public override void triggerAttack(Entity target)
    {
        if (this.currentCooldown > 0 || Vector2.Distance(entity.transform.position, target.transform.position) > range)
        {
            return;
        }


        this.currentCooldown = this.cooldown;

        FireBomb gameObject = Instantiate<FireBomb>(projectile);
        gameObject.transform.position = this.transform.position;
        gameObject.setCauser(entity);
        gameObject.setTarget(target.gameObject);
    }
}
