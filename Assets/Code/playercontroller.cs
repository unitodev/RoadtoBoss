using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class playercontroller : MonoBehaviour,IDamageable
{
    private PlayerInputAction _playerInputAction;
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider2D;
    [SerializeField] private LayerMask groundlayer,enemieslayer;
    [SerializeField]
    private float speed,jumpforce,extraHeightGround,attackrange;

    [SerializeField] private int Hp,Maxmana,damage,projectilespeed;
    [SerializeField] private Image HPbar, ManaBar;
    private float _timeBtwAttack,mana;
    public float startTimeBtwAttack;

    public ParticleSystem bloodvfx,levelupvfx;
    public Transform attackPos;
    public GameObject bullet;
    private int directionface=1,Level=1,Exp,Maxexp=10;
    private bool ground,canAttack;

    private Animator _animator;
    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
         _playerInputAction = new PlayerInputAction();
        _playerInputAction.Player.Jump.performed += OnJump;
        _playerInputAction.Player.Fire.performed += OnFire;
        _playerInputAction.Player.Slash.performed += OnSlash;
       // _playerInputAction.Player.Dash.performed += OnDash;

        mana = Maxmana;
    }

    #region input

    private void OnEnable()
    {
        _playerInputAction.Player.Enable();
    }

    private void OnDisable()
    {
        _playerInputAction.Player.Disable();
    }

    // Update is called once per frame
    public void OnDash(InputAction.CallbackContext context)
    {
        // _rb.AddForce(Vector2.right*jumpforce*directionface,ForceMode2D.Impulse);
        _rb.velocity = Vector2.right * jumpforce * directionface;
        Debug.Log("Dash");
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if(Level<5)return;
        if(!IsManaEnough(20))return;
       GameObject go= Instantiate(bullet,transform.position+new Vector3(1*directionface,0),transform.rotation);
        go.GetComponent<Rigidbody2D>().AddForce(Vector2.right*projectilespeed*directionface,ForceMode2D.Impulse);
        go.GetComponent<bullet>().damage = (damage * 2) + (Level * 2);
        Debug.Log("Fire");
    }
    public void OnSlash(InputAction.CallbackContext context)
    {
        if (!canAttack)return;
        //anim attack shakecam
        _animator.SetTrigger("slash");
        CamShake.Instance.shakeCam(5,.5f);
        Collider2D[] enemisToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackrange, enemieslayer);
        foreach (var item in enemisToDamage)
        {
            item.GetComponent<IDamageable>().TakeDamage(damage+Level*2);
        }
       // Debug.Log("Slash");
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if(!IsGround())return;
        _rb.AddForce(Vector2.up*jumpforce,ForceMode2D.Impulse);
    }

    #endregion

    bool IsGround()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(_boxCollider2D.bounds.center, _boxCollider2D.bounds.size, 0f,
            Vector2.down, extraHeightGround, groundlayer);
        return raycastHit2D.collider != null;
    }

    bool IsManaEnough(int cost)
    {
        if (mana - cost > 0)
        {
            mana -= cost;
            return true;
        }
        return false;
    }

    void Move()
    {
        
        var value = _playerInputAction.Player.Move.ReadValue<Vector2>();
        _rb.velocity = new Vector2((value.x * speed),_rb.velocity.y);
        if (value.x < 0)
        {
            transform.rotation = Quaternion.Euler(0,180,0);
            directionface = -1;
        }
        else if(value.x>0)
        {
            directionface = 1;
            transform.rotation = Quaternion.Euler(0,0,0);
        }
    }

    void manaregain()
    {
        if (mana < Maxmana)
        {
            mana += Time.deltaTime * 5;
        }
        else
        {
            mana = Maxmana;
        }
    }

    void UIupdate()
    {
        HPbar.fillAmount = Hp / 100;
        ManaBar.fillAmount = mana / 100;
    }
    void Update()
    {
        Move();
        manaregain();
        UIupdate();
       
        Maxexp = Level * 10;
        if (Exp >= Maxexp)
        {

            Exp -= Maxexp;
            Level++;
            //vfx
            levelupvfx.Play();
            //Instantiate(levelupvfx, transform.position, quaternion.identity);
        }
        if (_timeBtwAttack <= 0)
        {
            canAttack = true;
            _timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            _timeBtwAttack -= Time.deltaTime;
        }

        if (Hp <= 0)
        {
            //die or respawn
        }
    }

  void  OnDrawGizmosSelected()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(attackPos.position,attackrange);
        
    }

  public void TakeDamage(int Damage)
  {
      //sound hit
      bloodvfx.Play();
      Hp -= Damage;
  }

 public void SetExp(int gain)
 {
     Exp += gain;
 }
  public int getLevel()
  {
      return Level;
  }
}
