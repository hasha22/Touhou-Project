using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Boss/Attack Phase")]
    public class BossAttackPhase : BossPhase
    {
        public override void StartPhase(BossManager boss)
        {
            Debug.Log($"Starting Attack Phase: {phaseName}");
            //add logic for shooting here
            //boss.Spawner.StartPattern(this);
        }

        public override void EndPhase(BossManager boss)
        {
            Debug.Log($"Ending Attack Phase: {phaseName}");
            // transition to spell card phase here
            //boss.Spawner.StopPattern();
        }
    }

}

