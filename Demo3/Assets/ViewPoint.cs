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
    
    void Start()
    {
        main.rect = new Rect(0, 0, 0.495f, 1f);
        V.rect = new Rect(0.5f, 0, 0.245f, 0.49f);
        H.rect = new Rect(0.5f, 0, 0.245f, 0.49f);
        Block.rect = new Rect(0.5f, 0.5f, 0.245f, 0.5f);
        UI.rect = new Rect(0.75f, 0, 0.25f, 1);
        V.enabled = true;
        H.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeView()
    {
        if(V.enabled==true)
        {
            V.enabled = false;
            H.enabled = true;
        }
        else if(V.enabled==false)
        {
            V.enabled = true;
            H.enabled = false;
        }

    }
}
