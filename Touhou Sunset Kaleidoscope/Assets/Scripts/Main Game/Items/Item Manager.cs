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
        public bool isAutoCollecting = false;

        [Header("Offscreen UI")]
        public Transform topItemBar;

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
            scoreItem.transform.localScale = new Vector3(1, 1, 1);
            ItemController itemController = scoreItem.GetComponent<ItemController>();
            itemController.InitializeItem(ItemType.Score, regularScore, 0, 0, regularScoreSprite);

            scoreItem.SetActive(true);
        }
        public void SpawnGreatScoreItem(Vector3 spawnPos)
        {
            GameObject scoreItem = ObjectPool.instance.GetPooledItem();
            scoreItem.transform.position = spawnPos;
            scoreItem.transform.localScale = new Vector3(1, 1, 1);
            ItemController itemController = scoreItem.GetComponent<ItemController>();
            itemController.InitializeItem(ItemType.Score, greatScore, 0, 0, greatScoreSprite);

            scoreItem.SetActive(true);
        }
        public void SpawnSmallFaithItem(Vector3 spawnPos)
        {
            GameObject faithItem = ObjectPool.instance.GetPooledItem();
            faithItem.transform.position = spawnPos;
            faithItem.transform.localScale = new Vector3(1, 1, 1);
            ItemController itemController = faithItem.GetComponent<ItemController>();
            itemController.InitializeItem(ItemType.Faith, 0, 0, smallFaithMultiplier, smallFaithSprite);

            faithItem.SetActive(true);
        }
        public void SpawnStarFaithItem(Vector3 spawnPos)
        {
            GameObject faithItem = ObjectPool.instance.GetPooledItem();
            faithItem.transform.position = spawnPos;
            faithItem.transform.localScale = new Vector3(1, 1, 1);
            ItemController itemController = faithItem.GetComponent<ItemController>();
            itemController.InitializeItem(ItemType.Faith, 0, 0, starFaithMultiplier, starFaithSprite);

            faithItem.SetActive(true);
        }
        public void SpawnRegularPowerItem(Vector3 spawnPos)
        {
            GameObject powerItem = ObjectPool.instance.GetPooledItem();
            powerItem.transform.position = spawnPos;
            powerItem.transform.localScale = new Vector3(1, 1, 1);
            ItemController itemController = powerItem.GetComponent<ItemController>();
            itemController.InitializeItem(ItemType.Power, 0, regularPower, 0, powerSprite);

            PlayerManager playerManager = PlayerInputManager.instance.playerObject.GetComponent<PlayerManager>();
            if (playerManager.currentPower == 5f)
            {
                ConvertPowerItemToScore(itemController);
            }
            powerItem.SetActive(true);
        }
        public void SpawnGreatPowerItem(Vector3 spawnPos)
        {
            GameObject powerItem = ObjectPool.instance.GetPooledItem();
            powerItem.transform.position = spawnPos;
            powerItem.transform.localScale = new Vector3(1, 1, 1);
            ItemController itemController = powerItem.GetComponent<ItemController>();
            itemController.InitializeItem(ItemType.Power, 0, greatPower, 0, powerSprite);

            PlayerManager playerManager = PlayerInputManager.instance.playerObject.GetComponent<PlayerManager>();
            if (playerManager.currentPower == 5f)
            {
                ConvertPowerItemToScore(itemController);
            }
            powerItem.SetActive(true);
        }
        public void SpawnFullPowerItem(Vector3 spawnPos)
        {
            GameObject powerItem = ObjectPool.instance.GetPooledItem();
            powerItem.transform.position = spawnPos;
            powerItem.transform.localScale = new Vector3(1, 1, 1);
            ItemController itemController = powerItem.GetComponent<ItemController>();
            itemController.InitializeItem(ItemType.Power, 0, fullPower, 0, fullPowerSprite);

            PlayerManager playerManager = PlayerInputManager.instance.playerObject.GetComponent<PlayerManager>();
            if (playerManager.currentPower == 5f)
            {
                ConvertPowerItemToScore(itemController);
            }
            powerItem.SetActive(true);
        }
        public void Spawn1UpItem(Vector3 spawnPos)
        {
            GameObject oneUpItem = ObjectPool.instance.GetPooledItem();
            oneUpItem.transform.position = spawnPos;
            oneUpItem.transform.localScale = new Vector3(1, 1, 1);
            ItemController itemController = oneUpItem.GetComponent<ItemController>();
            itemController.InitializeItem(ItemType.OneUp, 0, 0, 0, oneUpSprite);

            oneUpItem.SetActive(true);
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
            powerItem.SetActive(true);

            return powerItem;
        }
        public void ConvertPowerItemToScore(ItemController itemController)
        {
            if (itemController.GetPower() == fullPower || itemController.GetPower() == greatPower)
            { itemController.InitializeItem(ItemType.Score, greatScore, 0, 0, greatScoreSprite); }
            else
            { itemController.InitializeItem(ItemType.Score, regularScore, 0, 0, regularScoreSprite); }
        }

        public void ConvertAllPowerItems()
        {
            convertAllPowerItemsCoroutine = StartCoroutine(ConvertItems());
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
        public void AutoCollectAllItems()
        {
            isAutoCollecting = true;
            PlayerManager playerManager = PlayerInputManager.instance.playerObject.GetComponent<PlayerManager>();
            foreach (var item in ObjectPool.instance.GetItemPool())
            {
                ItemController itemController = item.GetComponent<ItemController>();

                if (item.activeInHierarchy && !itemController.isAboveTop && itemController.IsInPlayableArea(item.transform.position))
                {
                    itemController.wasPulled = true;
                    itemController.currentPullRadius = itemController.autoCollectPullRadius;
                }
            }
        }
        public void ResetItemPullRadius()
        {
            isAutoCollecting = false;
            PlayerManager playerManager = PlayerInputManager.instance.playerObject.GetComponent<PlayerManager>();
            foreach (var item in ObjectPool.instance.GetItemPool())
            {
                ItemController itemController = item.GetComponent<ItemController>();

                itemController.currentPullRadius = itemController.defaultPullRadius;
            }
        }
        public void SpawnItem(ItemToSpawn item, Vector3 spawnPosition)
        {
            switch (item)
            {
                case ItemToSpawn.Power:
                    SpawnRegularPowerItem(spawnPosition);
                    break;
                case ItemToSpawn.GreatPower:
                    SpawnGreatPowerItem(spawnPosition);
                    break;
                case ItemToSpawn.FullPower:
                    SpawnFullPowerItem(spawnPosition);
                    break;
                case ItemToSpawn.Score:
                    SpawnRegularScoreItem(spawnPosition);
                    break;
                case ItemToSpawn.GreatScore:
                    SpawnGreatPowerItem(spawnPosition);
                    break;
                case ItemToSpawn.Faith:
                    SpawnSmallFaithItem(spawnPosition);
                    break;
                case ItemToSpawn.StarFaith:
                    SpawnStarFaithItem(spawnPosition);
                    break;
                case ItemToSpawn.OneUp:
                    Spawn1UpItem(spawnPosition);
                    break;
            }
        }
    }
}