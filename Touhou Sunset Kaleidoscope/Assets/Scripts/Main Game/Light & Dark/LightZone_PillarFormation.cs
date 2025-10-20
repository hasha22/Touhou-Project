using UnityEngine;
namespace KH
{
    public class LightZone_PillarFormation : LightZone
    {
        public float fallSpeed = 2f;
        public float width = 1f;

        protected override void InitializeLightZone(Vector2 position)
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
