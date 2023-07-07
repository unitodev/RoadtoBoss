using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeEnemy : Enemy
{
    [SerializeField]
    private Vector2 attackrange;
    [SerializeField] private float attackdamage,startTimeBtwAttack;
    private float _timeBtwAttack;
    [SerializeField] private Transform attackPos;
    [SerializeField] private LayerMask enemieslayer;
    [SerializeField]private GameObject bullet;

    
      void Attack()
    {
        if (_timeBtwAttack <= 0)
        { //set anim
            Collider2D[] enemisToDamage = Physics2D.OverlapBoxAll(attackPos.position, attackrange,90, enemieslayer);
            //projectile
            // Instantiate(bullet);
            Debug.Log("pew");
            _timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            _timeBtwAttack -= Time.deltaTime;
        }
    }

   void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.red;
       Gizmos.DrawWireCube(attackPos.position,attackrange); 
    }
}
