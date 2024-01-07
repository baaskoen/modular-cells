
using System.Collections.Generic;
using UnityEngine;

abstract public class EntityBehaviour
{
    protected readonly Entity entity;

    protected int commitmentValue = 0;

    public EntityBehaviour(Entity entity)
    {
        this.entity = entity;
    }

    virtual public void Update()
    {

    }

    virtual public void FixedUpdate()
    {

    }

    virtual public void Stop()
    {

    }

    virtual public void OnCollisionEnter2D(Collision2D collision, EntityComponent entityComponent = null)
    {

    }

    public static EntityBehaviour determineNextBehaviour(Entity entity)
    {
        if (Game.getSpeciesData().Count == 1 && Game.getEntities().Count > 50)
        {
            return new MassGenocide(entity);
        }

        EntityStats stats = entity.getStats();
        EntityBehaviour currentBehaviour = entity.getCurrentBehaviour();
        EntityBehaviour next;

        if (stats.getHungerPercentage() <= 50)
        {
            next = new SearchFood(entity);
        }
        else
        {
            next = new Idle(entity);
        }

        if (currentBehaviour == null)
        {
            return next;
        }

        return ((next.getCommitmentValue() + 1) <= currentBehaviour.getCommitmentValue()) ? currentBehaviour : next;
    }

    public static void birth(Entity entity, int amount, float mutationChance = 0.1f)
    {
        Game.triggerEntityEvent(new EntityEvent(entity, EntityEventType.Birth));
        bool mutated = false;

        for (int i = 0; i < amount; i++)
        {
            Entity baby = GameObject.Instantiate<Entity>(entity);
            baby.name = "Baby";
            baby.transform.position = entity.transform.position;

            bool shouldMutate = Random.value < mutationChance && !mutated;

            if (shouldMutate)
            {
                Game.triggerEntityEvent(new EntityEvent(entity, EntityEventType.Mutate));
                EntityStats stats = new EntityStats();
                baby.setStats(stats);
                mutate(baby);
                mutated = true;
            }
            else
            {
                baby.setStats(new EntityStats(entity));
            }
        }
    }

    public int getCommitmentValue()
    {
        return this.commitmentValue;
    }

    public static EntityComponent mutate(Entity entity, GameObject prefab = null)
    {
        GameObject gameObject = null;

        if (prefab == null)
        {
            string prefabName = EntityComponent.getRandom();

            gameObject = GameObject.Instantiate<GameObject>(
                Resources.Load("Entity/EntityComponents/" + prefabName) as GameObject,
                entity.transform
            );
            gameObject.name = prefabName;
        }
        else
        {
            gameObject = GameObject.Instantiate(prefab, entity.transform) as GameObject;
        }

        EntityComponent entityComponent = gameObject.GetComponent<EntityComponent>();

        EntityComponentAttachment attachment = EntityComponent.findAttachComponent(entity, entityComponent);

        if (attachment == null)
        {
            GameObject.Destroy(entityComponent.gameObject);

            List<EntityComponent> entityComponents = entity.getEntityComponents().FindAll((EntityComponent component) => !component.hasSlots);

            if (entityComponents.Count > 0)
            {
                EntityComponent.detach(Helpers.getRandomFromList<EntityComponent>(entityComponents));
            }

            return null;
        }

        EntityComponent.attach(attachment);
        return attachment.toAttach;
    }
}
