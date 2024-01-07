using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private Canvas gameCanvas;

    [SerializeField]
    private TextMeshProUGUI topBarText;

    [SerializeField]
    private FloatingText floatingText;

    [SerializeField]
    private RectTransform speciesDisplayList;

    private static Game game;

    private List<Timer> timers = new List<Timer>();

    private Dictionary<string, Entity> entities = new Dictionary<string, Entity>();

    private Dictionary<string, (int, Species)> speciesData = new Dictionary<string, (int, Species)>();

    void Awake()
    {
        game = this;

        Application.targetFrameRate = 90;
    }

    public static void addTimer(Timer timer)
    {
        game.timers.Add(timer);
    }

    public static void deleteTimer(Timer timer)
    {
        game.timers.Remove(timer);
    }

    public static void addEntity(Entity entity)
    {
        game.entities[entity.gameObject.GetInstanceID().ToString()] = entity;

        game.updateSpeciesList();
    }

    public static void removeEntity(Entity entity)
    {
        game.entities.Remove(entity.gameObject.GetInstanceID().ToString());

        game.updateSpeciesList();
    }

    public static Dictionary<string, Entity> getEntities()
    {
        return game.entities;
    }

    public static Entity getEntityById(string id)
    {
        if (!game.entities.ContainsKey(id))
        {
            return null;
        }

        return game.entities[id];
    }

    public static Dictionary<string, (int, Species)> getSpeciesData()
    {
        return game.speciesData;
    }

    public static Canvas getGameCanvas()
    {
        return game.gameCanvas;
    }

    void Start()
    {
        new Timer(1, () =>
        {
            game.topBarText.text = "Entities: " + game.entities.Count + ", " +
                string.Format("FPS: {0:0.}", (1.0f / Time.deltaTime) * Time.timeScale);
        }, true);
    }

    void Update()
    {
        this.updateTimers();
    }

    public static void triggerEntityEvent(EntityEvent entityEvent)
    {
        if (entityEvent.getFloatingText() == null)
        {
            return;
        }

        FloatingText instance = Instantiate<FloatingText>(game.floatingText);

        instance.transform.SetParent(game.gameCanvas.transform);
        instance.target = entityEvent.getEntity().transform;
        instance.textComponent.SetText(entityEvent.getFloatingText());
        instance.textComponent.color = entityEvent.getFloatingTextColor();
        instance.fontSize = entityEvent.getFontSize();
    }

    private void updateTimers()
    {
        for (int i = 0; i < game.timers.Count; i++)
        {
            timers[i].Tick();
        }
    }

    private void updateSpeciesList()
    {
        speciesData = new Dictionary<string, (int, Species)>();

        foreach (Entity entity in game.entities.Values)
        {
            if (!speciesData.ContainsKey(entity.getSpecies().getId()))
            {
                speciesData.Add(entity.getSpecies().getId(), (1, entity.getSpecies()));

                continue;
            }

            (int count, Species species) = speciesData[entity.getSpecies().getId()];
            speciesData[entity.getSpecies().getId()] = (count + 1, species);
        }

        List<KeyValuePair<string, (int, Species)>> sortedList = speciesData.ToList();

        sortedList.Sort((pair1, pair2) => pair2.Value.Item1.CompareTo(pair1.Value.Item1));

        int childCount = speciesDisplayList.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = speciesDisplayList.transform.GetChild(i);
            child.gameObject.SetActive(false);
        }

        for (int index = 0; index < sortedList.Count; index++)
        {
            if (index >= speciesDisplayList.childCount)
            {
                continue;
            }

            var sorted = sortedList[index];

            string key = sorted.Key;
            int count = sorted.Value.Item1;
            Species species = sorted.Value.Item2;

            SpeciesDisplay panel = speciesDisplayList.transform.GetChild(index).GetComponent<SpeciesDisplay>();
            panel.transform.SetParent(speciesDisplayList);
            Color color = sorted.Value.Item2.getColor();
            panel.image.color = color;

            color.a = 0.5f;
            panel.backgroundImage.color = color;
            panel.text.SetText(sorted.Value.Item1.ToString());

            panel.gameObject.SetActive(true);
        }
    }
}
