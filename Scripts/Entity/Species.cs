using UnityEngine;

public class Species
{
    private string id;

    private Color color;

    public Species()
    {
        this.color = this.color = Game.getEntities().Count == 0 ? Color.white : Helpers.getRandomColor(); ;
        this.id = Helpers.generateUniqueID();
    }

    public string getId()
    {
        return this.id;
    }

    public Color getColor()
    {
        return this.color;
    }
}