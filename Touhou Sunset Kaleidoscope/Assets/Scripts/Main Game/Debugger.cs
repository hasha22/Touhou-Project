using KH;
using UnityEngine;
public class Debugger : MonoBehaviour
{
    public static Debugger instance { get; private set; }

    [Header("Spawn Settings")]
    [SerializeField] private Transform topSpawnPoint;
    [SerializeField] private float horizontalRange = 2f;
    [SerializeField] private GameObject enemyObject;

    [Header("Spawning")]
    [SerializeField] private bool regularPowerItem = false;
    [SerializeField] private bool greatPowerItem = false;
    [SerializeField] private bool fullPowerItem = false;
    [Space]
    [SerializeField] private bool regularScoreItem = false;
    [SerializeField] private bool greatScoreItem = false;
    [Space]
    [SerializeField] private bool smallFaithItem = false;
    [SerializeField] private bool starFaithItem = false;
    [Space]
    [SerializeField] private bool oneUpItem = false;
    [Space]
    [SerializeField] private bool enableEnemy = false;
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
        if (greatPowerItem)
        {
            SpawnGreatPowerItem();
            greatPowerItem = !greatPowerItem;
        }
        if (regularPowerItem)
        {
            SpawnRegularPowerItem();
            regularPowerItem = !regularPowerItem;
        }
        if (fullPowerItem)
        {
            SpawnFullPowerItem();
            fullPowerItem = !fullPowerItem;
        }
        if (regularScoreItem)
        {
            SpawnRegularScoreItem();
            regularScoreItem = !regularScoreItem;
        }
        if (greatScoreItem)
        {
            SpawnGreatScoreItem();
            greatScoreItem = !greatScoreItem;
        }
        if (smallFaithItem)
        {
            SpawnSmallFaithItem();
            smallFaithItem = !smallFaithItem;
        }
        if (starFaithItem)
        {
            SpawnStarFaithItem();
            starFaithItem = !starFaithItem;
        }
        if (oneUpItem)
        {
            Spawn1UpItem();
            oneUpItem = !oneUpItem;
        }
        if (enableEnemy)
        {
            EnableEnemyObject();
            enableEnemy = !enableEnemy;
        }
    }
    private void SpawnRegularPowerItem()
    {
        Vector3 spawnPoint = topSpawnPoint.position + new Vector3(Random.Range(-horizontalRange, horizontalRange), 0f, 0f);

        ItemManager.instance.SpawnRegularPowerItem(spawnPoint);
    }
    private void SpawnGreatPowerItem()
    {
        Vector3 spawnPoint = topSpawnPoint.position + new Vector3(Random.Range(-horizontalRange, horizontalRange), 0f, 0f);

        ItemManager.instance.SpawnGreatPowerItem(spawnPoint);
    }
    private void SpawnFullPowerItem()
    {
        Vector3 spawnPoint = topSpawnPoint.position + new Vector3(Random.Range(-horizontalRange, horizontalRange), 0f, 0f);

        ItemManager.instance.SpawnFullPowerItem(spawnPoint);
    }
    private void SpawnRegularScoreItem()
    {
        Vector3 spawnPoint = topSpawnPoint.position + new Vector3(Random.Range(-horizontalRange, horizontalRange), 0f, 0f);

        ItemManager.instance.SpawnRegularScoreItem(spawnPoint);
    }
    private void SpawnGreatScoreItem()
    {
        Vector3 spawnPoint = topSpawnPoint.position + new Vector3(Random.Range(-horizontalRange, horizontalRange), 0f, 0f);
        ItemManager.instance.SpawnGreatScoreItem(spawnPoint);
    }
    private void SpawnSmallFaithItem()
    {
        Vector3 spawnPoint = topSpawnPoint.position + new Vector3(Random.Range(-horizontalRange, horizontalRange), 0f, 0f);
        ItemManager.instance.SpawnSmallFaithItem(spawnPoint);
    }
    private void SpawnStarFaithItem()
    {
        Vector3 spawnPoint = topSpawnPoint.position + new Vector3(Random.Range(-horizontalRange, horizontalRange), 0f, 0f);
        ItemManager.instance.SpawnStarFaithItem(spawnPoint);
    }
    private void Spawn1UpItem()
    {
        Vector3 spawnPoint = topSpawnPoint.position + new Vector3(Random.Range(-horizontalRange, horizontalRange), 0f, 0f);
        ItemManager.instance.Spawn1UpItem(spawnPoint);
    }
    private void EnableEnemyObject()
    {
        enemyObject.SetActive(true);

        EnemyController enemyController = enemyObject.GetComponent<EnemyController>();
        enemyController.hasDied = false;
        Enemy enemy = enemyController.GetEnemyData();
        enemy.enemyHealth = enemy.healthResetValue;
    }

}
