using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class StageManager : MonoBehaviour
    {
        public static StageManager instance { get; private set; }

        [Header("Stage Data")]
        public List<StageTemplate> stages;
        private StageTemplate currentStage;
        [SerializeField] private string currentStageName;
        [SerializeField] private float elapsedStageTime;
        public int currentStageIndex = 0;

        [Header("Current Wave Information")]
        private int currentWaveIndex;
        [SerializeField] private float timerBetweenWaves = 0f;
        private bool waitingForNextWave = false;

        private bool hasSpawnedFirstBoss = false;
        private bool hasSpawnedSecondBoss = false;
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
        private void Start()
        {
            // Initializes first stage
            if (stages.Count > 0)
            {
                InitializeStage(0);
            }
        }
        private void Update()
        {
            if (currentStage == null) return;

            elapsedStageTime += Time.deltaTime;
            // If the wave just finished, start the waiting timer
            if (WaveManager.instance.IsWaveFinished() && !waitingForNextWave)
            {
                waitingForNextWave = true;
                timerBetweenWaves = 0;
            }

            // Boss spawning - all stages have one midboss and one main boss, so SpawnBoss() is hardcoded
            if (elapsedStageTime >= currentStage.bosses[0].spawnTimeInStage && !hasSpawnedFirstBoss)
            {
                TriggerBossEvent(currentStage.bosses[0]);
                hasSpawnedFirstBoss = true;
            }
            else if (elapsedStageTime >= currentStage.bosses[1].spawnTimeInStage && !hasSpawnedSecondBoss)
            {
                TriggerBossEvent(currentStage.bosses[1]);
                hasSpawnedSecondBoss = true;
            }

            if (waitingForNextWave)
            {
                timerBetweenWaves += Time.deltaTime;

                if (timerBetweenWaves >= currentStage.waves[currentWaveIndex].delayBeforeNextWave)
                {
                    waitingForNextWave = false;
                    timerBetweenWaves = 0;

                    // Move to next wave or finish stage
                    if (currentWaveIndex < currentStage.waves.Count - 1)
                    {
                        currentWaveIndex++;
                        WaveManager.instance.InitializeWave(currentStage.waves[currentWaveIndex]);
                    }
                    else if (elapsedStageTime >= currentStage.stageDuration)
                    {
                        //OnStageCompleted();
                    }
                }
            }
        }
        public void InitializeStage(int index)
        {
            currentStageIndex = index;
            currentStage = stages[currentStageIndex];
            currentWaveIndex = 0;
            elapsedStageTime = 0;
            currentStageName = currentStage.stageName;

            hasSpawnedFirstBoss = false;
            hasSpawnedSecondBoss = false;

            // Start first wave
            if (currentStage.waves.Count > 0)
                WaveManager.instance.InitializeWave(currentStage.waves[currentWaveIndex]);

        }
        private void OnStageCompleted()
        {
            Debug.Log($"Stage {currentStage.stageName} completed!");

            // Advance to next stage if any
            if (currentStageIndex < stages.Count - 1)
            {
                InitializeStage(currentStageIndex + 1);
            }
            else
            {
                currentStage = null;
                elapsedStageTime = 0;
            }
        }
        private void TriggerBossEvent(Boss bossData)
        {
            EnemyDatabase.instance.SpawnBoss(bossData);
            //UI Updates - timer, health bar, background
            //VFX
            //Trigger Boss Movement and patterns
        }

    }
}
