using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class playercontroller : MonoBehaviour,IDamageable
{
    private PlayerInputAction _playerInputAction;
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider2D;
    [SerializeField] private LayerMask groundlayer,enemieslayer;
    [SerializeField]
    private float speed,jumpforce,extraHeightGround,attackrange;

    [SerializeField] private int Hp,damage;
    private float _timeBtwAttack;
    public float startTimeBtwAttack;

    public GameObject bloodvfx;
    public Transform attackPos;
    
    private bool ground,canAttack;
    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
         _playerInputAction = new PlayerInputAction();
        _playerInputAction.Player.Jump.performed += OnJump;
        _playerInputAction.Player.Fire.performed += OnFire;
        _playerInputAction.Player.Slash.performed += OnSlash;
        _playerInputAction.Player.Dash.performed += OnDash;
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
        _rb.AddForce(Vector2.left*jumpforce,ForceMode2D.Impulse);
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire");
    }
    public void OnSlash(InputAction.CallbackContext context)
    {
        if (!canAttack)return;
        //anim attack shakecam
        CamShake.Instance.shakeCam(5,.5f);
        Collider2D[] enemisToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackrange, enemieslayer);
        foreach (var item in enemisToDamage)
        {
            item.GetComponent<IDamageable>().TakeDamage(damage);
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
    void Update()
    {
        var value = _playerInputAction.Player.Move.ReadValue<Vector2>();
        
        _rb.velocity = new Vector2((value.x * speed),_rb.velocity.y);


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
      Instantiate(bloodvfx,transform.position,quaternion.identity);
      Hp -= Damage;
  }
}
