using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

//���۽� ù �̹����� ���δ�. 
//2�� �̹����� ���̸�, Let's go!�� ������ ���� ��ȯ�Ǹ� ������ ���۵ȴ�.
//��ư�� ������ 3�� �̹����� �� 1�� �������δ�.
//3�� �̹����� �ε��̹����� ���ư���.
//���� �� ��ȯ�� �뷫 3����.(�ڷ�ƾ �ۼ�)


public class UIscript_T : MonoBehaviour
{
    public Image firstImage;
    public Image secondImage;
    public VideoPlayer videoPlayer;

    private Button startbutton;
    private void Start()
    {
        firstImage.gameObject.SetActive(true);
        secondImage.gameObject.SetActive(false);

        startbutton = GetComponent<Button>();
        //startbutton.onClick.AddListener(SwitchImage);

    }
    public void SwitchImage()
    {
        firstImage.gameObject.SetActive(false);
        secondImage.gameObject.SetActive(true);
    }
    //2���̹��� ���ӽ���.
    public void GameScnesCtrl()
    {
        //3�� �̹����� ����
        StartCoroutine(LoadingImage());
    }

    IEnumerator LoadingImage()
    {
        yield return new WaitForSeconds(0.5f);
        secondImage.gameObject.SetActive(false);
        SceneManager.LoadScene(1);//�ӽ�. ���̸�. ���� �� �̸� �ۼ��ؾ��� �̵���. 
    }
}
