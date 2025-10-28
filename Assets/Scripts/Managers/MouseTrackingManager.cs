using Palmmedia.ReportGenerator.Core.Reporting.Builders.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SearchService;
using UnityEngine;

public class MouseTrackingManager : MonoBehaviour
{
    [SerializeField] private GameObject targetObject; // 설치될 위치를 보여줄 프리뷰 오브젝트
    GameObject target;

    private SkillBase ActivateObject;

    private SkillCoolDown skillCool;

    [SerializeField] LayerMask layermask;

    private static bool isClick = false;

    private void Start()
    {
        // 레이어를 통해 레이를 감지하기 위해 바닥 레이어 설정
        layermask = 1 << LayerMask.NameToLayer("FLOOR");
    }

    private void Update()
    {
        if (target != null && isClick == false && Input.GetMouseButtonDown(0))
        {
            isClick = true;
            SpawnActivateObj();
            Destroy(target);
        }
    }

    public void SpawnTarget(SkillBase obj)
    {
        ActivateObject = obj;
        StartCoroutine(CospawnTarget());
    }

    public void SetAnim(SkillCoolDown skillCoolDown)
    {
        skillCool = skillCoolDown;
    }

    IEnumerator CospawnTarget()
    {
        isClick = false;
        // 카메라에서 마우스 위치로 레이를 쏩니다
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        // 지정된 레이어에 적중시
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
        {
            target = Instantiate(targetObject, hit.point, targetObject.transform.rotation);
            while (!isClick)
            {
                Ray rayTarget = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitTarget;
                if (Physics.Raycast(rayTarget, out hitTarget, Mathf.Infinity,layermask))
                {
                    // 마우스 위치로 계속 좌표 위치를 변환
                    target.transform.position = hitTarget.point;
                    yield return null;
                }
            }
        }
    }

    private void SpawnActivateObj()
    {
        StartCoroutine(coSpawnActivateObj());
        skillCool.UseSkill();
    }

    IEnumerator coSpawnActivateObj()
    {
        if (isClick)
        {
            SkillBase obj = Instantiate(ActivateObject, target.transform.position, ActivateObject.transform.rotation);
            obj.UseSkill();
        }
        yield return null;
    }


}
