using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Picking : MonoBehaviour
{
    #region 싱글톤
    public static Picking instance = null; 
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
    public GameObject cur_pick, pre_pick;
    public Color curpick_Color, prepick_Color;

    public void pickListItem(string name)
    {

        //second click
        if (cur_pick)
        {
            //같은 버튼을 눌렀을때 예외처리하기
            if (cur_pick.name == name)
            {
                //Debug.Log("현재 보고있는 블럭입니다.");
                return;
            }
            //
            pre_pick = cur_pick;
            prepick_Color = curpick_Color;
            pre_pick.GetComponent<Renderer>().material.color = prepick_Color;

        }

        cur_pick = GameObject.Find(name);
        curpick_Color = cur_pick.GetComponent<Renderer>().material.color;
        cur_pick.GetComponent<Renderer>().material.color = Color.red;

    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // left mouse click
        {
            if (!EventSystem.current.IsPointerOverGameObject()) //not UI click
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {

                    //second click
                    if (cur_pick)
                    {
                        if (cur_pick == hit.collider.gameObject) return;
                        pre_pick = cur_pick;
                        prepick_Color = curpick_Color;
                        pre_pick.GetComponent<Renderer>().material.color = prepick_Color;

                    }

                    cur_pick = hit.collider.gameObject;
                    curpick_Color = cur_pick.GetComponent<Renderer>().material.color;
                    cur_pick.GetComponent<Renderer>().material.color = Color.red;
                    UIManager.instance.loadMeshData(cur_pick.name);
                    MeshGenerator.instance.name = cur_pick.name;
                    Debug.Log(cur_pick.name);

                    //boolean btn click
                    if (Boolean.instance.start_operator)
                    {
                        Boolean.instance.Bool(pre_pick, cur_pick, prepick_Color, curpick_Color);
                    }

                }
            }
        }
    }

}
