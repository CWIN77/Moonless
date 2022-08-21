using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
  // private void OnCollisionEnter2D(Collision2D collision)
  // {
  //   if (collision.gameObject.CompareTag("Monster"))
  //   {
  //     print("Attack");
  //   }
  // }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("Monster"))
    {
      print("Attack");
    }
  }
}
