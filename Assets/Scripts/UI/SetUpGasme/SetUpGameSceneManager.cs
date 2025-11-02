using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUpGameSceneManager : MonoBehaviour
{
    [Header("카메라")]
    [SerializeField] private GameObject inputSelectFactionCamera;
    [SerializeField] private GameObject inputSelectDifficultyCamera;
    [Header("캔버스")]
    [SerializeField] private GameObject inputSelectFactionCanvas;
    [SerializeField] private GameObject inputSelectDifficultyCanvas;

    


    private void Awake()
    {
        Init();
    }

    
    private void Init()
    {
        // 카메라 캔버스 초기 설정
        inputSelectFactionCamera.SetActive(true);
        inputSelectDifficultyCamera.SetActive(false);
        inputSelectFactionCanvas.SetActive(true);
        inputSelectDifficultyCanvas.SetActive(false);

        
        
    }

    // 진영 선택에서 난이도 선택으로 넘어간다.
    public void ChangeCamera()
    {
        inputSelectFactionCamera.SetActive(false);
        inputSelectDifficultyCamera.SetActive(true);
        inputSelectFactionCanvas.SetActive(false);
        inputSelectDifficultyCanvas.SetActive(true);
    }
}
