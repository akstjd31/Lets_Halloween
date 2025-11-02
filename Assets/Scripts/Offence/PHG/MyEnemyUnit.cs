using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyEnemyUnit : Unit
{
    //moveSpeed rotateSpeed ????
    [SerializeField] private float moveSpeed;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    private int currentWayPointIndex = 0;

    private Transform wayPointBox;
    [SerializeField] private int hp;
    [SerializeField] private string EnemyName;

    //???????? ?占쏙옙???? ??占폧????
    [SerializeField] private GameObject[] myEnemyUnitArray;

    //?? ???? ??????
    [SerializeField] private float spawnDelay;
    private float spawnTime = 0;

    //???? ???????? ????
    [SerializeField] private int maxUnitCount = 20;
    private int currentUnitCount;


    List<Renderer> renderers = new List<Renderer>();    //?????? ???? (????????????????)
    List<Color> originalRenderColor = new List<Color>();


    private void Start()
    {
        transform.position = GameObject.FindWithTag("StartPoint").transform.position;
        wayPointBox = GameObject.Find("WayPoint").transform;
        currentUnitCount = 0;

        renderers.AddRange(GetComponentsInChildren<Renderer>());

        foreach (var render in renderers)
        {
            originalRenderColor.Add(render.material.color); //???????? ???
        }
    }

    private void OnEnable()
    {
        transform.position = GameObject.FindWithTag("StartPoint").transform.position;
    }

    //????????? ?占쏙옙???
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("WayPoint"))
        {
            if (wayPointBox == null) { return; }

            currentWayPointIndex++;
        }

        if(other.tag=="EndPoint")
        {
            Debug.Log($"{gameObject.name} ?????? ????");
            gameObject.SetActive(false);
        }
    }

    //??????? ???? ??????? ????
    public void StatusEffectColor(PassiveSkill playerPassive)
    {
        switch (playerPassive)
        {
            case PassiveSkill.Slow:
                foreach (var renderer in renderers)
                {
                    renderer.material.color = Color.blue;
                }
                break;

            case PassiveSkill.Stun:
                foreach (var renderer in renderers)
                {
                    renderer.material.color = Color.yellow;
                }
                break;
        }
    }

    //???? ?????
    public void ReturnStatusEffectColor()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material.color = originalRenderColor[i];
        }
    }

    private void Update()
    {
        spawnTime += Time.deltaTime;
        MoveObj();

        if (currentUnitCount < maxUnitCount)
        {
            CreateEnemyUnit();
        }
    }

    //??????? ???
    private void MoveObj()
    {
        if (wayPointBox == null || currentWayPointIndex >= wayPointBox.childCount) { return; }

        Transform targetTransform = wayPointBox.GetChild(currentWayPointIndex);
        Vector3 direction = (targetTransform.position - transform.position).normalized;

        transform.position = Vector3.MoveTowards(transform.position,targetTransform.position,moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), moveSpeed * Time.deltaTime);

        if (direction == Vector3.zero)
        {
            currentWayPointIndex++;
        }
    }
    //?????
    private void CreateEnemyUnit()
    {
        switch (Input.inputString.ToUpper())
        {
            case "Q":
                SpawnUnit(0, currentWayPointIndex);
                break;

            case "W":
                SpawnUnit(1, currentWayPointIndex);
                break;

            case "E":
                SpawnUnit(2, currentWayPointIndex);
                break;

            case "R":
                SpawnUnit(3, currentWayPointIndex);
                break;

            case "T":
                SpawnUnit(4, currentWayPointIndex);
                break;
        }
    }

    //???占쏙옙? ???? ???? ???
    private void SpawnUnit(int mobIndex, int WayPointIndex)
    {
        if (spawnTime >= spawnDelay)
        {
            GameObject mob = Instantiate(myEnemyUnitArray[mobIndex], transform.position, transform.rotation);
            mob.GetComponent<EnemyUnit>().SetWayPoint(currentWayPointIndex);
            spawnTime = 0;
            currentUnitCount++;
        }
    }
}
