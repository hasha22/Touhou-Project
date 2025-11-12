using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KH
{
    public class StageManager : MonoBehaviour
    {
        public static StageManager instance { get; private set; }

        [Header("Stage Data")]
        public List<StageTemplate> stages;
        public int currentStageIndex = 0;
        [SerializeField] private string currentStageName;
        [SerializeField] private float elapsedStageTime;
        [SerializeField] private float initialWaveDelay = 5f;
        private StageTemplate currentStage;

        [Header("Current Wave Information")]
        private int currentWaveIndex;
        [SerializeField] private float timerBetweenWaves = 0f;

        [Header("Flags")]
        public bool isPaused = false;
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
                StartStage();
            }
        }
        public void StartStage()
        {
            StartCoroutine(InitialWaveDelayCoroutine(initialWaveDelay));
        }
        private IEnumerator InitialWaveDelayCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            InitializeStage(0);
        }
        private void Update()
        {
            if (isPaused) return;
            if (currentStage == null) return;
            if (WaveManager.instance.currentWave == null) return;

            elapsedStageTime += Time.deltaTime;

            // If the wave just finished, start the waiting timer
            if (WaveManager.instance.IsWaveFinished() && !waitingForNextWave)
            {
                waitingForNextWave = true;
                timerBetweenWaves = 0;
            }

            if (waitingForNextWave)
            {
                timerBetweenWaves += Time.deltaTime;

                if (timerBetweenWaves >= currentStage.waves[currentWaveIndex].delayBeforeNextWave)
                {
                    waitingForNextWave = false;
                    timerBetweenWaves = 0;

                    // Boss spawning - all stages have one midboss and one main boss, so SpawnBoss() is hardcoded
                    if (currentStage.bosses[0].spawnAfterWaveIndex == currentWaveIndex + 1 && !hasSpawnedFirstBoss)
                    {
                        StartCoroutine(SpawnBossWithDelay(currentStage.bosses[0], currentStage.bosses[0].delayBeforeSpawn));
                        hasSpawnedFirstBoss = true;
                        return;
                    }
                    else if (currentStage.bosses[1].spawnAfterWaveIndex == currentWaveIndex + 1 && !hasSpawnedSecondBoss)
                    {
                        TriggerBossEvent(currentStage.bosses[1]);
                        hasSpawnedSecondBoss = true;
                        return;
                    }

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
        private IEnumerator SpawnBossWithDelay(Boss bossData, float delay)
        {
            yield return new WaitForSeconds(delay);
            TriggerBossEvent(bossData);
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
        public void ResetStage()
        {
            timerBetweenWaves = 0;
            waitingForNextWave = true;
            WaveManager.instance.ResetWave();
            StartStage();
        }

    }
}
