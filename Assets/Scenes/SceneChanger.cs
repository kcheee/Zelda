using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // ���� �Ⱦ�.
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(4.5f);
        SceneManager.LoadScene(2);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (SceneManager.GetActiveScene().name == "Main_Scene")
            {
                Debug.Log("����");
                SceneManager.LoadScene(1);
            }
            if (SceneManager.GetActiveScene().name == "GetReadyfor")
            {
                Debug.Log("����22");
                SceneManager.LoadScene(2);
            }
        }

    }
}
