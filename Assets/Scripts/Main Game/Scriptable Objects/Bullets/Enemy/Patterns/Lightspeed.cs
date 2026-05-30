using KH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Patterns/Boss Patterns/Lightspeed")]
public class Lightspeed : EnemyShotPattern
{
    [Header("Lightspeed Data")]
    public int numberOfRepetitions = 6;
    public float delayBetweenPatterns = 1f;

    [Header("Durations")]
    public float lightPillarDuration = 1f;
    public float warningDurationHorizontal = 1f;
    public float warningDurationVertical = 1f;
    public float warningDurationDiagonalRight = 1f;
    public float warningDurationDiagonalLeft = 1f;
    public float warningDurationTargeted = 1.5f;
    public float warningFadeInDuration = 0.4f;

    [Header("Coroutines")]
    public Coroutine spellRoutine;

    public override void Fire(Vector2 origin, GameObject enemy)
    {
        BossManager boss = enemy.GetComponent<BossManager>();

        if (spellRoutine == null)
        {
            spellRoutine = boss.StartCoroutine(LightspeedRoutine(boss));
        }
    }
    private IEnumerator LightspeedRoutine(BossManager boss)
    {
        //add disappearing effect
        //Hides the boss, player can only attempt to survive the spell card
        boss.HideBoss();
        Debug.Log("Hid boss");
        int patternCount = System.Enum.GetValues(typeof(PillarPatternType)).Length;

        //loop for repetitions
        for (int i = 0; i < numberOfRepetitions; i++)
        {
            //Cycles through patterns
            PillarPatternType currentPattern = (PillarPatternType)(i % patternCount);
            Debug.Log($"Executing pattern: {currentPattern}");
            yield return ExecutePattern(GetSpawnPoints(currentPattern), DetermineWarningTime(currentPattern), boss);
            yield return new WaitForSeconds(delayBetweenPatterns);
        }
        boss.RevealBoss();
        spellRoutine = null;
    }
    private IEnumerator ExecutePattern(List<Transform> spawnPoints, float warningTime, BossManager boss)
    {
        List<GameObject> warningPillars = new();
        List<GameObject> lightPillars = new();

        // Fire sound
        if (attackSounds[0] != null)
        {
            AudioManager.instance.PlaySFX(attackSounds[0], boss.transform, attackSoundVolume);
        }

        // Warning phase
        foreach (Transform spawn in spawnPoints)
        {
            GameObject warningPillar = ObjectPool.instance.SpawnBullet(spawn.position);
            warningPillar.transform.rotation = spawn.rotation;
            warningPillar.transform.localScale = new Vector3(1f, 3f, 1f);
            warningPillars.Add(warningPillar);

            BulletController pillarController = warningPillar.GetComponent<BulletController>();
            pillarController.isPillarOfLight = true;
            pillarController.InitializePillarOfLight(bulletTypes[0].sprite, bulletTypes[0]);

            pillarController.StartCoroutine(pillarController.WarningPillarFadeInRoutine(warningFadeInDuration));
        }
        Debug.Log("Spawned warning pillars.");

        Debug.Log($"Waiting {warningTime} seconds");
        Debug.Log($"TimeScale: {Time.timeScale}");
        yield return new WaitForSeconds(warningTime);
        Debug.Log("Finished waiting");

        // Remove warnings
        foreach (GameObject pillar in warningPillars)
        {
            ObjectPool.instance.ReturnToPool(pillar);
        }
        warningPillars.Clear();

        Debug.Log("Removing warning pillars.");

        // Fire sound
        if (attackSounds[1] != null)
        {
            AudioManager.instance.PlaySFX(attackSounds[1], boss.transform, attackSoundVolume);
        }

        // Spawn lethal pillars
        foreach (Transform spawn in spawnPoints)
        {
            GameObject lightPillar = ObjectPool.instance.SpawnBullet(spawn.position);
            lightPillar.transform.rotation = spawn.rotation;
            lightPillar.transform.localScale = new Vector3(1f, 3f, 1f);
            lightPillars.Add(lightPillar);

            BulletController pillarController = lightPillar.GetComponent<BulletController>();
            pillarController.isPillarOfLight = true;
            pillarController.InitializePillarOfLight(bulletTypes[1].sprite, bulletTypes[1]);

            Debug.Log("Spawned lethal pillars.");
        }

        yield return new WaitForSeconds(lightPillarDuration);

        foreach (GameObject pillar in lightPillars)
        {
            ObjectPool.instance.ReturnToPool(pillar);
        }
        Debug.Log("Removed lethal pillars. Finished pattern.");
    }
    private float DetermineWarningTime(PillarPatternType currentPattern)
    {
        switch (currentPattern)
        {
            case PillarPatternType.Horizontal:
                return warningDurationHorizontal;
            case PillarPatternType.Vertical:
                return warningDurationVertical;
            case PillarPatternType.DiagonalLeft:
                return warningDurationDiagonalLeft;
            case PillarPatternType.DiagonalRight:
                return warningDurationDiagonalRight;
        }
        return 1f;
    }
    private List<Transform> GetSpawnPoints(PillarPatternType pattern)
    {
        switch (pattern)
        {
            case PillarPatternType.Horizontal:
                return ObjectPool.instance.horizontalPillarSpawns;

            case PillarPatternType.Vertical:
                return ObjectPool.instance.verticalPillarSpawns;

            case PillarPatternType.DiagonalLeft:
                return ObjectPool.instance.diagonalLeftPillarSpawns;

            case PillarPatternType.DiagonalRight:
                return ObjectPool.instance.diagonalRightPillarSpawns;
        }

        return ObjectPool.instance.horizontalPillarSpawns;
    }
}
