using UnityEngine;

namespace DefaultNamespace
{
    public abstract class Destructible: MonoBehaviour
    {
        public Destructible()
        {
            
        }

        public abstract void GetHit();

        public abstract void SwitchToShattered();
    }
}