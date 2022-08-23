using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCallAttack : MonoBehaviour
{
  public GameObject enemy;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("Player"))
    {
      enemy.GetComponent<Enemy>().Attack();
    }
  }
}
