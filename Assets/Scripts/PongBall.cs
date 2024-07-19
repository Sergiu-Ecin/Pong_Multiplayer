using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PongBall : NetworkBehaviour
{
    #region Variables
    //[SyncVar(hook = nameof(HandleBallSpeedUp))]
    public float ballSpeed;
    public Rigidbody2D rb;
    PongHUD pongHUD;
    #endregion

    #region Server
    public override void OnStartServer()
    {
        base.OnStartServer();

        rb.simulated = true;

        Vector2 direction = Random.insideUnitCircle.normalized;
        rb.velocity = direction * ballSpeed;
        pongHUD = FindObjectOfType<PongHUD>();
    }

    [ServerCallback]
    void OnCollisionEnter2D(Collision2D collision)
    {
        PongPlayer player = collision.transform.GetComponent<PongPlayer>();
        if (player != null)
        {
            float impactFactor = CalculateHit(transform.position,
                                              collision.transform.position,
                                              collision.collider.bounds.size.y);

            float horizontalDirection = collision.relativeVelocity.x > 0 ? 1 : -1;

            Vector2 direction = new Vector2(horizontalDirection, impactFactor).normalized;

            rb.velocity = direction * ballSpeed;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            ballSpeed++;
            Debug.Log("SpeedUp");
        }

        if (collision.gameObject.CompareTag("Wall Right"))
        {
            pongHUD.UpdateScore(1);
            RespawnBallAndPlayers();
        }

        if (collision.gameObject.CompareTag("Wall Left"))
        {
            pongHUD.UpdateScore(2);
            RespawnBallAndPlayers();
        }
    }

    void RespawnBallAndPlayers()
    {
        NetworkServer.Destroy(gameObject);

        PongMultiplayerManager.Instance.RespawnBallAndPlayers();
    }
    #endregion

    #region Client
    float CalculateHit(Vector2 ballPosition, Vector2 playerPosition, float playerHeight)
    {
        float positionDifference = ballPosition.y - playerPosition.y;
        float hitFactor = positionDifference / playerHeight;
        return hitFactor;
    }
    #endregion
}