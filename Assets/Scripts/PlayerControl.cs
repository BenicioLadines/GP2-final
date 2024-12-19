using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public PlayerState currentState;
    Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    SpriteRenderer sr;
    [HideInInspector]public Color defaultColor;

    [SerializeField] string HorizontalInput;
    [SerializeField] string VerticalInput;
    public string attackButton;
    public string jumpButton;

    [SerializeField] float groundAccel;
    [SerializeField] float groundDecel;
    [SerializeField] float groundSpeed;
    [SerializeField] float airAccel;
    [SerializeField] float airDecel;
    [SerializeField] float airSpeed;
    [SerializeField] float jumpPower;
    [HideInInspector] public Vector2 directionalInput;
    [SerializeField] Vector2 groundCheckOffset;
    [SerializeField] float groundCheckRadius;
    [SerializeField] int maxHealthPoints;
    int currentHealthPoints;
    public float knockbackDecay;
    public float knockbackEndVelocity;
    public bool controlling;
    [SerializeField]List<AttackData> hitboxes;
    public bool facingRight;

    #region States

    public GroundState groundState = new GroundState();
    public AirState airState = new AirState();
    public KnockbackState knockbackState = new KnockbackState();
    public AttackingState attackingState = new AttackingState();
    public CrouchState crouchState = new CrouchState();
    public AirAttackState airAttackState = new AirAttackState();
    public LandingState landingState = new LandingState();

    #endregion

    public enum AttackType
    {
        neutralGround,
        forwardGround,
        downGround,
        upGround,
        neutralAir,
        upAir,
        downAir,
        backAir
    }


    public void ChangeState(PlayerState state)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = state;
        currentState.EnterState(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        defaultColor = sr.color;
        if (OnTheGround())
        {
            ChangeState(groundState);
        }
        else
        {
            ChangeState(airState);
        }

        currentHealthPoints = maxHealthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (controlling)
        {
            directionalInput = new Vector2(Input.GetAxisRaw(HorizontalInput), Input.GetAxisRaw(VerticalInput));
        }
        currentState.UpdateState();

        
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState();

        if(transform.position.y < -20)
        {
            Death();
        }
    }

    public void Jump()
    {
        rb.velocity = rb.velocity + (Vector2.up * jumpPower);
        ChangeState(airState);
    }

    public bool DirectionallyInputting()
    {
        if (!controlling) return false;

        if (directionalInput == null) return false;

        if(directionalInput.sqrMagnitude <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Walk()
    {
        animator.Play("run");
        float walkVelocity = Mathf.Lerp(rb.velocity.x, groundSpeed * directionalInput.x, groundAccel);

        rb.velocity = new Vector2(walkVelocity, rb.velocity.y);


       /* if(Mathf.Abs(rb.velocity.x) < groundSpeed)
        {
            rb.velocity += groundAccel * directionalInput * Time.fixedDeltaTime;
        }*/
    }

    public void GroundDecelerate()
    {
        float deceleration = Mathf.Lerp(rb.velocity.x, 0, groundDecel);
        rb.velocity = new Vector2(deceleration, rb.velocity.y);
    }

    public void AirDrift()
    {

        float driftVelocity = Mathf.Lerp(rb.velocity.x, airSpeed * directionalInput.x, airAccel);

        rb.velocity = new Vector2(driftVelocity, rb.velocity.y);

       /* if (Mathf.Abs(rb.velocity.x) < airSpeed)
        {
            rb.velocity += airAccel * directionalInput * Time.fixedDeltaTime;
        }*/
    }

    public void AirDecelerate()
    {
        float deceleration = Mathf.Lerp(rb.velocity.x, 0, airDecel);
        rb.velocity = new Vector2(deceleration, rb.velocity.y);
    }

    public bool OnTheGround()
    {
        if (Physics2D.OverlapCircle((Vector2)transform.position + groundCheckOffset, groundCheckRadius, LayerMask.GetMask("Default")))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + (Vector3) groundCheckOffset, groundCheckRadius);
    }

    public void TakeDamage(AttackData attack)
    {
        currentHealthPoints--;
        if(currentHealthPoints <= 0 ) 
        {
            Death();
        }

        knockbackState.knockbackForce = (Quaternion.Euler(0, 0, -attack.currentAngle) * Vector2.up) * attack.force;
        ChangeState(knockbackState);

    }

    public void AttackClash(AttackData attack)
    {
        knockbackState.knockbackForce = (Quaternion.Euler(0, 0, -attack.currentAngle) * Vector2.up) * attack.force;
        ChangeState(knockbackState);
    }

    public void Attack(AttackType attackType)
    {
        attackingState.attackType = attackType;
        ChangeState(attackingState);
    }

    public void AirAttack(AttackType attackType)
    {
        airAttackState.attackType = attackType;
        ChangeState(airAttackState);
    }

    public void TakeKnockback(Vector2 knockback)
    {
        rb.velocity = knockback;
    }

    public void TurnAround()
    {
        if (facingRight)
        {
            transform.localScale = Vector3.one;
            facingRight = false;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = true;
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }

    public Vector2 GetVelocity() => rb.velocity;
    public Vector2 SetVelocity(Vector2 velocity) => rb.velocity = velocity;

    public void ChangeColor(Color color)
    {
        sr.color = color;
    }

    public void HitboxEnable(int index)
    {
        hitboxes[index].gameObject.SetActive(true);
    }

    public void HitboxDisable(int index)
    {
        if (hitboxes[index].gameObject.activeSelf)
        {
            hitboxes[index].gameObject.SetActive(false);
        }
    }
    public int GetInputDirection()
    {
        int angle = (int)Vector2.SignedAngle(Vector2.up, directionalInput) / 90;
        if (facingRight)
        {
            angle *= -1;
        }
        return angle;
        
    }

    public bool IsAttacking()
    {
        return currentState == attackingState || currentState == airAttackState;
    }
}
