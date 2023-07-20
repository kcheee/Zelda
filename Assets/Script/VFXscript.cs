using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXscript : MonoBehaviour
{
    public List<Slash> slashes;
    // Start is called before the first frame update
    void Start()
    {
        DisableSlashes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void StartSlash()
    {
        StartCoroutine(SlashAttack());
    }
    
    void StartSlash2()
    {
        StartCoroutine(SlashAttack());
    }

    
    IEnumerator SlashAttack()
    {
        for (int i = 0; i < slashes.Count; i++)
        {
            yield return new WaitForSeconds(slashes[i].delay);
            slashes[i].slashvfx.SetActive(true);
        }
        yield return new WaitForSeconds(1);
        DisableSlashes();
    }
    void DisableSlashes()
    {
        for (int i = 0; i < slashes.Count; i++)
            slashes[i].slashvfx.SetActive(false);
    }
    [System.Serializable]
    public class Slash
    {
        public GameObject slashvfx;
        public float delay;
    }
}
