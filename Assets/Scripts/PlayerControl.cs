using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    PlayerState currentState;
    Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    SpriteRenderer sr;
    [HideInInspector]public Color defaultColor;

    [SerializeField] float groundAccel;
    [SerializeField] float groundDecel;
    [SerializeField] float groundSpeed;
    [SerializeField] float airAccel;
    [SerializeField] float airDecel;
    [SerializeField] float airSpeed;
    [SerializeField] float jumpPower;
    Vector2 directionalInput;
    [SerializeField] Vector2 groundCheckOffset;
    [SerializeField] float groundCheckRadius;
    [SerializeField] int maxHealthPoints;
    int currentHealthPoints;
    public float knockbackDecay;
    public float knockbackEndVelocity;
    public bool controlling;
    [SerializeField]List<AttackData> hitboxes;

    #region States

    public GroundState groundState = new GroundState();
    public AirState airState = new AirState();
    public KnockbackState knockbackState = new KnockbackState();
    public AttackingState attackingState = new AttackingState();

    #endregion

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
        directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        currentState.UpdateState();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState();
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
        if(Physics2D.OverlapCircle((Vector2)transform.position + groundCheckOffset, groundCheckRadius))
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

        knockbackState.knockbackForce = (Quaternion.Euler(0, 0, -attack.angle) * Vector2.up) * attack.force;
        ChangeState(knockbackState);

    }

    public void TakeKnockback(Vector2 knockback)
    {
        rb.velocity = knockback;
    }

    void Death()
    {
        Debug.Log("you freakin died");
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
}
