

using UnityEngine;

public class Idle : EntityBehaviour
{

    private float currentDuration = 0f;
    private Vector2 currentDirection;

    public Idle(Entity entity) : base(entity)
    {
        this.currentDirection = Helpers.generateRandomDirection();
    }

    public override void FixedUpdate()
    {
        Helpers.moveTowards(entity, currentDirection);
    }

    public override void Update()
    {
        currentDuration -= Time.deltaTime;

        if (currentDuration <= 0f)
        {
            this.entity.getNearby();
            this.currentDirection = this.determineDirection();
            currentDuration = Random.Range(1, 10);
        }
    }

    override public void OnCollisionEnter2D(Collision2D collision, EntityComponent entityComponent = null)
    {

    }

    private Vector2 determineDirection()
    {
        GameObject home = entity.getHome();

        int maxDistance = entity.getStats().idleBehaviour == IdleBehaviour.Roam ? 10 : 4;

        if (home && Vector2.Distance(entity.transform.position, home.transform.position) >= maxDistance)
        {
            return (home.transform.position - entity.transform.position).normalized;
        }

        return Helpers.generateRandomDirection();
    }
}