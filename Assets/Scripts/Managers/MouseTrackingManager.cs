using Palmmedia.ReportGenerator.Core.Reporting.Builders.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MouseTrackingManager : Singleton<MouseTrackingManager>
{
    [SerializeField] public GameObject targetObject; // 설치될 위치를 보여줄 프리뷰 오브젝트
    GameObject target;
    Renderer targetColor;

    private PlayerUnit ActivateUnitObject;
    private SkillBase ActivateSkillObject;
    
    private SkillCoolDown skillCool;

    [SerializeField] LayerMask layermask;
    [SerializeField] LayerMask layermask2;
    [SerializeField] LayerMask layermask3;
    [SerializeField] LayerMask activateMask;
    private static bool isClick = false;

    private ParentButton activeButton;

    bool clickEnable;

    bool onBuild;
    bool onMouseMap;
    bool overActivate;

    private void Start()
    {
        // 레이어를 통해 레이를 감지하기 위해 바닥 레이어 설정
        layermask = 1 << LayerMask.NameToLayer("FLOOR");
        layermask2 = 1 << LayerMask.NameToLayer("PlayerUnit_Projectile");
        layermask3 = 1 << LayerMask.NameToLayer("EnableFLOOR");
        activateMask = 1 << LayerMask.NameToLayer("PlayerUnit");

    }

    private void Update()
    {
        if (target != null && isClick == false && Input.GetMouseButtonDown(0))
        {
            // UI 위하고 클릭이 활성화 되었을시
            if (clickEnable == true && !EventSystem.current.IsPointerOverGameObject()) 
            {
                isClick = true;

                if (ActivateSkillObject != null)
                {
                    SpawnActivateObj(ActivateSkillObject,null);
                }

                else if (ActivateUnitObject != null)
                {
                    SpawnActivateObj(null,ActivateUnitObject);
                }

                Destroy(target);
            }
        }
    }

    public void SpawnTargetSkill(SkillBase obj)
    {
        ActivateSkillObject = obj;
        StartCoroutine(CospawnTarget());
    }

    public void SpawnTargetUnit(PlayerUnit obj)
    {
        ActivateUnitObject = obj;
        StartCoroutine(CospawnTarget());
    }


    public void SetAnim(SkillCoolDown skillCoolDown)
    {
        skillCool = skillCoolDown;
    }

    IEnumerator CospawnTarget()
    {
        Destroy(target);

        isClick = false;
        // 카메라에서 마우스 위치로 레이를 쏩니다
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, layermask, QueryTriggerInteraction.Ignore))
        {
            yield break;
        }

        // 지정된 레이어에 적중시
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
        {
            target = Instantiate(targetObject, hit.point, targetObject.transform.rotation);

            var targetRange = target.transform.Find("Range").gameObject;

            Transform rangeScale = targetRange.GetComponent<Transform>();

            rangeScale.localScale = new Vector3(ActivateSkillObject.skillRange*2,1, ActivateSkillObject.skillRange*2);


            targetColor = target.GetComponent<Renderer>();

            while (!isClick)
            {
                Ray rayTarget = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitTarget;

                if (!Physics.Raycast(rayTarget, out hitTarget, Mathf.Infinity, layermask, QueryTriggerInteraction.Ignore))
                {
                    yield return null;
                    continue;
                }

                if (Physics.Raycast(rayTarget, out hitTarget, Mathf.Infinity, layermask))
                {
                    // 마우스 위치로 계속 좌표 위치를 변환
                    target.transform.position = hitTarget.point;

                    target.transform.position += Vector3.up;

                    // 빌드공간에 있을시 true
                    onBuild = Physics.Raycast(rayTarget, out hitTarget, Mathf.Infinity, layermask2);

                    onMouseMap = Physics.Raycast(rayTarget, out hitTarget, Mathf.Infinity, layermask3);

                    if (ActivateUnitObject != null || ActivateSkillObject != null)
                    {
                        Vector3 halfExtents = targetColor.bounds.extents;

                        overActivate = Physics.BoxCast(target.transform.position + Vector3.up * 5, halfExtents, Vector3.down, target.transform.rotation, 10f, activateMask, QueryTriggerInteraction.Ignore);
                        //overActivate = Physics.Raycast(target.transform.position + Vector3.up, Vector3.down, out _, 10f, activateMask);
                        if (overActivate)
                        {
                            SetColorRecursive(target ,new Color(1, 0, 0, 0.5f)); // 빨간색
                            Debug.Log("유닛 겹침");
                            clickEnable = false;
                            yield return null;
                            continue;
                        }
                    }

                    if (onMouseMap == true && onBuild == true)
                    {
                        if (target.tag == "PlayerUnit")
                        {
                            SetColorRecursive(target, new Color(0, 1, 0, 0.5f)); // 초록색
                            clickEnable = true;
                            Debug.Log("타워 설치 지역 현재 플레이어 초록색");

                        }

                        else if (target.tag == "Skill")
                        {
                            SetColorRecursive(target, new Color(1, 0, 0, 0.5f)); // 빨간색
                            Debug.Log("타워 설치 지역 현재 스킬 빨간색");
                            clickEnable = false;
                        }

                        yield return null;
                    }

                    else if (onMouseMap == true && onBuild == false)
                    {
                        if (target.tag == "PlayerUnit")
                        {
                            SetColorRecursive(target, new Color(1, 0, 0, 0.5f)); // 빨간색
                            Debug.Log("길 지역 현재 플레이어 빨간색");
                            clickEnable = false;
                        }

                        else if (target.tag == "Skill")
                        {
                            SetColorRecursive(target, new Color(0, 1, 0, 0.5f)); // 초록색
                            Debug.Log("길 지역 현재 스킬 초록색");
                            clickEnable = true;
                        }
                        yield return null;
                    }

                    else
                    {
                        SetColorRecursive(target, new Color(1, 0, 0, 0.5f)); // 빨간색
                        Debug.Log("그외 지역 빨간색");
                        clickEnable = false;
                    }

                    yield return null;
                }
            }
            
        }
    }

    void SetColorRecursive(GameObject obj, Color color)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderers)
        {
            Material[] materials = r.materials;

            foreach (Material m in materials)
            {
                m.SetColor("_Color", color);
            }

            //if (r != null && r.material != null)
            //{
            //    r.material.color = color;
            //}
        }
    }

    private void SpawnActivateObj(SkillBase skill, PlayerUnit unit)
    {
        StartCoroutine(coSpawnActivateObj(skill,unit));
    }

    IEnumerator coSpawnActivateObj(SkillBase skill, PlayerUnit unit)
    {


        if (isClick)
        {
            

            if (skill != null)
            {
                skillCool.UseSkill();
                var obj = Instantiate(skill, target.transform.position, skill.transform.rotation);
                SkillBase skillObj = obj.GetComponent<SkillBase>();
                skillObj.UseSkill();

                if (activeButton != null)
                {
                    SkillButton a = activeButton.GetComponent<SkillButton>();
                    a.RemoveCount();
                }
            }

            else if (unit != null)
            {
                var obj = Instantiate(unit, target.transform.position, unit.transform.rotation);
            }
        }
        yield return null;
    }

    public void SetActiveButton(ParentButton button)
    {
        activeButton = button;
    }
}
