using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour
{
    public static MaterialChange instance;
    private void Awake()
    {
        instance = this;
    }


    public SkinnedMeshRenderer[] skinnedMeshRenderers;
    public Material[] originalMaterials;
    public Material damageMaterial;
    public float damageTime = 0.1f;
   IEnumerator IEDamage()
    {
        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
            skinnedMeshRenderers[i].material = damageMaterial;
        }

        yield return new WaitForSeconds(damageTime);

        for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
            for (int j = 0; j < i+1; j++)
            {
                skinnedMeshRenderers[i].material = originalMaterials[j];
            }
        }
    }

    Coroutine crt;
    public void DoDamage()
    {
        if (crt != null)
            StopCoroutine(crt);

        crt = StartCoroutine(IEDamage());
    }


}
