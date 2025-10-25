using UnityEngine;
namespace KH
{
    public class WaveManager : MonoBehaviour
    {
        public static WaveManager instance { get; private set; }

        [Header("Wave Data")]
        [SerializeField] private float waveTimer = 0;
        private int nextSpawnIndex = 0;
        private int nextLightSpawnIndex = 0;
        public WaveTemplate currentWave;

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
            waveTimer = 0f; // resets wave timer each wave.
            nextSpawnIndex = 0;
            nextLightSpawnIndex = 0;
        }
        void Update()
        {
            if (currentWave == null) return;
            waveTimer += Time.deltaTime;

            // spawns all enemies in the wave at their assigned time
            while (nextSpawnIndex < currentWave.spawnEvents.Count &&
          waveTimer >= currentWave.spawnEvents[nextSpawnIndex].spawnTime)
            {
                EnemyDatabase.instance.SpawnEnemy(currentWave.spawnEvents[nextSpawnIndex]);
                nextSpawnIndex++;
            }

            // Spawn light zones
            while (nextLightSpawnIndex < currentWave.circularLightSpawnEvents.Count &&
                   waveTimer >= currentWave.circularLightSpawnEvents[nextLightSpawnIndex].spawnTime)
            {
                LightZoneManager.instance.SpawnCircularZone(currentWave.circularLightSpawnEvents[nextLightSpawnIndex].spawnPosition,
                    currentWave.circularLightSpawnEvents[nextLightSpawnIndex].circularZoneType);
                nextLightSpawnIndex++;
            }
        }
        public bool IsWaveFinished()
        {
            return nextSpawnIndex >= currentWave.spawnEvents.Count;
        }
    }

}

