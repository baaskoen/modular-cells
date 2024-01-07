

using UnityEngine;

public class MassGenocide : EntityBehaviour
{
    Entity target;

    public MassGenocide(Entity entity) : base(entity)
    {

    }

    public override void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        entity.triggerAttack(target);
        Helpers.moveTowards(entity, target.gameObject);
    }

    public override void Update()
    {
        this.updateCommitmentValue();

        if (target)
        {
            return;
        }

        target = entity.getNearby().getClosestFriend();
    }

    override public void OnCollisionEnter2D(Collision2D collision, EntityComponent entityComponent = null)
    {
        Entity otherEntity = Combat.getEntityFromCollision(collision);

        if (otherEntity != this.target || otherEntity == null || entity == null)
        {
            return;
        }

        Combat.takeHit(entity, otherEntity);
    }

    private void updateCommitmentValue()
    {
        if (Game.getEntities().Count <= 40 && Game.getSpeciesData().Count >= 5)
        {
            this.commitmentValue = 0;
            return;
        }

        this.commitmentValue = 10;
    }
}