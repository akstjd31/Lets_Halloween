using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBase : MonoBehaviour, ISkil
{
    [SerializeField] private TextMeshProUGUI timetext;

    public string _name { get; set; }              // 이름
    public float _cooldown { get; set; }           // 쿨타임
    public float _currentCooldown { get; set; }    // 현재 쿨타임
    public float _duration { get; set; }           // 지속시간
    public float _currentDuration { get; set; }    // 남은 지속시간
    public float range { get; set; }               // 범위
    public bool IsReady { get; set; }              // 준비상태

    protected bool endSkill = false;

    private SphereCollider collider;

    private float _cooldownTime = 0;
    private float _durationTime = 0;

    private float timer = 0;

    void Start()
    {

        collider = GetComponent<SphereCollider>();

        if (collider != null )
        {
            collider.radius = range;
        }
    }

    public void UpdateTime(float cooldownTime, float durationTime)
    {
        _currentCooldown = cooldownTime;
        _currentDuration = durationTime;
        
        if (_cooldown <= _currentCooldown)
        {
            IsReady = true;
            Debug.Log("스킬 준비됨");
        }

        if (_duration <= _currentDuration)
        {
            endSkill = true;
        }

        Debug.Log($"남음{_currentCooldown}");
        Debug.Log($"지속{_currentDuration}");
    }
    public void StartTime()
    {
        Debug.Log("쿨타임 작동중");
        _cooldownTime += Time.deltaTime;
        if (!endSkill)
        {
            _durationTime += Time.deltaTime;
        }
        
        UpdateTime(_cooldownTime, _durationTime);

        if (IsReady)
        {
            _cooldownTime = 0;
            endSkill = false;
        }

        if (endSkill)
        {
            Debug.Log("종료되었음");
            _durationTime = 0;
            gameObject.SetActive(false);
        }
    }
    public void UseSkill()
    {
        _currentCooldown = 0;
        _currentDuration = 0;
        IsReady = false;
        Debug.Log("스킬 사용함");
        
        StartCoroutine(Starte());
    }

    IEnumerator Starte()
    {
        while (true)
        {
            StartTime();

            if (IsReady == true)
            {
                Destroy(gameObject);
                yield break;
            }

            yield return null;
        }
    }

    virtual public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (!endSkill)
            {
                // 자식에서 오버라이드
            }
            Debug.Log("범위내에 적 들어옴");
        }
    }
}
