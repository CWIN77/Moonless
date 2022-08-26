using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D coll)
  {
    if (coll.gameObject.CompareTag("Monster"))
    {
      coll.gameObject.GetComponent<Enemy>().TakeDamage(20);
    }
  }
}
