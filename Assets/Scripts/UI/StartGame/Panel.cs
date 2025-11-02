using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    private void Update()
    {
        if (gameObject.activeSelf && Input.anyKeyDown)
        {
            // 오브젝트가 활성화 되어 있을 경우 아무 키 입력을 받으면 비활성화 된다.
            gameObject.SetActive(false);
        }
    }
}
