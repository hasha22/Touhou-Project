using UnityEngine;
namespace KH
{
    public class Boss : ScriptableObject
    {
        [Header("Boss Information")]
        public string bossName;
        public Sprite bossSprite;
        public int bossHealth;
        public int bossID;
        public GameObject bossPrefab;
    }
}

