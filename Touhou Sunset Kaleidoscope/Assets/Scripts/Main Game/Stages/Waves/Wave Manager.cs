using UnityEngine;
namespace KH
{
    public class WaveManager : MonoBehaviour
    {
        public static WaveManager instance { get; private set; }

        [Header("Wave Data")]
        [SerializeField] private float waveTimer = 0;
        private int nextSpawnIndex = 0;
        private int nextCircularLightSpawnIndex = 0;
        private int nextPillarLightSpawnIndex = 0;
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
            nextCircularLightSpawnIndex = 0;
            nextPillarLightSpawnIndex = 0;
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

            // Spawn circular light zones
            while (nextCircularLightSpawnIndex < currentWave.circularLightSpawnEvents.Count &&
                   waveTimer >= currentWave.circularLightSpawnEvents[nextCircularLightSpawnIndex].spawnTime)
            {
                LightZoneManager.instance.SpawnCircularZone(currentWave.circularLightSpawnEvents[nextCircularLightSpawnIndex].spawnPosition,
                    currentWave.circularLightSpawnEvents[nextCircularLightSpawnIndex].circularZoneSize);
                nextCircularLightSpawnIndex++;
            }

            // Spawn pillar light zones
            while (nextPillarLightSpawnIndex < currentWave.pillarLightSpawnEvents.Count &&
                   waveTimer >= currentWave.pillarLightSpawnEvents[nextPillarLightSpawnIndex].spawnTime)
            {
                LightZoneManager.instance.SpawnPillarZone(currentWave.pillarLightSpawnEvents[nextPillarLightSpawnIndex].spawnPosition,
                    currentWave.pillarLightSpawnEvents[nextPillarLightSpawnIndex].pillarZoneSize,
                     currentWave.pillarLightSpawnEvents[nextPillarLightSpawnIndex].direction);
                nextPillarLightSpawnIndex++;
            }
        }
        public bool IsWaveFinished()
        {
            return nextSpawnIndex >= currentWave.spawnEvents.Count &&
                nextCircularLightSpawnIndex >= currentWave.circularLightSpawnEvents.Count &&
                nextPillarLightSpawnIndex >= currentWave.pillarLightSpawnEvents.Count;
        }
    }

}

