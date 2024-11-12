using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    PlayerState currentState;
    Rigidbody2D rb;
    Animator animator;
    Vector2 directionInput;
    



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }
}
