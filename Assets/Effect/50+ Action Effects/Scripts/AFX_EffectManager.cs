namespace TheDeveloper.AFX
{
    using UnityEngine;

    public class AFX_EffectManager : MonoBehaviour
    {
        public AFX_SpecificEffectManager projectiles, areaEffects;
        private bool first = true;
        public GameObject projectilesAdditionalObject;

        private void Start() { 
            projectilesAdditionalObject.SetActive(true);
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.D)) {
                if(first) {
                    projectiles.NextEffect();
                    if(projectiles.AtFinish()) { 
                        first = false;
                        projectilesAdditionalObject.SetActive(false);
                    }
                } else
                    areaEffects.NextEffect();
            }
            else if(Input.GetKeyDown(KeyCode.A)) {
                if(first)
                    projectiles.PriorEffect();
                else {
                    if(areaEffects.AtStart()) {
                        first = true;
                        projectilesAdditionalObject.SetActive(true);
                    }
                    else
                        areaEffects.PriorEffect();
                }
            }

            if(Input.GetKeyDown(KeyCode.W)) {
                if(first)
                    projectiles.Spawn(true);
                else
                    areaEffects.Spawn(true);
            }
        }
    }
}