using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData : MonoBehaviour
{
    public float angle;
    public float force;
    [SerializeField]Collider2D attackCollider;
    public bool constant;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerControl>(out PlayerControl player))
        {
            player.TakeDamage(this);
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

        
        Vector2 destination = origin + (Vector2)((Quaternion.Euler(0, 0, -angle) * Vector2.up) * force);
        Gizmos.DrawLine(origin, destination);
    }
}
