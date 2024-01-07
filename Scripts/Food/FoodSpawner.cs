using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;

    private Timer spawnTimer;

    private float minSpawnSeconds = 2f;

    private float maxSpawnSeconds = 10f;

    void Start()
    {
        this.spawnTimer = new Timer(Random.Range(this.minSpawnSeconds, this.maxSpawnSeconds), () =>
        {
            this.spawnFood();
            float newDuration = Random.Range(this.minSpawnSeconds, this.maxSpawnSeconds);
            this.spawnTimer.setDuration(newDuration);
            this.spawnTimer.setDurationLeft(newDuration);
        }, true);

        this.spawnFood();
    }

    private void spawnFood()
    {
        GameObject foodGo = Instantiate(this.foodPrefab);
        foodGo.transform.position = transform.position;
        Vector2 direction = Helpers.generateRandomDirection();

        Rigidbody2D r = foodGo.GetComponent<Rigidbody2D>();
        r.AddForce(direction * Random.Range(10, 30));
    }
}
