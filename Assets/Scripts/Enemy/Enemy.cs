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
    float playerDistance = player.transform.position.x - gameObject.transform.position.x;
    print(playerDistance);
    if (-1.28 < playerDistance && playerDistance < 1.28)
    {
      Attack();

      if (playerDistance < 0) { transform.localScale = new Vector3(-1.5F, 1.5F, 1); }
      else if (0 < playerDistance) { transform.localScale = new Vector3(1.5F, 1.5F, 1); }

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
