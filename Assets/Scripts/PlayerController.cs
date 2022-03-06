using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPun
{
    [Header("Player Info")]
    public int id;
    private int curAttackerId;

    [Header("Stats")]
    public float moveSpeed;
    public float jumpForce;
    public int curHP;
    public int maxHP;
    public int kills;
    public bool dead;

    [Header("Bullets")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnLoc;
    public int damage;
    public float shootRate;
    public float bulletSpeed;
    private float lastShootTime;

    [Header("Components")]
    public Rigidbody rig;
    public ScriptMachine movementScript;
    public Photon.Realtime.Player photonPlayer;
    //public PlayerWeapon weapon;
    //public MeshRenderer mr;

    private Vector2 movementInput = Vector2.zero;
    private bool flashingDamage;

    [PunRPC]
    public void Initialize(Player player)
    {
        id = player.ActorNumber;
        photonPlayer = player;

        // Add to player list
        GameManager.instance.players[id - 1] = this;

        // if not local player
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

    public void Shoot(Vector3 spawnPosition, Quaternion direction)
    {
        if (NetworkManager.instance.networkMode)
            photonView.RPC("SpawnBullet", RpcTarget.All, damage, spawnPosition, direction);
        else
            SpawnBullet(damage, spawnPosition, direction);
    }

    [PunRPC]
    public GameObject SpawnBullet(int damage, Vector3 spawnLoc, Quaternion dir)
    {
        GameObject bullet = Instantiate(bulletPrefab, spawnLoc, dir);
        bullet.GetComponent<Bullet>().Initialize(damage, id, photonView.IsMine);
        return bullet;
    }

    [PunRPC]
    public void TakeDamage(int attackerId, int damage)
    {
        Debug.Log($"Player {id} takes {damage} damage from player {attackerId}");
    }

    //private void Update()
    //{
    //    if (photonView.IsMine && !dead)
    //    {
    //        Move(movementInput);
    //    }
    //}

    //private void Move(Vector2 input)
    //{
    //    // Calculate direction relative to forward direction
    //    Vector3 dir = (transform.forward * input.y + transform.right * input.x) * moveSpeed;
    //    dir.y = rig.velocity.y;

    //    // Set as velocity
    //    rig.velocity = dir;
    //}
}
