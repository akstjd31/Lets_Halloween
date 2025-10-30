using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ParentButton : MonoBehaviour
{

    public Button button;

    private void Awake()
    {
       button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(OnClickButton);

        Debug.Log(button.onClick);
    }

    

    virtual public void OnClickButton()
    {
        MouseTrackingManager.Instance.SetActiveButton(this);
        //자식에서 오버라이드
    }
}


