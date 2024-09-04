using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class OneWayPlatform : MonoBehaviour
{
    //ÇÑ¹æÇâ ÇÃ·§Æû

    private GameObject currentOneWayPlatform;
    [SerializeField] private CapsuleCollider2D playerColider;
    [SerializeField] private PlayerInputActions playerActions;
    private void Start()
    {
        playerActions =  GetComponent<PlayerMovement>()._playerActions;
        playerColider = GetComponent<CapsuleCollider2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (playerActions.Game.Move.phase.IsInProgress() && playerActions.Game.Jump.WasPerformedThisFrame())
        {
            if (playerActions.Game.Move.ReadValue<Vector2>().y < -0.8f)
            {
                if (currentOneWayPlatform != null)
                {
                    StartCoroutine(DisableCollision());
                }
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
            //if ((collision.transform.position - transform.position).y < 0)
            //{
            //    TilemapCollider2D tc = currentOneWayPlatform.GetComponent<TilemapCollider2D>();
            //    Physics2D.IgnoreCollision(playerColider, tc);
            //}

        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }
    private IEnumerator DisableCollision()
    {
        TilemapCollider2D tc = currentOneWayPlatform.GetComponent<TilemapCollider2D>();
        Physics2D.IgnoreCollision(playerColider, tc);
        yield return new WaitForSeconds(0.25f);
        Debug.Log("Disabled");
        Physics2D.IgnoreCollision(playerColider, tc, false);

        //Physics2D.IgnoreCollision(playerColider, tc,false);
        yield return null;
    }
}
