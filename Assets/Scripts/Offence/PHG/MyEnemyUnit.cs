using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    private MonsterUnitButton monsterUnitButton;
    private SkillButtonGoel buttonGoel;
    public bool clickSkill;

    private void Start()
    {
        transform.position = GameObject.FindWithTag("StartPoint").transform.position;
        wayPointBox = GameObject.Find("WayPoint").transform;
        currentUnitCount = 0;
        buttonGoel = GameObject.Find("SkillButton_3").GetComponent<SkillButtonGoel>();
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
                monsterUnitButton = GameObject.Find("UnitButton").GetComponent<MonsterUnitButton>();
                SpawnUnit(0, currentWayPointIndex);
                break;

            case "W":
                monsterUnitButton = GameObject.Find("UnitButton_1").GetComponent<MonsterUnitButton>();
                SpawnUnit(1, currentWayPointIndex);
                break;

            case "E":
                monsterUnitButton = GameObject.Find("UnitButton_2").GetComponent<MonsterUnitButton>();
                SpawnUnit(2, currentWayPointIndex);
                break;

            case "R":
                monsterUnitButton = GameObject.Find("UnitButton_3").GetComponent<MonsterUnitButton>();
                SpawnUnit(3, currentWayPointIndex);
                break;
        }
    }

    public void SpawnGoal()
    {
        clickSkill = true;
        SpawnUnit(4, currentWayPointIndex);
    }

    //???占쏙옙? ???? ???? ???
    private void SpawnUnit(int mobIndex, int WayPointIndex)
    {
        if (spawnTime >= spawnDelay)
        {
            if(clickSkill == true && buttonGoel.skillCount > 0)
            {
                GameObject mob = Instantiate(myEnemyUnitArray[mobIndex], transform.position, transform.rotation);
                mob.GetComponent<EnemyUnit>().SetWayPoint(currentWayPointIndex);
                spawnTime = 0;
                currentUnitCount++;
                buttonGoel.RemoveCount();
                clickSkill = false;
            }
           
            else if (monsterUnitButton.unitCount > 0)
            {
                GameObject mob = Instantiate(myEnemyUnitArray[mobIndex], transform.position, transform.rotation);
                mob.GetComponent<EnemyUnit>().SetWayPoint(currentWayPointIndex);
                spawnTime = 0;
                currentUnitCount++;
                monsterUnitButton.RemoveCount();
            }
        }
    }
}
