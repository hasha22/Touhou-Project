using KH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Patterns/Boss Patterns/Lightspeed")]
public class Lightspeed : EnemyShotPattern
{
    [Header("Lightspeed Data")]
    public int numberOfRepetitionsPhase1 = 4;
    public int numberOfRepetitionsPhase2 = 6;
    public float delayBetweenPatterns = 1f;

    [Header("Durations")]
    public float lightPillarDuration = 1f;
    public float warningDurationHorizontal = 1f;
    public float warningDurationVertical = 1f;
    public float warningDurationDiagonalRight = 1f;
    public float warningDurationDiagonalLeft = 1f;
    public float warningDurationTargeted = 1.5f;
    public float warningFadeInDuration = 0.4f;
    public float delayBetweenPhases = 1f;

    public override void Fire(Vector2 origin, GameObject enemy)
    {
        BossManager boss = enemy.GetComponent<BossManager>();
        if (boss.activeAttackPatternRoutine == null && boss != null)
        {
            boss.activeAttackPatternRoutine = boss.StartCoroutine(LightspeedRoutine(boss));
        }
    }
    private IEnumerator LightspeedRoutine(BossManager boss)
    {
        //add disappearing effect
        //Hides the boss, player can only attempt to survive the spell card
        boss.HideBoss();
        int patternCountPhase1 = System.Enum.GetValues(typeof(PillarPatternType)).Length - 1;
        int patternCountPhase2 = patternCountPhase1 + 1;

        //Phase 1 Repetitions
        for (int i = 0; i < numberOfRepetitionsPhase1; i++)
        {
            //Cycles through patterns
            PillarPatternType currentPattern = (PillarPatternType)(i % patternCountPhase1);
            yield return ExecutePattern(currentPattern, GetSpawnPoints(currentPattern), DetermineWarningTime(currentPattern), boss);
            yield return new WaitForSeconds(delayBetweenPatterns);
        }

        //Perhaps add a mid fight dialogue as a warning?
        yield return new WaitForSeconds(delayBetweenPhases);

        //Phase 2 Repetitions
        PillarPatternType previousPattern = PillarPatternType.DiagonalRight;
        for (int i = 0; i < numberOfRepetitionsPhase2; i++)
        {
            PillarPatternType currentPattern;
            //This prevents the random pattern from being the same as the previous one
            do
            {
                currentPattern =
                    (PillarPatternType)Random.Range(0, patternCountPhase2);
            }
            while (currentPattern == previousPattern);
            previousPattern = currentPattern;

            yield return ExecutePattern(currentPattern, GetSpawnPoints(currentPattern), DetermineWarningTime(currentPattern), boss);
            yield return new WaitForSeconds(delayBetweenPatterns);
        }
        boss.RevealBoss();
        boss.activeAttackPatternRoutine = null;
    }
    private IEnumerator ExecutePattern(PillarPatternType currentPattern, List<Transform> spawnPoints, float warningTime, BossManager boss)
    {
        List<GameObject> warningPillars = new();
        List<GameObject> lightPillars = new();
        List<Quaternion> pillarRotations = new();

        // Fire sound
        if (attackSounds[0] != null)
        {
            AudioManager.instance.PlaySFX(attackSounds[0], boss.transform, attackSoundVolume);
        }

        // Warning phase
        foreach (Transform spawn in spawnPoints)
        {
            GameObject warningPillar = ObjectPool.instance.SpawnBullet(spawn.position);
            warningPillar.transform.localScale = new Vector3(1f, 3f, 1f);
            warningPillars.Add(warningPillar);

            Quaternion pillarRotation = GetPillarRotation(spawn, currentPattern);
            warningPillar.transform.rotation = pillarRotation;
            pillarRotations.Add(pillarRotation);

            BulletController pillarController = warningPillar.GetComponent<BulletController>();
            pillarController.isPillarOfLight = true;
            pillarController.InitializePillarOfLight(bulletTypes[0].sprite, bulletTypes[0]);

            pillarController.StartCoroutine(pillarController.WarningPillarFadeInRoutine(warningFadeInDuration));
        }
        yield return new WaitForSeconds(warningTime);

        // Remove warnings
        foreach (GameObject pillar in warningPillars)
        {
            ObjectPool.instance.ReturnToPool(pillar);
        }
        warningPillars.Clear();

        // Fire sound
        if (attackSounds[1] != null)
        {
            AudioManager.instance.PlaySFX(attackSounds[1], boss.transform, attackSoundVolume);
        }

        // Spawn lethal pillars
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            Transform spawn = spawnPoints[i];
            GameObject lightPillar = ObjectPool.instance.SpawnBullet(spawn.position);

            lightPillar.transform.rotation = pillarRotations[i];
            lightPillar.transform.localScale = new Vector3(1f, 3f, 1f);

            lightPillars.Add(lightPillar);

            BulletController pillarController = lightPillar.GetComponent<BulletController>();
            pillarController.isPillarOfLight = true;
            pillarController.InitializePillarOfLight(bulletTypes[1].sprite, bulletTypes[1]);
        }
        boss.StartCoroutine(CleanupPillars(lightPillars)
);
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
            case PillarPatternType.Targeted:
                return warningDurationTargeted;
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
            case PillarPatternType.Targeted:
                return ObjectPool.instance.targetedPillarTransforms;
        }
        return ObjectPool.instance.horizontalPillarSpawns;
    }
    private IEnumerator CleanupPillars(List<GameObject> pillars)
    {
        yield return new WaitForSeconds(lightPillarDuration);

        foreach (GameObject pillar in pillars)
        {
            ObjectPool.instance.ReturnToPool(pillar);
        }
    }
    private Quaternion GetPillarRotation(Transform spawn, PillarPatternType pattern)
    {
        if (pattern != PillarPatternType.Targeted) return spawn.rotation;

        Vector2 playerPos = PlayerInputManager.instance.playerObject.transform.position;
        Vector2 direction = playerPos - (Vector2)spawn.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        return Quaternion.Euler(0f, 0f, angle - 90f);
    }
}
