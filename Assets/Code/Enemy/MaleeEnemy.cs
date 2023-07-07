using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaleeEnemy : Enemy
{
    [SerializeField] private float attackrange,attackdamage,startTimeBtwAttack;
     private float _timeBtwAttack;
     [SerializeField] private Transform attackPos;
     [SerializeField] private LayerMask enemieslayer;

    
      void Attack()
     {
         if (_timeBtwAttack <= 0)
         { //set anim
             Collider2D[] enemisToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackrange, enemieslayer);
             foreach (var item in enemisToDamage)
             {
                 item.GetComponent<IDamageable>().TakeDamage((int)attackdamage);
             }
             _timeBtwAttack = startTimeBtwAttack;
         }
         else
         {
             _timeBtwAttack -= Time.deltaTime;
         }
     }
}
