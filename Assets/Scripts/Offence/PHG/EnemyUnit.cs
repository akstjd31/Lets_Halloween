using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    //������Ÿ��
    [SerializeField] private float moveSpeed;   //ȸ���ӵ��� �̵��ӵ��� ���� �����
    private int currentWayPointIndex = 0;

    private Transform wayPointBox;
    [SerializeField] private int maxHp;
    [SerializeField] private string EnemyName;

    private int currentHp;

    Animator animator;

    bool isDie = false;     //���� ���翩��Ȯ��


    private void Start()
    {
        wayPointBox = GameObject.Find("Waypoints").transform;
        animator = GetComponent<Animator>();
        currentHp = maxHp;
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

        if (other.tag == "Endpoint")
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
        Debug.Log($"������ {damage}����");
        currentHp = currentHp - damage;

        if (currentHp <= 0 && isDie==false)
        {
            moveSpeed = 0;
            isDie = true;
            animator.SetTrigger("Die");
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
       gameObject.SetActive(false);   
    }

  

}
