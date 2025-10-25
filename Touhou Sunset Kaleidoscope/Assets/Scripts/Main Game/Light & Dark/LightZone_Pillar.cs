using UnityEngine;
namespace KH
{
    public class LightZone_Pillar : LightZoneBase
    {
        public float width = 2f;
        public float height = 10f;
        public float fallSpeed = 3f;

        public override void Initialize(Vector2 position)
        {
            transform.position = position;
        }
        protected override void Update()
        {
            base.Update();
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        }
    }
}
