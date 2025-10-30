using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private GameObject inputFadePanel;
    [field : SerializeField] public UnityEvent OnCompleteCallBack { get; private set; } = new ();
    private float fadeTimer = 0f;
    private float fadeTime = 1.5f;
    private int fadeCount = 0;


    private void Awake()
    {
        
    }

    private void Start()
    {
        // 비어있을 경우 로그 출력
        if(inputFadePanel == null)
        {
            Debug.Log("패널이 비어있습니다.");
            return;
        }

    }

    private void Update()
    {
        fadeTimer += Time.deltaTime;

        // 페이드 인 실행
        if(fadeTimer <= fadeTime)
        {
            inputFadePanel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f, 0f, fadeTimer / fadeTime ));
            //Debug.Log("페이드 인 실행 중입니다");
        }
        //페이드 실행이 끝나면 콜백 함수 호출

        if(fadeTimer > fadeTime && fadeCount < 1) 
        {
            Debug.Log("페이드 인이 종료됐습니다.");
            //InvokeOnCompleteCallBack(); 
            // 페이드 인 끝나면 비활성화
            gameObject.SetActive(false);
            fadeCount++;
        }
       
        

    }

    public void InvokeOnCompleteCallBack()
    {
        // 페이드 인 완료 후 콜백 함수 실행
        OnCompleteCallBack?.Invoke();
    }

    
}
