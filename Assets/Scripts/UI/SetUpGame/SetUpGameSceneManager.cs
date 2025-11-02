using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUpGameSceneManager : MonoBehaviour
{
    [Header("ī�޶�")]
    [SerializeField] private GameObject inputSelectFactionCamera;
    [SerializeField] private GameObject inputSelectDifficultyCamera;
    [Header("ĵ����")]
    [SerializeField] private GameObject inputSelectFactionCanvas;
    [SerializeField] private GameObject inputSelectDifficultyCanvas;

    


    private void Awake()
    {
        Init();
    }

    
    private void Init()
    {
        // ī�޶� ĵ���� �ʱ� ����
        inputSelectFactionCamera.SetActive(true);
        inputSelectDifficultyCamera.SetActive(false);
        inputSelectFactionCanvas.SetActive(true);
        inputSelectDifficultyCanvas.SetActive(false);
    }

    // ���� ���ÿ��� ���̵� �������� �Ѿ��.
    public void ChangeCamera()
    {
        inputSelectFactionCamera.SetActive(false);
        inputSelectDifficultyCamera.SetActive(true);
        inputSelectFactionCanvas.SetActive(false);
        inputSelectDifficultyCanvas.SetActive(true);
    }
}
