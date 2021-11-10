using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class btnEvent : MonoBehaviour
{
    #region MainMenu
    public void clickMeshBtn()
    {
        UIManager.instance.openMeshMenu();
        UIManager.instance.initMeshMenu();
        Preview.instance.DestoryBlock();
    }


    public void clickListBtn()
    {
        UIManager.instance.loadList();
        UIManager.instance.openListMenu();

    }
    public void clickFuncBtn()
    {
        UIManager.instance.openFuncMenu();
    }
    #endregion
    #region MeshMenu
    public void clickCreateBtn()
    {
        UIManager.instance.setMeshMenu();
        GameObject sup=MeshGenerator.instance.createBlock();
        MeshGenerator.instance.translateBlock(sup);
        MeshGenerator.instance.rotateBlock(sup);
        BlockData.instance.addBlockList(sup.name);
        Preview.instance.start_pre = true;
       
    }
    public void clickModifyBtn()
    {
        UIManager.instance.setMeshMenu();
        BlockData.instance.updateBlockList(MeshGenerator.instance.name);
        MeshGenerator.instance.deleteBlock();
        GameObject sup = MeshGenerator.instance.modifyBlock();
        MeshGenerator.instance.translateBlock(sup);
        MeshGenerator.instance.rotateBlock(sup);
    }

    public void clickDeleteBtn()
    {
        MeshGenerator.instance.deleteBlock();
        BlockData.instance.deleteBlockList(MeshGenerator.instance.name);
    }
    #endregion
    #region ListMenu
    public void clickListItem()
    {
        MeshGenerator.instance.name = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMPro.TMP_Text>().text;
        UIManager.instance.loadMeshData(MeshGenerator.instance.name);
        Picking.instance.pickListItem(MeshGenerator.instance.name);

    }
    #endregion
    #region FunctionMenu
    public void clickUnionBtn()
    {

    }
    public void clickSubtractBtn()
    {

    }
    public void clickIntersectBtn()
    {

    }
    #endregion
}
