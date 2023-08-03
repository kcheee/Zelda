using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

//시작시 첫 이미지가 보인다. 
//2번 이미지가 보이며, Let's go!를 누르면 씬이 전환되며 게임이 시작된다.
//버튼을 누르면 3번 이미지가 약 1초 정도보인다.
//3번 이미지의 로딩이미지가 돌아간다.
//다음 씬 전환은 대략 3초후.(코루틴 작성)


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
    //2번이미지 게임시작.
    public void GameScnesCtrl()
    {
        //3번 이미지를 등장
        StartCoroutine(LoadingImage());
    }

    IEnumerator LoadingImage()
    {
        yield return new WaitForSeconds(0.5f);
        secondImage.gameObject.SetActive(false);
        SceneManager.LoadScene(1);//임시. 씬이름. 게임 씬 이름 작성해야지 이동함. 
    }
}
