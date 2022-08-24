using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  private Animator anim;
  private GameObject player;

  private int HP = 100;

  private void Start()
  {
    anim = GetComponent<Animator>();
    player = GameObject.FindGameObjectWithTag("Player");
  }

  private void Update()
  {
    float playerDistance = gameObject.transform.position.x - player.transform.position.x;
    if (-1.3 < playerDistance || playerDistance > 1.3)
    {
      Attack();
    }
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
    bool animAttack1 = anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1");
    bool animTakeHit = anim.GetCurrentAnimatorStateInfo(0).IsName("TakeHit");

    if (HP > 0 && !animAttack1 && !animTakeHit)
    {
      anim.SetTrigger("Attack1");
    }
  }
}
