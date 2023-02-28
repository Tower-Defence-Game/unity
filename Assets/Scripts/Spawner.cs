using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<SpawnOptions> spawnOptions = new List<SpawnOptions>();

    void Start()
    {
        foreach (var spawn in spawnOptions)
            StartCoroutine(CreateObjectsCoroutine(spawn.ObjectToSpawn, spawn.Wait, spawn.Every, spawn.Count));
    }

    void CreateObject(GameObject gameObject)
    {
        Instantiate(gameObject);
    }

    IEnumerator CreateObjectsCoroutine(GameObject gameObject, float wait, float every, int count)
    {
        yield return new WaitForSeconds(wait);

        for (int i = 0; i < count; i++) {
            CreateObject(gameObject);
            yield return new WaitForSeconds(every);
        }
    }
}
