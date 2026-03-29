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
            moveRoutine = boss.StartCoroutine(MovementSequence(boss.transform.position, boss, boss.rb));
        }

        public override void EndPhase(BossManager boss)
        {
            boss.StopCoroutine(attackRoutine);
            boss.StopCoroutine(moveRoutine);
        }
    }

}

