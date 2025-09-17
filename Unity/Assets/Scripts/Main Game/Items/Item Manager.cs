using KH;
using UnityEngine;
public class ItemManager : MonoBehaviour
{
    public static ItemManager instance { get; private set; }

    [Header("Power Types")]
    [SerializeField] private float regularPower = 0.1f;
    [SerializeField] private float greatPower = 0.5f;

    [Header("Score Types")]
    [SerializeField] private int regularScore = 500;
    [SerializeField] private int mediumScore = 1000;
    [SerializeField] private int greatScore = 2000;

    [Header("Sprites")]
    [SerializeField] private Sprite scoreSprite;
    [SerializeField] private Sprite powerSprite;
    [SerializeField] private Sprite oneUpSprite;

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
    public void SpawnRegularScoreItem(Vector3 spawnPos)
    {
        GameObject scoreItem = ObjectPool.instance.GetPooledItem();
        scoreItem.transform.position = spawnPos;
        ItemController itemController = scoreItem.GetComponent<ItemController>();
        itemController.InitializeItem(ItemType.Score, regularScore, 0, scoreSprite);
    }
    public void SpawnMediumScoreItem(Vector3 spawnPos)
    {
        GameObject scoreItem = ObjectPool.instance.GetPooledItem();
        scoreItem.transform.position = spawnPos;
        ItemController itemController = scoreItem.GetComponent<ItemController>();
        itemController.InitializeItem(ItemType.Score, mediumScore, 0, scoreSprite);
    }
    public void SpawnGreatScoreItem(Vector3 spawnPos)
    {
        GameObject scoreItem = ObjectPool.instance.GetPooledItem();
        scoreItem.transform.position = spawnPos;
        ItemController itemController = scoreItem.GetComponent<ItemController>();
        itemController.InitializeItem(ItemType.Score, greatScore, 0, scoreSprite);
    }
    public void SpawnRegularPowerItem(Vector3 spawnPos)
    {
        GameObject powerItem = ObjectPool.instance.GetPooledItem();
        powerItem.transform.position = spawnPos;
        ItemController itemController = powerItem.GetComponent<ItemController>();
        itemController.InitializeItem(ItemType.Power, 0, regularPower, powerSprite);
    }
    public void SpawnGreatPowerItem(Vector3 spawnPos)
    {
        GameObject powerItem = ObjectPool.instance.GetPooledItem();
        powerItem.transform.position = spawnPos;
        ItemController itemController = powerItem.GetComponent<ItemController>();
        itemController.InitializeItem(ItemType.Power, 0, greatPower, powerSprite);
    }
    public void Spawn1UpItem(Vector3 spawnPos)
    {
        GameObject oneUpItem = ObjectPool.instance.GetPooledItem();
        oneUpItem.transform.position = spawnPos;
        ItemController itemController = oneUpItem.GetComponent<ItemController>();
        itemController.InitializeItem(ItemType.OneUp, 0, 0, oneUpSprite);
    }
}
