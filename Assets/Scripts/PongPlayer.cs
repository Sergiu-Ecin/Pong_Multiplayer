using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PongPlayer : NetworkBehaviour
{
    public float speed = 30f;
    public Rigidbody2D rbPlayer;
    PongHUD pongHUD;

    private void Start()
    {
        pongHUD = FindObjectOfType<PongHUD>();
    }
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            rbPlayer.velocity = new Vector2(0, Input.GetAxisRaw("Vertical")) * speed;
        }
    }
}