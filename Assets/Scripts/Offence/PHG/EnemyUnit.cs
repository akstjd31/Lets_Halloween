using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{

    [SerializeField] private string _name;
    [SerializeField] private string _story;
    [SerializeField] private string _any;
    [SerializeField] private float _hp;
    [SerializeField] private int _price;

    public string Name => _name;
    public string Story => _story;
    public string Any => _any;
    public float HP => _hp;
    public int Price => _price;


    //프로토타입
    public float moveSpeedOrigin;   //회전속도는 이동속도와 같게 맞출것

    public float MoveSpeed { get; set; }

    private int currentWayPointIndex = 0;

    private Transform wayPointBox;
    [SerializeField] private int maxHp;
    [SerializeField] private string EnemyName;

    private int currentHp;

    Animator animator;

    public bool isDie = false;     //유닛 생사여부확인

    List<Renderer> renderers = new List<Renderer>();    //렌더러 저장 (피격판정추가를위한)
    List<Color> originalRenderColor = new List<Color>();

    Vector3 firstPoint;



    private void Start()
    {
        MoveSpeed = moveSpeedOrigin;
        wayPointBox = GameObject.Find("WayPoint").transform;
        animator = GetComponent<Animator>();
        currentHp = maxHp;
        renderers.AddRange(GetComponentsInChildren<Renderer>());
        firstPoint = transform.position;

        foreach (var render in renderers)
        {
            originalRenderColor.Add(render.material.color); //원래색상 추가
        }
    }

    //웨이포인트 충돌처리
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("WayPoint"))
        {
            if (wayPointBox == null) { return; }

            currentWayPointIndex++;

            if (currentWayPointIndex >= wayPointBox.childCount) { currentWayPointIndex = 0; }
        }

        if (other.tag == "EndPoint")
        {
            Debug.Log($"{gameObject.name} 끝지점 도달");
            gameObject.SetActive(false);
        }

    }

    private void OnEnable()
    {
        isDie = false;
        MoveSpeed = moveSpeedOrigin;
    }

    private void Update()
    {
        if (!isDie)
            MoveObj();
    }

    //오브젝트 이동
    private void MoveObj()
    {
        if (wayPointBox == null || currentWayPointIndex >= wayPointBox.childCount)
        { return; }

        Transform targetTransform = wayPointBox.GetChild(currentWayPointIndex);
        Vector3 direction = (targetTransform.position - transform.position).normalized;

        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, MoveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), MoveSpeed * Time.deltaTime);

        if (direction == Vector3.zero)
        {
            currentWayPointIndex++;
        }
    }

    public void TakeDamage(int damage)
    {
        //Debug.Log($"데미지 {damage}입음");
        currentHp = currentHp - damage;

        if (currentHp <= 0 && isDie == false)
        {
            MoveSpeed = 0;
            isDie = true;
            animator.SetTrigger("Die");
        }
    }

    //유닛색상 지정
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

    //색상 원상복귀
    public void ReturnStatusEffectColor()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material.color = originalRenderColor[i];
        }
    }

    //생성시 플레이어가 가는위치 던져줌
    public void SetWayPoint(int wayPointIndex)
    {
        currentWayPointIndex = wayPointIndex;
    }

    //유니티 이벤트 함수
    void Die()
    {
        //Destroy(gameObject);
        WaveManager.Instance.OnEnemyDeactivated(this.GetComponent<Enemy>());
    }

    public void Teleport()
    {
        Debug.Log("텔레포트 작동함");
        Transform targetTransform = wayPointBox.GetChild(currentWayPointIndex);
        transform.position = new Vector3(firstPoint.x, firstPoint.y, firstPoint.z);

        //transform.position = targetTransform.position;
    }

    public void BerserkBuff(float power, float duration)
    {
        StartCoroutine(BerserkBuffTimer(power, duration));
    }

    private IEnumerator BerserkBuffTimer(float power, float duration)
    {
        MoveSpeed += moveSpeedOrigin * (power / 100f);

        float durationTimer = 0f;

        while (durationTimer < duration)
        {
            durationTimer += Time.deltaTime;

            yield return null;
        }

        MoveSpeed = moveSpeedOrigin;
    }
}
