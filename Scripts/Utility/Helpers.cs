using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static Vector2 generateRandomDirection()
    {
        float angle = Random.Range(0f, 360f);
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    public static void moveTowards(Entity entity, Vector2 direction, float speedScale = 1)
    {
        Rigidbody2D rigidbody = entity.getRigidbody();

        Vector2 desiredVelocity = direction * entity.getStats().movementSpeed * speedScale;

        // if (rigidbody.velocity.magnitude > desiredVelocity.magnitude)
        // {
        //     desiredVelocity = Vector2.Lerp(rigidbody.velocity, desiredVelocity, 5f);
        // }

        Vector2 velocityDifference = desiredVelocity - rigidbody.velocity;
        Vector2 dampenedVelocityDifference = velocityDifference * (0.8f * Time.fixedDeltaTime);
        Vector2 finalVelocity = rigidbody.velocity + dampenedVelocityDifference;

        rigidbody.AddForce(finalVelocity - rigidbody.velocity, ForceMode2D.Impulse);


        rotateTowards(entity, direction, speedScale);
    }

    public static void rotateTowards(Entity entity, Vector2 direction, float speedScale = 1)
    {
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
        entity.transform.rotation = Quaternion.Slerp(
            entity.transform.rotation, targetRotation, entity.getStats().rotationSpeed * speedScale * Time.fixedDeltaTime
        );
    }

    public static void rotateTowards(Entity entity, GameObject target, float speedScale = 1)
    {
        rotateTowards(entity, (target.transform.position - entity.transform.position).normalized, speedScale);
    }

    public static void moveTowards(Entity entity, GameObject target, float speedScale = 1)
    {
        moveTowards(entity, (target.transform.position - entity.transform.position).normalized, speedScale);
    }

    public static Color getRandomColor(float minBrightness = 0.5f)
    {
        Color randomColor;
        float brightness;

        do
        {
            randomColor = new Color(Random.value, Random.value, Random.value, 1.0f);
            brightness = randomColor.grayscale;
        } while (brightness < minBrightness);

        return randomColor;
    }

    public static T getRandomFromList<T>(List<T> list)
    {
        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        return list[randomIndex];
    }

    public static void shuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static string generateUniqueID()
    {
        return System.Guid.NewGuid().ToString();
    }

    public static T getRandomEnumValue<T>() where T : System.Enum
    {
        T[] allValues = (T[])System.Enum.GetValues(typeof(T));
        int randomIndex = Random.Range(0, allValues.Length);
        return allValues[randomIndex];
    }

    public static bool getRandomBool()
    {
        return Random.Range(0, 2) == 1;
    }
}