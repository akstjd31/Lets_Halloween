using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    //������Ÿ��
    [SerializeField] private float moveSpeed;   //ȸ���ӵ��� �̵��ӵ��� ���� �����

    public float MoveSpeed {get => moveSpeed; set => moveSpeed = value; }

    public int currentWayPointIndex { get; set; }

    private Transform wayPointBox;
    [SerializeField] private int maxHp;
    [SerializeField] private string EnemyName;

    private int currentHp;

    Animator animator;

    bool isDie = false;     //���� ���翩��Ȯ��

    List<Renderer> renderers = new List<Renderer>();    //������ ���� (�ǰ������߰�������)
    List<Color> originalRenderColor = new List<Color>();



    private void Start()
    {
        currentWayPointIndex = 0;
        wayPointBox = GameObject.Find("Waypoints").transform;
        animator = GetComponent<Animator>();
        currentHp = maxHp;
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        foreach (var render in renderers)
        {
            originalRenderColor.Add(render.material.color); //�������� �߰�
        }
    }
    
    


    //��������Ʈ �浹ó��
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Waypoints"))
        {
            if (wayPointBox == null) { return; }

            currentWayPointIndex++;

            if (currentWayPointIndex >= wayPointBox.childCount) { currentWayPointIndex = 0; }
        }

        if (other.tag == "EndPoint")
        {
            Debug.Log($"{gameObject.name} ������ ����");
            gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        MoveObj();
    }

    //������Ʈ �̵�
    private void MoveObj()
    {
        if (wayPointBox == null) { return; }

        Transform targetTransform = wayPointBox.GetChild(currentWayPointIndex);
        Vector3 direction = (targetTransform.position - transform.position).normalized;

        transform.position = Vector3.MoveTowards(transform.position,targetTransform.position,moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), moveSpeed * Time.deltaTime);

        if (direction == Vector3.zero)
        {
            currentWayPointIndex++;
        }
    }

    public void TakeDamage(int damage)
    {
        //Debug.Log($"������ {damage}����");
        currentHp = currentHp - damage;

        if (currentHp <= 0 && isDie==false)
        {
            moveSpeed = 0;
            isDie = true;
            animator.SetTrigger("Die");
        }
    }
 
    //���ֻ��� ����
    public void StatusEffectColor(PassiveSkill playerPassive)
    {
        switch(playerPassive)
        {
            case PassiveSkill.Slow:
                foreach(var renderer in renderers)
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

    //���� ���󺹱�
    public void ReturnStatusEffectColor()
    {
        for(int i=0; i< renderers.Count; i++)
        {
            renderers[i].material.color = originalRenderColor[i];
        }
    }

    //������ �÷��̾ ������ġ ������
    public void SetWayPoint(int wayPointIndex)
    {
        currentWayPointIndex = wayPointIndex;
    }

    //����Ƽ �̺�Ʈ �Լ�
    void Die()
    {
        //Destroy(gameObject);
       gameObject.SetActive(false);   
    }

  

}
