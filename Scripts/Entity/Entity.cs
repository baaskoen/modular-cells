using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public SpriteRenderer[] visuals;

    public List<EntityComponent> entityComponents = new List<EntityComponent>();

    public LayerMask nearbyLayer;

    protected EntityStats stats;

    protected Timer behaviourTimer, lifeSpanTimer, hungerTimer, regenTimer;

    protected new Rigidbody2D rigidbody;

    protected EntityBehaviour currentBehaviour, nextBehaviour;

    protected NearbyList nearby;

    protected float secondsSinceLastNearbyCache = 0;

    protected GameObject home = null;

    protected bool hasBirthed = false;

    public virtual void Start()
    {
        if (this.stats == null)
        {
            stats = new EntityStats();
        }

        Game.addEntity(this);

        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.mass = 0.2f;
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        collider.offset = Vector2.zero;
        collider.size = new Vector2(0.28f, 0.4f);

        initTimers();

        currentBehaviour = new Idle(this);

        foreach (SpriteRenderer renderer in this.visuals)
        {
            renderer.color = stats.species.getColor();
        }

        foreach (EntityComponent component in this.entityComponents)
        {
            if (component.hasSlots)
            {
                rigidbody.mass += 0.05f;
            }

            component.GetComponent<SpriteRenderer>().color = stats.species.getColor();
        }

        this.scanNearby();
    }

    void FixedUpdate()
    {
        this.currentBehaviour.FixedUpdate();
    }

    void Update()
    {
        this.currentBehaviour.Update();
        this.secondsSinceLastNearbyCache += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.currentBehaviour != null)
        {
            this.currentBehaviour.OnCollisionEnter2D(collision, null);
        }
    }

    public void addEntityComponent(EntityComponent component)
    {
        this.entityComponents.Add(component);
    }

    public Species getSpecies()
    {
        return this.stats.species;
    }

    public List<EntityComponent> getEntityComponents()
    {
        return this.entityComponents;
    }

    private void initTimers()
    {
        behaviourTimer = new Timer(1, () =>
        {
            if (nextBehaviour == null)
            {
                nextBehaviour = EntityBehaviour.determineNextBehaviour(this);
            }

            string current = currentBehaviour.GetType().ToString();

            if (nextBehaviour.GetType() != currentBehaviour.GetType())
            {
                currentBehaviour.Stop();
                currentBehaviour = nextBehaviour;
                Game.triggerEntityEvent(new EntityEvent(this, EntityEventType.BehaviourChange));
            }

            nextBehaviour = null;
        }, true);

        lifeSpanTimer = new Timer(1, () =>
        {
            stats.currentLifeSpan += 1;
            this.onLifeSpanTimerElapsed();
        }, true);

        hungerTimer = new Timer(3, () =>
        {
            int nutrition = 2;
            stats.currentHunger = Mathf.Max(0, stats.currentHunger - nutrition);
            Game.triggerEntityEvent(new EntityEvent(this, EntityEventType.ReduceHunger));

            if (stats.currentHunger <= 0)
            {
                this.takeDamage(Mathf.RoundToInt(stats.maxHealth / 20));
            }
        }, true);

        regenTimer = new Timer(2, () =>
        {
            this.regenHealth(1);
        }, true);
    }

    virtual protected void onLifeSpanTimerElapsed()
    {
        if (!hasBirthed && stats.getHungerPercentage() >= 50 && stats.getHealthPercentage() >= 50 && stats.currentLifeSpan > stats.birthTime)
        // if (stats.getHungerPercentage() >= 50 && Random.value <= 0.1f && Game.getEntities().Count <= 250)
        {
            EntityBehaviour.birth(this, Random.Range(2, 4));
            hasBirthed = true;
        }
    }

    public EntityStats getStats()
    {
        return this.stats;
    }

    public void setStats(EntityStats stats)
    {
        this.stats = stats;
    }

    public EntityBehaviour getCurrentBehaviour()
    {
        return this.currentBehaviour;
    }

    public NearbyList getNearby()
    {
        if (this.secondsSinceLastNearbyCache >= 0.5f)
        {
            this.scanNearby();
            this.secondsSinceLastNearbyCache = 0;
        }

        return this.nearby;
    }

    public Rigidbody2D getRigidbody()
    {
        return this.rigidbody;
    }

    public GameObject getHome()
    {
        return this.home;
    }

    public void triggerAttack(Entity target)
    {
        foreach (EntityComponent component in this.entityComponents)
        {
            component.triggerAttack(target);
        }
    }

    public void triggerGrab(GameObject target)
    {
        foreach (EntityComponent component in this.entityComponents)
        {
            component.triggerGrab(target);
        }
    }

    public void takeDamage(int damage, Entity causer = null)
    {
        this.stats.currentHealth = Mathf.Max(0, stats.currentHealth - damage);

        float damagePercentage = (damage / stats.maxHealth) * 100;

        Game.triggerEntityEvent(new EntityEvent(
            this,
            EntityEventType.ReduceHealth,
            "-" + damage,
            Color.white,
            damagePercentage >= 50 ? 26 : (damagePercentage <= 25 ? 18 : 22)
        ));

        if (causer != null)
        {
            this.nextBehaviour = new ReactToAttacker(this, causer);
        }

        if (this.stats.currentHealth == 0)
        {
            this.die();
        }
    }

    public void regenHealth(int amount)
    {
        int oldHealth = stats.currentHealth;
        this.stats.currentHealth = Mathf.Min(stats.maxHealth, stats.currentHealth + amount);

        EntityEvent entityEvent = new EntityEvent(this, EntityEventType.IncreaseHealth);

        if (oldHealth != stats.currentHealth && amount >= 5)
        {
            entityEvent.setFloatingText("+" + amount, Color.green);
        }

        Game.triggerEntityEvent(entityEvent);
    }

    public void eat(int nutrition)
    {
        this.stats.currentHunger = Mathf.Min(stats.maxHunger, stats.currentHunger + nutrition);

        Game.triggerEntityEvent(new EntityEvent(this, EntityEventType.IncreaseHunger));
    }

    public void die()
    {
        this.hungerTimer.Destroy();
        this.lifeSpanTimer.Destroy();
        this.behaviourTimer.Destroy();
        this.regenTimer.Destroy();

        for (int i = 0; i <= (this.stats.currentHunger / 30); i++)
        {
            GameObject foodGo = Instantiate(Resources.Load<GameObject>("Food_2"));
            foodGo.transform.position = transform.position;
            Vector2 direction = Helpers.generateRandomDirection();

            Rigidbody2D r = foodGo.GetComponent<Rigidbody2D>();
            r.AddForce(direction * Random.Range(10, 40));
        }

        Game.triggerEntityEvent(new EntityEvent(this, EntityEventType.Death));
        Game.removeEntity(this);
        Destroy(gameObject);
    }

    public void scanNearby()
    {
        this.nearby = new NearbyList(this, 10, nearbyLayer);

        if (nearby.getClosestFoodSpawner())
        {
            this.home = nearby.getClosestFoodSpawner();
        }
    }
}
