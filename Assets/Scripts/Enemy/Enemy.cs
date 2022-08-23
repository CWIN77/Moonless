using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  private Animator anim;

  private int HP = 100;

  private void Start()
  {
    anim = GetComponent<Animator>();
  }

  public void TakeDamage(int dmg)
  {
    if (HP > 0)
    {
      anim.SetTrigger("TakeHit");
      HP -= dmg;

      if (HP <= 0)
      {
        anim.SetTrigger("Death");
      }
    }
  }

  public void Attack()
  {
    if (HP > 0)
    {
      anim.SetTrigger("Attack1");
    }

  }
}
