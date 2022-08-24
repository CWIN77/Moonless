using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D coll)
  {
    if (coll.gameObject.CompareTag("Player"))
    {
      coll.gameObject.GetComponent<PlayerMovement>().TakeDamage(10);
    }
  }
}