using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPoint : MonoBehaviour
{
    #region 싱글톤
    public static ViewPoint instance = null;
    private void Awake()
    {
        if (instance == null) //instance가 null. 즉, 시스템상에 존재하고 있지 않을때
        {
            instance = this; //내자신을 instance로 넣어줍니다.
            DontDestroyOnLoad(gameObject); //OnLoad(씬이 로드 되었을때) 자신을 파괴하지 않고 유지
        }
        else
        {
            if (instance != this) //instance가 내가 아니라면 이미 instance가 하나 존재하고 있다는 의미
                Destroy(this.gameObject); //둘 이상 존재하면 안되는 객체이니 방금 AWake된 자신을 삭제
        }
    }
    #endregion
    // Start is called before the first frame update
    public Camera main, H, V, UI, Block;
    int setWidth = 1920; // 사용자 설정 너비
    int setHeight = 1080; // 사용자 설정 높이
    int deviceWidth;
    int deviceHeight;
    void Start()
    {
        main.rect = new Rect(0, 0, 0.495f, 1f);
        V.rect = new Rect(0.5f, 0, 0.245f, 0.49f);
        H.rect = new Rect(0.5f, 0, 0.245f, 0.49f);
        Block.rect = new Rect(0.5f, 0.5f, 0.245f, 0.5f);
        UI.rect = new Rect(0.75f, 0, 0.25f, 1);
        V.enabled = true;
        H.enabled = false;
        deviceWidth = Screen.width; // 기기 너비 저장
        deviceHeight = Screen.height; // 기기 높이 저장
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void changeView()
    {
        if (V.enabled == true)
        {
            V.enabled = false;
            H.enabled = true;
        }
        else if (V.enabled == false)
        {
            V.enabled = true;
            H.enabled = false;
        }

    }
    public void SetResolution()
    {
        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기
        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }
}
