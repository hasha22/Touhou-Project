using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Boss/Spell Card Phase")]
    public class BossSpellCardPhase : BossPhase
    {
        public override void StartPhase(BossManager boss)
        {
            Debug.Log($"Starting Spell Card: {phaseName}");
            // make UI changes here
            // begin attack sequence here
        }

        public override void EndPhase(BossManager boss)
        {
            // transition to next phase or kill boss
        }
    }
}

