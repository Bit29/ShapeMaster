using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEvent : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        pressKey();
    }

    private void pressKey()
    {
        if(Input.GetKeyUp(KeyCode.I))
        {
            UIManager.instance.openMainMenu();
        }
        if(Input.GetKeyUp(KeyCode.V))
        {
            ViewPoint.instance.changeView();
        }
        if(Input.GetKeyUp(KeyCode.Tab))
        {

        }
    }
}
