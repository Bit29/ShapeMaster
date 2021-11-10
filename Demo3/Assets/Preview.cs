using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Preview : MonoBehaviour
{
    #region 싱글톤
    public static Preview instance = null;
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
    public bool start_pre = false;
    GameObject preblock;
    public void previewBlock()
    {
        Destroy(preblock);
        preblock = MeshGenerator.instance.previewBlock();
        preblock.transform.Translate(100, 100, 100);
        MeshGenerator.instance.rotateBlock(preblock);
    }
    public void DestoryBlock()
    {
        if (preblock!=null)
        {
            Destroy(preblock);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
