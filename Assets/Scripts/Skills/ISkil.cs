using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkil
{
    string _name { get; set; }              // 이름
    float _cooldown { get; set; }           // 쿨타임
    float _currentCooldown { get; set; }    // 현재 쿨타임
    float _duration { get; set; }           // 지속시간
    float range { get; set; }               // 범위
    bool IsReady { get; set; }              // 준비상태

    void UpdateTime(float cooldowntime, float durationtime);

    void StartTime();
}
