using System.Collections;
using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Boss/Attack Phase")]
    public class BossAttackPhase : BossPhase
    {
        public override void StartPhase(BossManager boss)
        {
            // same logic as enemy controller attack sequence
            attackRoutine = boss.StartCoroutine(AttackSequence(boss.transform.position, boss));

            // for later
            moveRoutine = boss.StartCoroutine(MovementSequence(boss.transform.position, boss));
        }

        public override void EndPhase(BossManager boss)
        {
            boss.StopCoroutine(attackRoutine);
            boss.StopCoroutine(moveRoutine);
        }
        private IEnumerator AttackSequence(Vector2 origin, BossManager boss)
        {
            int index = 0;
            if (phaseAttackSequence.patternSteps.Count == 0)
                yield break;

            while (phaseAttackSequence.loopPattern || index < phaseAttackSequence.patternSteps.Count)
            {
                PatternStep step = phaseAttackSequence.patternSteps[index];
                step.pattern.Fire(boss.transform.position);
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
        private IEnumerator MovementSequence(Vector2 origin, BossManager boss)
        {
            yield return null;
        }
    }

}

