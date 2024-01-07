using UnityEngine;

public class EntityEvent
{
    private Entity entity;
    private EntityEventType entityEventType;

    private string floatingText;
    private Color floatingTextColor;
    private int fontSize = 16;

    public EntityEvent(Entity entity, EntityEventType entityEventType)
    {
        this.entity = entity;
        this.entityEventType = entityEventType;
    }

    public EntityEvent(Entity entity, EntityEventType entityEventType, string floatingText, int fontSize = 16)
    {
        this.entity = entity;
        this.entityEventType = entityEventType;
        this.floatingText = floatingText;
        this.floatingTextColor = Color.white;
        this.fontSize = fontSize;
    }

    public EntityEvent(Entity entity, EntityEventType entityEventType, string floatingText, Color floatingTextColor, int fontSize = 16)
    {
        this.entity = entity;
        this.entityEventType = entityEventType;
        this.floatingText = floatingText;
        this.floatingTextColor = floatingTextColor;
        this.fontSize = fontSize;
    }

    public void setFloatingText(string text, Color color, int fontSize = 16)
    {
        this.floatingText = text;
        this.floatingTextColor = color;
        this.fontSize = fontSize;
    }

    public Entity getEntity()
    {
        return entity;
    }

    public EntityEventType getEventType()
    {
        return entityEventType;
    }

    public string getFloatingText()
    {
        return this.floatingText;
    }

    public Color getFloatingTextColor()
    {
        return this.floatingTextColor;
    }

    public int getFontSize()
    {
        return this.fontSize;
    }
}

public enum EntityEventType
{
    ReduceHealth,
    IncreaseHealth,
    ReduceHunger,
    IncreaseHunger,
    Birth,
    Mutate,
    AttachComponent,
    DetachComponent,
    Death,
    BehaviourChange
}