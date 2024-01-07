
using UnityEngine;

public class EntityStats
{
    public Species species;

    public int currentHealth, maxHealth = 100;

    public int currentHunger, maxHunger = 100;

    public int birthTime = 100;

    public float currentLifeSpan;

    public float movementSpeed = 1.5f, rotationSpeed = 5f;

    public int bumpDamage = 10;

    public HungerBehaviour hungerBehaviour;

    public IdleBehaviour idleBehaviour;

    public PanicBehaviour panicBehaviour;

    public bool loneWolf;

    public EntityStats()
    {
        this.species = new Species();

        this.hungerBehaviour = Helpers.getRandomEnumValue<HungerBehaviour>();
        this.idleBehaviour = Helpers.getRandomEnumValue<IdleBehaviour>();
        this.panicBehaviour = Helpers.getRandomEnumValue<PanicBehaviour>();

        this.resetNumericValues();
    }

    public EntityStats(Entity entity)
    {
        EntityStats stats = entity.getStats();

        this.species = stats.species;
        this.hungerBehaviour = stats.hungerBehaviour;
        this.idleBehaviour = stats.idleBehaviour;
        this.panicBehaviour = stats.panicBehaviour;
        this.loneWolf = stats.loneWolf;

        this.resetNumericValues();
    }

    public void resetNumericValues()
    {
        this.currentLifeSpan = 0;
        this.currentHunger = this.maxHunger;
        // this.currentHunger = 10;
        // this.loneWolf = Helpers.getRandomBool();
        this.loneWolf = true;
        this.currentHealth = this.maxHealth;
        this.birthTime = Random.Range(120, 241);
    }

    public float getHealthPercentage()
    {
        return ((float)this.currentHealth / (float)this.maxHealth) * 100;
    }

    public float getHungerPercentage()
    {
        return ((float)this.currentHunger / (float)this.maxHunger) * 100;
    }
}
