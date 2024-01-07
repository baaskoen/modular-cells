using UnityEngine;

public class EntityComponentAttachment
{
    public EntityComponent toAttach { get; set; }
    public EntityComponent attachToParent { get; set; }

    public Vector2 attachPosition { get; set; }

    public Vector2 attachDirection { get; set; }
}