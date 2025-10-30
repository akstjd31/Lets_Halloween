using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBase : MonoBehaviour, ISkil
{
    [SerializeField] public string skillName;
    [SerializeField] public float skillCooldown;
    [SerializeField] private float skillDuration;
    [SerializeField] private float skillRange;

    public string _name { get; set; }              // 이름
    public float _cooldown { get; set; }           // 쿨타임
    public float _currentCooldown { get; set; }    // 현재 쿨타임
    public float _duration { get; set; }           // 지속시간
    public float _currentDuration { get; set; }    // 남은 지속시간
    public float _range { get; set; }              // 범위
    public bool IsReady { get; set; }              // 준비상태

    protected bool endSkill = false;

    private new SphereCollider collider;

    private float _durationTime = 0;

    void Start()
    {
        IsReady = true;

        collider = GetComponent<SphereCollider>();

        if (collider != null )
        {
            collider.radius = _range;
        }
    }

    protected virtual void OnEnable()
    {
        _name = skillName;
        _cooldown = skillCooldown;
        _duration = skillDuration;
        _range = skillRange;
    }


    public void UpdateTime(float durationTime)
    {
        _currentDuration = durationTime;
        

        if (_duration <= _currentDuration)
        {
            endSkill = true;
        }
        Debug.Log($"지속시간{_duration}");
        Debug.Log($"지속{_currentDuration}");
    }
    public void StartTime()
    {
        Debug.Log("쿨타임 작동중");
        _durationTime += Time.deltaTime;
        
        UpdateTime(_durationTime);

        if (endSkill)
        {
            Destroy(gameObject);
            _durationTime = 0;
            gameObject.SetActive(false);
        }
    }
    public void UseSkill()
    {
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
