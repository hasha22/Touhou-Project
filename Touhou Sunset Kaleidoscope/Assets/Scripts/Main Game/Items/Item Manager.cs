using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager instance { get; private set; }

        [Header("Power Types")]
        [SerializeField] private float regularPower = 0.05f;
        [SerializeField] private float greatPower = 1.0f;
        [SerializeField] private float fullPower = 5.0f;

        [Header("Score Types")]
        [SerializeField] private int regularScore = 500;
        [SerializeField] private int greatScore = 2000;
        [SerializeField] private int starFaithMultiplier = 500;
        [SerializeField] private int smallFaithMultiplier = 100;

        [Header("Item Auto-Collect")]
        public float topBoundaryWorldY = 5f;
        [SerializeField] private float offScreenSpawnMargin = 2f;

        [Header("Offscreen UI")]
        [SerializeField] private RectTransform indicatorParent;

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

        [Header("Coroutines")]
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
        public void SpawnSmallFaithItem(Vector3 spawnPos)
        {
            GameObject scoreItem = ObjectPool.instance.GetPooledItem();
            scoreItem.transform.position = spawnPos;
            ItemController itemController = scoreItem.GetComponent<ItemController>();
            itemController.InitializeItem(ItemType.Faith, 0, 0, smallFaithMultiplier, smallFaithSprite);
        }
        public void SpawnStarFaithItem(Vector3 spawnPos)
        {
            GameObject scoreItem = ObjectPool.instance.GetPooledItem();
            scoreItem.transform.position = spawnPos;
            ItemController itemController = scoreItem.GetComponent<ItemController>();
            itemController.InitializeItem(ItemType.Faith, 0, 0, starFaithMultiplier, starFaithSprite);
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
            else
            {
                powerItem.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
                itemController.InitializeItem(ItemType.Power, 0, greatPower, 0, powerSprite);
            }

            return powerItem;
        }

        public void ConvertAllPowerItems()
        {
            convertAllPowerItemsCoroutine = StartCoroutine(ConvertItems());
        }
        public void AutoCollectAllItems()
        {
            PlayerManager playerManager = PlayerInputManager.instance.playerObject.GetComponent<PlayerManager>();
            foreach (var item in ObjectPool.instance.GetItemPool())
            {
                ItemController itemController = item.GetComponent<ItemController>();
                if (!item.activeInHierarchy) continue;

                itemController.currentPullRadius = itemController.autoCollectPullRadius;

            }
        }
        public void ResetItemPullRadius()
        {
            PlayerManager playerManager = PlayerInputManager.instance.playerObject.GetComponent<PlayerManager>();
            foreach (var item in ObjectPool.instance.GetItemPool())
            {
                ItemController itemController = item.GetComponent<ItemController>();

                itemController.currentPullRadius = itemController.defaultPullRadius;
            }
        }
        private IEnumerator ConvertItems()
        {
            List<GameObject> allPowerItems = ObjectPool.instance.GetItemPool();

            foreach (GameObject item in allPowerItems)
            {
                if (item.activeInHierarchy && item.CompareTag("Power"))
                {
                    // Add VFX and SFX as well
                    ItemController itemController = item.GetComponent<ItemController>();
                    if (itemController.GetPower() == fullPower || itemController.GetPower() == greatPower)
                    { itemController.InitializeItem(ItemType.Score, greatScore, 0, 0, greatScoreSprite); }
                    else
                    { itemController.InitializeItem(ItemType.Score, regularScore, 0, 0, regularScoreSprite); }
                }
                yield return null;
            }
        }
    }
}