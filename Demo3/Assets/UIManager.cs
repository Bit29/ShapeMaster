using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region 싱글톤
    public static UIManager instance = null;
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
    #region UI
    public Image mainMenu, meshMenu, funcMenu;
    public Slider rounding;
    public TMPro.TMP_InputField topA, topB, bottomA, bottomB, rX, rY, heightT, heightB, centerX, centerY, centerZ, rotate;
    public ScrollRect listMenu;
    public Button listItem;
    #endregion
    #region UI Function
    public void openMainMenu()
    {
        mainMenu.gameObject.SetActive(true);
        meshMenu.gameObject.SetActive(false);
        listMenu.gameObject.SetActive(false);
        funcMenu.gameObject.SetActive(false);
    }
    public void openMeshMenu()
    {
        meshMenu.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
        listMenu.gameObject.SetActive(false);
        funcMenu.gameObject.SetActive(false);
    }
    public void openListMenu()
    {
        listMenu.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
        meshMenu.gameObject.SetActive(false);
        funcMenu.gameObject.SetActive(false);
    }
    public void openFuncMenu()
    {
        funcMenu.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
        meshMenu.gameObject.SetActive(false);
        listMenu.gameObject.SetActive(false);
    }
    public void setMeshMenu()
    {
        float.TryParse(topA.text, out MeshGenerator.instance.topA);
        float.TryParse(topB.text, out MeshGenerator.instance.topB);
        float.TryParse(bottomA.text, out MeshGenerator.instance.bottomA);
        float.TryParse(bottomB.text, out MeshGenerator.instance.bottomB);
        float.TryParse(rX.text, out MeshGenerator.instance.rX);
        float.TryParse(rY.text, out MeshGenerator.instance.rY);
        float.TryParse(heightT.text, out MeshGenerator.instance.heightT);
        float.TryParse(heightB.text, out MeshGenerator.instance.heightB);
        float.TryParse(centerX.text, out MeshGenerator.instance.centerX);
        float.TryParse(centerY.text, out MeshGenerator.instance.centerY);
        float.TryParse(centerZ.text, out MeshGenerator.instance.centerZ);
        float.TryParse(rotate.text, out MeshGenerator.instance.rotate);

        MeshGenerator.instance.rounding = rounding.value;
    }
    public void initMeshMenu()
    {
        topA.text = string.Empty;
        topB.text = string.Empty;
        bottomA.text = string.Empty;
        bottomB.text = string.Empty;
        rX.text = string.Empty;
        rY.text = string.Empty;
        heightT.text = string.Empty;
        heightB.text = string.Empty;
        centerX.text = "0";
        centerY.text = "0";
        centerZ.text = "0";
        rotate.text = "0";
        rounding.value = 0.0f;
    }
    public void loadMeshData(string name)
    {
        openMeshMenu();
        delay = false;

        topA.text = string.Format(""+BlockData.instance.MBlockStore[name].topA);
        topB.text = string.Format("" + BlockData.instance.MBlockStore[name].topB);
        bottomA.text = string.Format("" + BlockData.instance.MBlockStore[name].bottomA);
        bottomB.text = string.Format("" + BlockData.instance.MBlockStore[name].bottomB);
        rX.text = string.Format("" + BlockData.instance.MBlockStore[name].rX);
        rY.text = string.Format("" + BlockData.instance.MBlockStore[name].rY);
        heightT.text = string.Format("" + BlockData.instance.MBlockStore[name].heightT);
        heightB.text = string.Format("" + BlockData.instance.MBlockStore[name].heightB);
        centerX.text = string.Format("" + BlockData.instance.MBlockStore[name].centerX);
        centerY.text = string.Format("" + BlockData.instance.MBlockStore[name].centerY);
        centerZ.text = string.Format("" + BlockData.instance.MBlockStore[name].centerZ);
        rotate.text = string.Format("" + BlockData.instance.MBlockStore[name].rotate);
        rounding.value = BlockData.instance.MBlockStore[name].rounding;
        delay = true;
        return;
    }
    public void loadList()
    {
        var child= listMenu.GetComponent<ScrollRect>().content;
        for(int i=0;i<child.childCount;i++)
        {
            Destroy(child.GetChild(i).gameObject);
            
        }

        for (int i=0;i<BlockData.instance.MBlockRank.Count;i++)
        {
            listItem.GetComponentInChildren<TMPro.TMP_Text>().text = BlockData.instance.MBlockRank[i];
            var clone = Instantiate(listItem, listMenu.content);
        }
        return;
    }
    public void changeUIValue()
    {
        setMeshMenu();
        Preview.instance.previewBlock();
    }
    public bool delay = true;

    #endregion
    private void Start()
    {
        topA.text = "11";
        topB.text = "11";
        bottomA.text = "11";
        bottomB.text = "11";
        rX.text = "1";
        rY.text = "1";
        heightT.text = "11";
        heightB.text = "0";
        centerX.text = "0";
        centerY.text = "0";
        centerZ.text = "0";
        rotate.text = "0";
        openMainMenu();

        if (delay)
        {

            if (rounding != null&& topA != null&& topB != null&& bottomA != null && bottomB != null && rX != null && rY != null && heightT != null && heightB != null && rotate != null)
            {
                rounding.onValueChanged.AddListener(delegate { changeUIValue(); });
                topA.onEndEdit.AddListener(delegate { changeUIValue(); });
                topB.onEndEdit.AddListener(delegate { changeUIValue(); });
                bottomA.onEndEdit.AddListener(delegate { changeUIValue(); });
                bottomB.onEndEdit.AddListener(delegate { changeUIValue(); });
                rX.onEndEdit.AddListener(delegate { changeUIValue(); });
                rY.onEndEdit.AddListener(delegate { changeUIValue(); });
                heightT.onEndEdit.AddListener(delegate { changeUIValue(); });
                heightB.onEndEdit.AddListener(delegate { changeUIValue(); });
                rotate.onEndEdit.AddListener(delegate { changeUIValue(); });
            }
           
        }
        
    }

}




