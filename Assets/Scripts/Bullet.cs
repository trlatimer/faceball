using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    private int attackerId;
    private bool isMine;

    public Rigidbody rig;

    public void Initialize(int damage, int attackerId, bool isMine)
    {
        this.damage = damage;
        this.attackerId = attackerId;
        this.isMine = isMine;

        Destroy(gameObject, 5.0f);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player") && isMine)
    //    {
    //        PlayerController player = GameManager.instance.GetPlayer(other.gameObject);

    //        if (player.id != attackerId)
    //            player.photonView.RPC("TakeDamage", player.photonPlayer, attackerId, damage);
    //    }

    //    Destroy(gameObject);
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isMine)
        {
            PlayerController player = GameManager.instance.GetPlayer(collision.gameObject);

            if (player.id != attackerId)
            {
                if (NetworkManager.instance.networkMode)
                    player.photonView.RPC("TakeDamage", player.photonPlayer, attackerId, damage);
                else
                    player.TakeDamage(attackerId, damage);
            }

        }

        Destroy(gameObject);
    }

}
