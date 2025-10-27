using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();

                if (instance == null)
                {
                    GameObject singletonObj = new GameObject();
                    instance = singletonObj.AddComponent<T>();
                    singletonObj.name = typeof(T).ToString();
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        // 없으면 해당 오브젝트를 단일 인스턴스화
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        // 이미 있는 경우
        else
        {
            Destroy(this.gameObject);
        }
    }
}