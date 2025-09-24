using UnityEngine;

public class Debugger : MonoBehaviour
{
    public static Debugger instance { get; private set; }

    [Header("Spawn Settings")]
    public Transform topSpawnPoint;
    public float horizontalRange = 2f;

    [Header("Bools")]
    public bool spawnItem = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Update()
    {
        if (spawnItem)
        {
            SpawnItem();
            spawnItem = !spawnItem;
        }
    }
    private void SpawnItem()
    {
        Vector3 spawnPoint = topSpawnPoint.position + new Vector3(Random.Range(-horizontalRange, horizontalRange), 0f, 0f);

        ItemManager.instance.SpawnGreatPowerItem(spawnPoint);
    }

}
