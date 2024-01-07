

using UnityEngine;

public class SearchFood : EntityBehaviour
{
    private GameObject food;

    private GameObject home;

    private Vector2 desperateRunDirection;

    private Entity attackTarget;

    private float secondsSinceAttacking = 0;

    public SearchFood(Entity entity) : base(entity)
    {
        this.updateCommitmentValue();
        this.desperateRunDirection = Helpers.generateRandomDirection();
    }

    public override void FixedUpdate()
    {
        this.move();
    }

    public override void Update()
    {
        food = entity.getNearby().getClosestFood();
        this.updateCommitmentValue();
    }

    override public void OnCollisionEnter2D(Collision2D collision, EntityComponent entityComponent = null)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            Food food = collision.gameObject.GetComponent<Food>();
            food.destroyTimer += 10;

            Vector2 pushDirection = (food.transform.position - entity.transform.position).normalized;

            food.GetComponent<Rigidbody2D>().AddForce(pushDirection * 0.2f, ForceMode2D.Impulse);

            int nutrition = 10;
            food.nutrition -= nutrition;
            entity.eat(nutrition);

            return;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            this.desperateRunDirection = Helpers.generateRandomDirection();
            return;
        }

        if (!this.attackTarget)
        {
            return;
        }

        Entity otherEntity = Combat.getEntityFromCollision(collision);

        if (otherEntity != attackTarget)
        {
            return;
        }

        Combat.takeHit(entity, otherEntity);

        if (otherEntity == null)
        {
            entity.scanNearby();
            food = entity.getNearby().getClosestFood();
            updateCommitmentValue();
        }
    }

    private void move()
    {
        if (attackTarget)
        {
            float distanceToTarget = Vector2.Distance(attackTarget.transform.position, entity.transform.position);
            float distanceToFood = Mathf.Infinity;

            if (food != null)
            {
                distanceToFood = Vector2.Distance(food.transform.position, entity.transform.position);
            }

            if (distanceToFood > distanceToTarget)
            {
                entity.triggerGrab(attackTarget.gameObject);
                this.attemptAttack();
                return;
            }
        }

        if (food != null)
        {
            Helpers.moveTowards(entity, food);
            entity.triggerGrab(food);
            return;
        }

        if (entity.getStats().getHungerPercentage() <= 15)
        {
            Entity closestEntity = entity.getStats().loneWolf ? entity.getNearby().getClosestEntity() : entity.getNearby().getClosestEnemy();

            if (entity.getStats().hungerBehaviour == HungerBehaviour.Attack && closestEntity != null)
            {
                this.attackTarget = closestEntity;
                return;
            }

            Helpers.moveTowards(entity, desperateRunDirection);
            return;
        }

        if (entity.getHome() != null)
        {
            if (Vector2.Distance(entity.transform.position, entity.getHome().transform.position) >= 1)
            {
                Helpers.moveTowards(entity, entity.getHome());
            }
            else
            {
                Helpers.rotateTowards(entity, entity.getHome());
            }

            return;
        }

        // if (entity.getNearby().getClosestEntity())
        // {
        //     Helpers.moveTowards(entity, entity.getNearby().getClosestEntity().gameObject);
        //     return;
        // }


        Helpers.moveTowards(entity, desperateRunDirection);
    }

    private void attemptAttack()
    {
        entity.triggerAttack(attackTarget);
        secondsSinceAttacking += Time.deltaTime;
        Helpers.moveTowards(entity, attackTarget.gameObject);

        if (secondsSinceAttacking >= 15)
        {
            attackTarget = null;
            secondsSinceAttacking = 0;
        }
    }

    private void updateCommitmentValue()
    {
        float hunger = entity.getStats().getHungerPercentage();

        if (hunger <= 50)
        {
            this.commitmentValue = 10;
        }
        else if (hunger <= 80)
        {
            this.commitmentValue = 5;
        }
        else
        {
            this.commitmentValue = 0;
        }
    }


}