using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public  class Enemy : MonoBehaviour,IDamageable
{
    [SerializeField] private int health;
    [SerializeField] private float speed,startDazedTime;
        private float dazedTime;

    [SerializeField] private Animator anim;
    private bool invulable;
    public GameObject bloodvfx;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dazedTime <= 0)
        {
            speed = 5;
            invulable = false;
        }
        else
        {
            invulable = true;
            speed = 0;
            dazedTime -=Time.deltaTime;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        Attack();
    }

     void Attack()
     {
         
     }

     public void TakeDamage(int Damage)
    {
        if(invulable)return;
        
        Instantiate(bloodvfx, transform.position, quaternion.identity);
        dazedTime = startDazedTime;
        health -= Damage;

       // Debug.Log(Damage);
    }
}
