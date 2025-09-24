using KH;
using System.Collections;
using UnityEngine;
public class ItemManager : MonoBehaviour
{
    public static ItemManager instance { get; private set; }

    [Header("Power Types")]
    [SerializeField] private float regularPower = 0.1f;
    [SerializeField] private float greatPower = 1.0f;
    [SerializeField] private float fullPower = 5.0f;

    [Header("Score Types")]
    [SerializeField] private int regularScore = 500;
    [SerializeField] private int greatScore = 2000;
    [SerializeField] private int starFaithMultiplier = 500;
    [SerializeField] private int smallFaithMultiplier = 100;

    [Header("Sprites")]
    [SerializeField] public Sprite regularScoreSprite;
    [SerializeField] public Sprite greatScoreSprite;
    [Space]
    [SerializeField] public Sprite starFaithSprite;
    [SerializeField] public Sprite smallFaithSprite;
    [Space]
    [SerializeField] private Sprite powerSprite;
    [SerializeField] private Sprite greatPowerSprite;
    [SerializeField] private Sprite fullPowerSprite;
    [Space]
    [SerializeField] private Sprite oneUpSprite;
    [SerializeField] private Sprite bombSprite;

    public Coroutine convertAllPowerItemsCoroutine = null;

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
        itemController.InitializeItem(ItemType.Score, regularScore, 0, 0, regularScoreSprite);
    }
    public void SpawnGreatScoreItem(Vector3 spawnPos)
    {
        GameObject scoreItem = ObjectPool.instance.GetPooledItem();
        scoreItem.transform.position = spawnPos;
        ItemController itemController = scoreItem.GetComponent<ItemController>();
        itemController.InitializeItem(ItemType.Score, greatScore, 0, 0, greatScoreSprite);
    }
    public void SpawnStarFaithItem(Vector3 spawnPos)
    {
        GameObject scoreItem = ObjectPool.instance.GetPooledItem();
        scoreItem.transform.position = spawnPos;
        ItemController itemController = scoreItem.GetComponent<ItemController>();
        itemController.InitializeItem(ItemType.Faith, 0, 0, starFaithMultiplier, starFaithSprite);
    }
    public void SpawnSmallFaithItem(Vector3 spawnPos)
    {
        GameObject scoreItem = ObjectPool.instance.GetPooledItem();
        scoreItem.transform.position = spawnPos;
        ItemController itemController = scoreItem.GetComponent<ItemController>();
        itemController.InitializeItem(ItemType.Faith, 0, 0, smallFaithMultiplier, smallFaithSprite);
    }
    public void SpawnRegularPowerItem(Vector3 spawnPos)
    {
        GameObject powerItem = ObjectPool.instance.GetPooledItem();
        powerItem.transform.position = spawnPos;
        ItemController itemController = powerItem.GetComponent<ItemController>();
        itemController.InitializeItem(ItemType.Power, 0, regularPower, 0, powerSprite);
    }
    public void SpawnGreatPowerItem(Vector3 spawnPos)
    {
        GameObject powerItem = ObjectPool.instance.GetPooledItem();
        powerItem.transform.position = spawnPos;
        ItemController itemController = powerItem.GetComponent<ItemController>();
        itemController.InitializeItem(ItemType.Power, 0, greatPower, 0, powerSprite);
    }
    public void SpawnFullPowerItem(Vector3 spawnPos)
    {
        GameObject powerItem = ObjectPool.instance.GetPooledItem();
        powerItem.transform.position = spawnPos;
        ItemController itemController = powerItem.GetComponent<ItemController>();
        itemController.InitializeItem(ItemType.Power, 0, fullPower, 0, fullPowerSprite);
    }
    public void Spawn1UpItem(Vector3 spawnPos)
    {
        GameObject oneUpItem = ObjectPool.instance.GetPooledItem();
        oneUpItem.transform.position = spawnPos;
        ItemController itemController = oneUpItem.GetComponent<ItemController>();
        itemController.InitializeItem(ItemType.OneUp, 0, 0, 0, oneUpSprite);
    }
    public GameObject InitializePlayerDeathItem(int parity, Vector3 spawnPos)
    {
        GameObject powerItem = ObjectPool.instance.GetPooledItem();
        powerItem.transform.position = spawnPos;
        ItemController itemController = powerItem.GetComponent<ItemController>();

        if (parity % 2 == 0)
        { itemController.InitializeItem(ItemType.Power, 0, regularPower, 0, powerSprite); }
        else { itemController.InitializeItem(ItemType.Power, 0, regularPower, 0, powerSprite); }

        return powerItem;
    }

    public void ConvertAllPowerItems()
    {
        convertAllPowerItemsCoroutine = StartCoroutine(ConvertItems());
    }
    private IEnumerator ConvertItems()
    {
        GameObject[] allPowerItems = GameObject.FindGameObjectsWithTag("Power");

        foreach (GameObject item in allPowerItems)
        {
            // Add VFX and SFX as well
            ItemController itemController = item.GetComponent<ItemController>();
            itemController.InitializeItem(ItemType.Score, regularScore, 0, 0, regularScoreSprite);

            yield return null;
        }
    }
}
