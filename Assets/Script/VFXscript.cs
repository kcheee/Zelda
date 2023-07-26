using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXscript : MonoBehaviour
{
    public List<Slash> slashes;
    [System.Serializable]
    public class Slash
    {
        public GameObject slashvfx;
        public float delay;
    }

    
    //// Start is called before the first frame update
    //void Start()
    //{
    //    DisableSlashes();
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
    //void StartSlash()
    //{
    //    StartCoroutine(SlashAttack());
    //}
    //void StopSlash()
    //{
    //    DisableSlashes();
    //}
    //void StartSlash2()
    //{
    //    StartCoroutine(SlashAttack());
    //}
    //void StopSlash2()
    //{
    //    DisableSlashes();
    //}

    //void StartSlash3()
    //{
    //    StartCoroutine(SlashAttack());
    //}
    //void StopSlash3()
    //{
    //    DisableSlashes();
    //}

    //IEnumerator SlashAttack()
    //{
    //    for (int i = 0; i < slashes.Count; i++)
    //    {
    //        yield return new WaitForSeconds(slashes[i].delay);
    //        slashes[i].slashvfx.SetActive(true);
    //    }
    //    yield return new WaitForSeconds(1);
    //}
    //void DisableSlashes()
    //{
    //    for (int i = 0; i < slashes.Count; i++)
    //        slashes[i].slashvfx.SetActive(false);
    //}
}
