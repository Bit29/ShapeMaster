using Parabox.CSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boolean : MonoBehaviour
{
    #region 싱글톤
    public static Boolean instance = null;
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
    public GameObject obj_copy;
    public bool start_operator = false;
    public enum booltype { uni, sub, inter };
    public booltype flag;
    public void Bool(GameObject operand1, GameObject operand2, Color prepick_Color, Color curpick_Color)
    {

        Model result=null;
        switch (flag)
        {
            case booltype.uni:
                {
                    operand1.GetComponent<Renderer>().material.color = prepick_Color;
                    operand2.GetComponent<Renderer>().material.color = curpick_Color;
                    result = CSG.Union(operand1, operand2);
                }
                break;
            case booltype.sub:
                {
                    operand1.GetComponent<Renderer>().material.color = prepick_Color;
                    operand2.GetComponent<Renderer>().material.color = prepick_Color;
                    result = CSG.Subtract(operand1, operand2);
                }
                break;
            case booltype.inter:
                {
                    operand1.GetComponent<Renderer>().material.color = prepick_Color;
                    operand2.GetComponent<Renderer>().material.color = prepick_Color;
                    result = CSG.Intersect(operand1, operand2);
                }
                break;
        }

        var composite = new GameObject("bool_obj");
        composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        Destroy(operand1);
        Destroy(operand2);
        composite.AddComponent<BoxCollider>();
        start_operator = false;
    }
   
}
