using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEntity : Entity
{
    public bool birthEverySecond = false;

    public GameObject appendedComponent1;
    public GameObject appendedComponent2;

    public int startHunger;

    public HungerBehaviour behaviour = HungerBehaviour.Attack;

    override public void Start()
    {
        base.Start();


        this.stats.hungerBehaviour = behaviour;

        if (startHunger > 0)
        {
            this.stats.currentHunger = startHunger;
        }

        if (this.appendedComponent1)
        {
            EntityComponent attached = EntityBehaviour.mutate(this, this.appendedComponent1);

            if (attached)
            {
                if (attached.hasSlots)
                {
                    rigidbody.mass += 0.05f;
                }

                attached.GetComponent<SpriteRenderer>().color = stats.species.getColor();
            }
        }

        if (this.appendedComponent2)
        {
            EntityComponent attached = EntityBehaviour.mutate(this, this.appendedComponent2);

            if (attached)
            {
                if (attached.hasSlots)
                {
                    rigidbody.mass += 0.05f;
                }

                attached.GetComponent<SpriteRenderer>().color = stats.species.getColor();
            }
        }

    }

    override protected void onLifeSpanTimerElapsed()
    {
        if (birthEverySecond)
        {
            EntityBehaviour.birth(this, 1, 1);
            this.hasBirthed = true;
            this.die();
            return;
        }

        base.onLifeSpanTimerElapsed();
    }
}
