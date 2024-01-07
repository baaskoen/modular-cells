using System.Collections.Generic;
using UnityEngine;

public class NearbyList
{
    private readonly Entity entity;
    private readonly float range;

    private GameObject closestFoodSpawner, closestFood;
    private Entity closestEntity, closestFriend, closestEnemy;

    public NearbyList(Entity entity, float maxDistance, LayerMask layer)
    {
        this.entity = entity;
        this.range = maxDistance;

        Vector2 fromPosition = entity.transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(fromPosition, maxDistance, layer);
        float closestFoodSpawnerDistance = maxDistance;
        float closestEntityDistance = maxDistance;
        float closestFoodDistance = maxDistance;
        float closestFriendDistance = maxDistance;
        float closestEnemyDistance = maxDistance;

        foreach (Collider2D collider in colliders)
        {
            GameObject go = collider.gameObject;
            Vector2 toPosition = go.transform.position;
            float distance = Vector2.Distance(fromPosition, toPosition);

            if (go.CompareTag("Entity"))
            {
                if (go.GetInstanceID() == entity.gameObject.GetInstanceID())
                {
                    continue;
                }

                Entity other = Game.getEntityById(go.gameObject.GetInstanceID().ToString());

                if (!other)
                {
                    continue;
                }

                if (distance < closestEntityDistance)
                {
                    closestEntity = other;
                    closestEntityDistance = distance;
                }

                if (other.getSpecies().getId() != entity.getSpecies().getId() && distance < closestEnemyDistance)
                {
                    closestEnemy = other;
                    closestEnemyDistance = distance;
                }

                if (other.getSpecies().getId() == entity.getSpecies().getId() && distance < closestFriendDistance)
                {
                    closestFriend = other;
                    closestFriendDistance = distance;
                }

                continue;
            }

            if (go.CompareTag("Food"))
            {
                if (distance < closestFoodDistance)
                {
                    closestFood = go;
                    closestFoodDistance = distance;
                }

                continue;
            }

            if (go.CompareTag("FoodSpawner"))
            {
                if (distance < closestFoodSpawnerDistance)
                {
                    closestFoodSpawner = go;
                    closestFoodSpawnerDistance = distance;
                }

                continue;
            }
        }
    }

    public GameObject getClosestFoodSpawner()
    {
        return this.closestFoodSpawner;
    }

    public Entity getClosestEntity()
    {
        return this.closestEntity;
    }

    public GameObject getClosestFood()
    {
        return this.closestFood;
    }


    public Entity getClosestFriend()
    {
        return this.closestFriend;
    }

    public Entity getClosestEnemy()
    {
        return this.closestEnemy;
    }
}