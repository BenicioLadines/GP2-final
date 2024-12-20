using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackData : MonoBehaviour
{
    [SerializeField]float angle;
    float flippedAngle;
    public float currentAngle;
    public float force;
    [SerializeField]Collider2D attackCollider;
    public bool constant;
    public PlayerControl player;
    [SerializeField] ParticleSystem hitsparkGen;


    private void Update()
    {
        flippedAngle = -angle;

        if (player == null)
        {
            return;
        }
        if (player.facingRight)
        {
            currentAngle = flippedAngle;
        }
        else
        {
            currentAngle = angle;
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerControl>(out PlayerControl otherPlayer))
        {
            if (otherPlayer.IsAttacking())
            {
                otherPlayer.AttackClash(this);
                if (!constant) gameObject.SetActive(false);
                return;
            }
            hitsparkGen.transform.position = collision.ClosestPoint(collision.transform.position);
            hitsparkGen.Play();
            otherPlayer.TakeDamage(this);
            if(!constant) gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector2 origin = transform.position;
        if( attackCollider != null )
        {
            origin += attackCollider.offset;
        }

        
        Vector2 destination = origin + (Vector2)((Quaternion.Euler(0, 0, -currentAngle) * Vector2.up) * force);
        Gizmos.DrawLine(origin, destination);
    }
}
