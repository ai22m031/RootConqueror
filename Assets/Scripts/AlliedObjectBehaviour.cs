using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AlliedObjectBehaviour : MonoBehaviour
{
    private int _health;
    public int health { get; set; }

    public abstract void TakeDamage(int damage);
}
