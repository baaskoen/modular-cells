

using UnityEngine;

public class ReactToAttacker : EntityBehaviour
{

    private Entity attacker;

    private bool retaliating = false;

    private float retaliatingTimer = 0;

    public ReactToAttacker(Entity entity, Entity attacker) : base(entity)
    {
        this.attacker = attacker;
        this.updateCommitmentValue();
    }

    public override void Update()
    {
        retaliatingTimer -= Time.deltaTime;
        this.updateCommitmentValue();

        if (!attacker)
        {
            return;
        }

        if (retaliatingTimer <= 0)
        {
            retaliating = entity.getStats().panicBehaviour == PanicBehaviour.Retaliate || entity.getStats().currentHealth >= attacker.getStats().currentHealth;
            retaliatingTimer = 0.5f;
        }


        entity.triggerAttack(attacker);
    }

    public override void FixedUpdate()
    {
        if (!attacker)
        {
            return;
        }

        if (retaliating)
        {
            entity.triggerGrab(attacker.gameObject);
            Helpers.moveTowards(entity, attacker.gameObject);
        }
        else
        {
            Helpers.moveTowards(entity, (entity.transform.position - attacker.transform.position).normalized);
        }
    }

    override public void OnCollisionEnter2D(Collision2D collision, EntityComponent entityComponent = null)
    {
        if (!this.attacker || !retaliating)
        {
            return;
        }

        Entity otherEntity = Combat.getEntityFromCollision(collision);

        if (otherEntity != this.attacker)
        {
            return;
        }

        Combat.takeHit(entity, otherEntity);

        if (this.attacker == null)
        {
            entity.scanNearby();
        }
    }

    private void updateCommitmentValue()
    {
        if (this.attacker != null && Vector2.Distance(entity.transform.position, attacker.transform.position) < 10)
        {
            this.commitmentValue = 20;
            return;
        }

        this.commitmentValue = 0;
        return;
    }
}