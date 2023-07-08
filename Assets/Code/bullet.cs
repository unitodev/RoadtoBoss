using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{ 
    public int damage;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<IDamageable>()!=null)
        {
            other.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }
}
