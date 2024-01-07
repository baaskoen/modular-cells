using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityComponent : MonoBehaviour
{
    protected Entity entity;

    public virtual void Awake()
    {
        entity = this.GetComponentInParent<Entity>();
    }

    public bool hasSlots = false;

    public bool attachableTop = true, attachableRight = true, attachableBottom = true, attachableLeft = false;

    public Vector2 positionOffset = Vector2.zero;

    public float rotationOffset = 90;

    public Vector2 attachPosition = Vector2.zero;

    public Entity getEntity()
    {
        return entity;
    }

    public virtual void triggerAttack(Entity target)
    {

    }

    public virtual void triggerGrab(GameObject target)
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.entity.getCurrentBehaviour() != null)
        {
            this.entity.getCurrentBehaviour().OnCollisionEnter2D(collision, this);
        }
    }

    public static string getRandom()
    {
        List<string> componentList = new List<string>() {
            "Core",
            "Horn",
            "Leg",
            "FireBomber",
            "Hook",
            "Grapple"
        };

        return Helpers.getRandomFromList<string>(componentList);
    }

    virtual public void beforeDestroy()
    {

    }

    public static void attach(EntityComponentAttachment attachment)
    {
        Vector2 location = attachment.attachToParent.transform.localPosition;

        if (attachment.attachDirection == Vector2.up)
        {
            location.y -= attachment.toAttach.positionOffset.y;
            attachment.toAttach.transform.localEulerAngles = new Vector3(0, 0, 180);
        }

        if (attachment.attachDirection == Vector2.right)
        {
            location.x += attachment.toAttach.positionOffset.x;
            attachment.toAttach.transform.localEulerAngles = new Vector3(0, 0, -attachment.toAttach.rotationOffset);
        }

        if (attachment.attachDirection == Vector2.down)
        {
            location.y += attachment.toAttach.positionOffset.y;
        }

        if (attachment.attachDirection == Vector2.left)
        {
            location.x -= attachment.toAttach.positionOffset.x;
            attachment.toAttach.transform.localEulerAngles = new Vector3(0, 0, attachment.toAttach.rotationOffset);
        }

        attachment.toAttach.transform.localPosition = location;
        attachment.toAttach.entity.addEntityComponent(attachment.toAttach);
        attachment.toAttach.attachPosition = attachment.attachPosition;

        Game.triggerEntityEvent(new EntityEvent(attachment.toAttach.entity, EntityEventType.AttachComponent));
    }

    public static void detach(EntityComponent component)
    {
        component.beforeDestroy();
        component.entity.getEntityComponents().Remove(component);

        Game.triggerEntityEvent(new EntityEvent(component.entity, EntityEventType.DetachComponent));
        Destroy(component.gameObject);
    }

    public static EntityComponentAttachment findAttachComponent(Entity entity, EntityComponent toAttach)
    {
        List<Vector2> availablePositions = new List<Vector2>();
        List<EntityComponent> entityComponents = entity.getEntityComponents();
        List<EntityComponent> componentsWithSlots = entityComponents.FindAll((EntityComponent component) => component.hasSlots);

        List<Vector2> directions = new List<Vector2>();

        if (toAttach.attachableTop)
        {
            directions.Add(Vector2.up);
        }

        if (toAttach.attachableBottom)
        {
            directions.Add(Vector2.down);
        }

        if (toAttach.attachableRight)
        {
            directions.Add(Vector2.right);
        }

        if (toAttach.attachableLeft)
        {
            directions.Add(Vector2.left);
        }

        Helpers.shuffleList(directions);

        // Try to prefer symmetry
        // int componentsLeft = entityComponents.Where(c => c.attachPosition.x < 0 && c.hasSlots).Count();
        // int componentsRight = entityComponents.Where(c => c.attachPosition.x > 0 && c.hasSlots).Count();
        // int indexOfLeft = directions.IndexOf(Vector2.left);
        // int indexOfRight = directions.IndexOf(Vector2.right);

        // if (componentsLeft < componentsRight)
        // {
        //     directions.Remove(Vector2.left);
        //     directions.Insert(indexOfRight, Vector2.left);
        // }

        // if (componentsRight < componentsLeft)
        // {
        //     directions.Remove(Vector2.right);
        //     directions.Insert(indexOfLeft, Vector2.right);
        // }

        foreach (EntityComponent component in componentsWithSlots)
        {
            foreach (Vector2 direction in directions)
            {
                Vector2 next = component.attachPosition + direction;

                // We don't want to attach close to main core
                if (toAttach.hasSlots && next.y >= 0 && next.x < 3 && next.x > -3)
                {
                    continue;
                }

                if (entityComponents.Find((EntityComponent c) => c.attachPosition == next))
                {
                    continue;
                }

                EntityComponentAttachment location = new EntityComponentAttachment();
                location.toAttach = toAttach;
                location.attachToParent = component;
                location.attachPosition = next;
                location.attachDirection = direction;
                return location;
            }
        }

        return null;
    }
}
