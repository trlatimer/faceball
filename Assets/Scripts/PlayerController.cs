using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPun
{
    [Header("Info")]
    public int id;
    private int curAttackerId;

    [Header("Stats")]
    public float moveSpeed;
    public float jumpForce;
    public int curHP;
    public int maxHP;
    public int kills;
    public bool dead;

    [Header("Components")]
    public Rigidbody rig;
    public ScriptMachine movementScript;
    public Player photonPlayer;
    //public PlayerWeapon weapon;
    //public MeshRenderer mr;

    private Vector2 movementInput = Vector2.zero;
    private bool flashingDamage;

    [PunRPC]
    public void Initialize(Player player)
    {
        id = player.ActorNumber;
        photonPlayer = player;

        // Add to players list
        GameManager.instance.players[id - 1] = this;

        // not local player
        if (!photonView.IsMine)
        {
            GetComponentInChildren<Camera>().gameObject.SetActive(false);
            movementScript.enabled = false;
            rig.isKinematic = true;
        }
        else
        {
            //GameUI.instance.Initialize(this);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (photonView.IsMine)
        {
            movementInput = context.ReadValue<Vector2>();
        }
    }

    //private void Update()
    //{
    //    if (photonView.IsMine && !dead)
    //    {
    //        Move(movementInput);
    //    }
    //}

    private void Move(Vector2 input)
    {
        // Calculate direction relative to forward direction
        Vector3 dir = (transform.forward * input.y + transform.right * input.x) * moveSpeed;
        dir.y = rig.velocity.y;

        // Set as velocity
        rig.velocity = dir;
    }
}
