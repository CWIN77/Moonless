using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D coll)
  {
    if (coll.gameObject.CompareTag("Player"))
    {
      coll.gameObject.GetComponent<Player>().TakeDamage(5);
    }
  }
}