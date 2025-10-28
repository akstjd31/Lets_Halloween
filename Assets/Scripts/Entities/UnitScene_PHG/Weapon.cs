using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //투사체 넣을 프리팹
    [SerializeField] private GameObject projectilePrefab;
    //투사체 풀 사이즈
    [SerializeField] private int projectilePoolSize;

    private int projectileCount;
    GameObject[] projectilePrefabs;

    Transform targetTransform;


    private void Start()
    {
        Init();
    }

    void Init()
    {
        projectilePrefabs = new GameObject[projectilePoolSize];
        projectileCount = projectilePoolSize;   //풀 충당

        for(int i=0; i<projectilePrefabs.Length; i++)
        {
            projectilePrefabs[i] = Instantiate(projectilePrefab,transform.position , Quaternion.identity);
            projectilePrefabs[i].SetActive(false);  //비활성화
        }
    }
    public void Shoot(Transform target)
    {
        foreach (var obj in projectilePrefabs) 
        {
            //현재 활성화되어있는지 확인후 비활이면 활성화
            if(!obj.activeSelf)
            {
                obj.transform.position = transform.position; 
                obj.SetActive(true);
                obj.GetComponent<Projectile>().SetTarget(target);
                return;
            }
        }
    }




}
