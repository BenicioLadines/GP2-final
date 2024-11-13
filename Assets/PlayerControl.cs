using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    PlayerState currentState;
    Rigidbody2D rb;
    Animator animator;

    [SerializeField] float groundAccel;
    [SerializeField] float groundSpeed;
    [SerializeField] float airAccel;
    [SerializeField] float airSpeed;
    [SerializeField] float jumpPower;
    Vector2 directionalInput;
    [SerializeField] Vector2 groundCheckOffset;
    [SerializeField] float groundCheckRadius;

    #region States

    [HideInInspector]public GroundState groundState = new GroundState();
    [HideInInspector]public AirState airState = new AirState();

    #endregion

    public void ChangeState(PlayerState state)
    {
        if(currentState != null)
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
        if (OnTheGround())
        {
            ChangeState(groundState);
        }
        else
        {
            ChangeState(airState);
        }
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
        if(directionalInput == null)
        {
            return false;
        }

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
        if(Mathf.Abs(rb.velocity.x) < groundSpeed)
        {
            rb.velocity += groundAccel * directionalInput * Time.fixedDeltaTime;
        }
    }

    public void AirDrift()
    {
        if (Mathf.Abs(rb.velocity.x) < airSpeed)
        {
            rb.velocity += airAccel * directionalInput * Time.fixedDeltaTime;
        }
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

}
