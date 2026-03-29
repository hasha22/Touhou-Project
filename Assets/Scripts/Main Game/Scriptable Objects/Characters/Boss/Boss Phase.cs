using System.Collections;
using UnityEngine;
namespace KH
{
    public class BossPhase : ScriptableObject
    {
        [Header("Phase Information")]
        public int phaseBossHealth;
        public string phaseName;
        public float duration;
        public bool isSpellCard;
        public MovementSequence phaseMovementSequence;
        public AttackSequence phaseAttackSequence;
        public float delayBeforeNextPhase;

        [Header("Universal Routines")]
        protected Coroutine attackRoutine;
        protected Coroutine moveRoutine;

        public virtual void StartPhase(BossManager boss) { }
        public virtual void EndPhase(BossManager boss) { }
        protected virtual IEnumerator AttackSequence(Vector2 origin, BossManager boss)
        {
            int index = 0;
            if (phaseAttackSequence.patternSteps.Count == 0)
                yield break;

            while (phaseAttackSequence.loopPattern || index < phaseAttackSequence.patternSteps.Count)
            {
                PatternStep step = phaseAttackSequence.patternSteps[index];
                step.pattern.Fire(boss.transform.position, boss.gameObject);

                yield return new WaitForSeconds(step.delayBeforeNextPattern);

                index++;

                if (index >= phaseAttackSequence.patternSteps.Count)
                {
                    if (phaseAttackSequence.loopPattern)
                        index = 0;
                    else
                        break;
                }
            }
        }
        protected virtual IEnumerator MovementSequence(Vector2 origin, BossManager boss, Rigidbody2D rb)
        {
            int index = 0;
            if (phaseMovementSequence.movementSteps.Count == 0)
                yield break;

            // runs each step in the movement sequence
            while (index < phaseMovementSequence.movementSteps.Count || phaseMovementSequence.loopSequence)
            {
                MovementStep step = phaseMovementSequence.movementSteps[index];
                float elapsed = 0f;

                Vector2 startPosition = rb.position;

                while (elapsed < step.duration)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / step.duration);

                    // get total movement
                    Vector2 totalOffset = step.pattern.GetTotalMovement(boss.transform, step.duration);
                    Vector2 targetPosition = startPosition + totalOffset;

                    // lerp to target position
                    rb.MovePosition(Vector2.Lerp(startPosition, targetPosition, t));

                    yield return null;
                }
                // final check to ensure enemy is on target
                Vector2 finalOffset = step.pattern.GetTotalMovement(boss.transform, step.duration);
                rb.MovePosition(startPosition + finalOffset);

                if (step.delayBeforeNext > 0)
                    yield return new WaitForSeconds(step.delayBeforeNext);

                index++;
                if (index >= phaseMovementSequence.movementSteps.Count)
                {
                    if (phaseMovementSequence.loopSequence)
                        index = 0;
                    else
                        break;
                }
            }
        }

    }
}


