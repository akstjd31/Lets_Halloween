using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IEntity
{
    public string Name { get; set; }
    public int ID { get; set; }
    public Transform Target { get; set; }

    public virtual void Initialize(string name, int id, Transform target)
    {
        Name = name;
        ID = id;
        Target = target;
    }
}
