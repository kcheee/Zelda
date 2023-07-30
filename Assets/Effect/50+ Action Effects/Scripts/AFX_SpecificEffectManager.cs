namespace TheDeveloper.AFX
{
    using UnityEngine;

    public class AFX_SpecificEffectManager : MonoBehaviour
    {
        public GameObject[] effects;
        private int selectedEffect = 0;

        public bool destroyChildren = false;

        public void PriorEffect() {
            if(!AtStart())
                --selectedEffect;
        }
        public void NextEffect() {
            if(!AtFinish())
                ++selectedEffect;
        }
        public void Spawn(bool keepRotation) {
            GameObject instance = null;
            if(keepRotation)
                instance = Instantiate(effects[selectedEffect], transform.position, Quaternion.LookRotation(transform.forward));
            else
                instance = Instantiate(effects[selectedEffect], transform.position, Quaternion.identity);
            
            if(destroyChildren) {
                if(instance.TryGetComponent<ParticleSystem>(out var ps)) {
                    Destroy(instance, ps.main.duration * 1.2f);
                }
            }
        }
        public bool AtFinish() { return selectedEffect + 1 >= effects.Length; }
        public bool AtStart() { return selectedEffect <= 0; }
    }
}