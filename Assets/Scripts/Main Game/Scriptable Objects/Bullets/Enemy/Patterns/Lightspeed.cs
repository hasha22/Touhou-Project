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

    [Header("Warning Durations")]
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


        //loop for repetitions
        for (int i = 0; i < numberOfRepetitions; i++)
        {
            List<GameObject> warningPillarsToDisable = new List<GameObject>();

            //cycles through patterns
            PillarPatternType currentPattern = (PillarPatternType)i;

            //Determines warning time for each pattern type. Default 1f.
            float warningTime = DetermineWarningTime(currentPattern);

            //Sound Effect
            if (attackSounds[0] != null)
            {
                AudioManager.instance.PlaySFX(attackSounds[0], boss.transform, attackSoundVolume);
            }
            // Horizontal Pillars
            if (i == 0)
            {
                foreach (Transform spawns in ObjectPool.instance.horizontalPillarSpawns)
                {
                    //Spawns Pillar and sets position & rotation
                    GameObject warningPillar = ObjectPool.instance.SpawnBullet(spawns.position);
                    warningPillar.transform.rotation = Quaternion.Euler(0f, 0f, spawns.transform.rotation.z);

                    //Overrides default bullet movement
                    BulletController pillarController = warningPillar.GetComponent<BulletController>();
                    pillarController.isPillarOfLight = true;

                    //Initialization and fading in
                    pillarController.InitializePillarOfLight(bulletTypes[0].sprite, bulletTypes[0]);
                    warningPillarsToDisable.Add(warningPillar);
                    pillarController.StartCoroutine(pillarController.WarningPillarFadeInRoutine(warningPillar, warningFadeInDuration));
                }
                yield return new WaitForSeconds(warningTime);

                if (attackSounds[1] != null)
                {
                    AudioManager.instance.PlaySFX(attackSounds[1], boss.transform, attackSoundVolume);
                }

                foreach (Transform spawns in ObjectPool.instance.horizontalPillarSpawns)
                {
                    //Spawns Pillar and sets position & rotation
                    GameObject lightPillar = ObjectPool.instance.SpawnBullet(spawns.position);
                    lightPillar.transform.rotation = Quaternion.Euler(0f, 0f, spawns.transform.rotation.z);

                    //Overrides default bullet movement
                    BulletController pillarController = lightPillar.GetComponent<BulletController>();
                    pillarController.isPillarOfLight = true;

                    //Initialization
                    pillarController.InitializePillarOfLight(bulletTypes[1].sprite, bulletTypes[1]);

                    foreach (GameObject pillar in warningPillarsToDisable)
                    {
                        warningPillarsToDisable.Remove(pillar);
                        ObjectPool.instance.ReturnToPool(pillar);
                    }
                }

            }
            // Vertical Pillars
            else if (i == 1)
            {

            }
            // Diagonal Left Pillars
            else if (i == 2)
            {

            }
            //Diagonal Right Pillars
            else if (i == 3)
            {

            }
            //Boss starts getting harder after 4 repetitions
            else if (i == 4)
            {

            }

            yield return new WaitForSeconds(delayBetweenPatterns);
        }
        boss.RevealBoss();
        spellRoutine = null;
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
}
