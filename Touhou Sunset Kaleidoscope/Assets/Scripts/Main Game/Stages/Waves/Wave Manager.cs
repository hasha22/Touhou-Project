using UnityEngine;
namespace KH
{
    public class WaveManager : MonoBehaviour
    {
        public static WaveManager instance { get; private set; }

        [Header("Wave Data")]
        [SerializeField] private float waveTimer = 0;
        private int nextSpawnIndex = 0;
        [SerializeField] private WaveTemplate currentWave;
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
        public void InitializeWave(WaveTemplate wave)
        {
            currentWave = wave;
            waveTimer = 0f;
            nextSpawnIndex = 0;
        }
        void Update()
        {
            if (currentWave == null) return;
            waveTimer += Time.deltaTime;

            while (nextSpawnIndex < currentWave.spawnEvents.Count &&
                   waveTimer >= currentWave.spawnEvents[nextSpawnIndex].spawnTime)
            {
                EnemyDatabase.instance.SpawnEnemy(currentWave.spawnEvents[nextSpawnIndex]);
                nextSpawnIndex++;
            }
        }
        public bool IsWaveFinished()
        {
            return nextSpawnIndex >= currentWave.spawnEvents.Count;
        }
    }

}

