using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  private Animator anim;
  private Rigidbody2D rb;
  private GameObject player;

  private float attackRange = 1.1f;
  private float playerDistance;
  private float moveSpeed = 0.8f;
  private sbyte direction = 1;
  private enum MovementState { idle, walk, death }
  private MovementState state;
  private float animLength = 0;
  private float startPosX;
  private bool findPlayer = false;
  private bool waitAttack = false;
  private float waitTime = 0;

  private int HP = 100;

  private void Start()
  {
    anim = GetComponent<Animator>();
    rb = GetComponent<Rigidbody2D>();
    player = GameObject.FindGameObjectWithTag("Player");
    startPosX = transform.localPosition.x;
  }

  private void Update()
  {
    bool animAttack1 = anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1");
    bool animTakeHit = anim.GetCurrentAnimatorStateInfo(0).IsName("TakeHit");
    bool animWait = anim.GetCurrentAnimatorStateInfo(0).IsName("Wait");

    playerDistance = player.transform.position.x - gameObject.transform.position.x;

    if (playerDistance < 0) { direction = -1; } // Left
    else if (0 < playerDistance) { direction = 1; } // Right

    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < animLength && animLength > 0)
    {
      rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }
    else
    {
      rb.constraints &= ~RigidbodyConstraints2D.FreezeAll;
      rb.constraints = RigidbodyConstraints2D.FreezeRotation;

      animLength = 0;
    }

    if (anim.GetInteger("Wait") > 0 && waitTime > 0.2)
    {
      waitTime = 0;
      anim.SetInteger("Wait", anim.GetInteger("Wait") - 1);
    }
    else if (anim.GetInteger("Wait") > 0)
    {
      waitTime += Time.deltaTime;
    }

    if (Mathf.Abs(playerDistance) < attackRange)
    {
      Attack();
    }

    if (HP > 0)
    {
      if (!animAttack1 && !animTakeHit && !animWait && (2.6 > Mathf.Abs(playerDistance) || findPlayer))
      {
        findPlayer = true;
        transform.localScale = new Vector3(direction * transform.localScale.y, transform.localScale.y, transform.localScale.z);
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
        state = MovementState.walk;
      }
      else
      {
        state = MovementState.idle;
      }

      anim.SetInteger("State", (int)state);

    }
  }

  public void TakeDamage(int dmg)
  {
    bool animTakeHit = anim.GetCurrentAnimatorStateInfo(0).IsName("TakeHit");

    if (HP > 0 && !animTakeHit)
    {
      anim.SetTrigger("TakeHit");
      HP -= dmg;
      animLength = GetAnimLength("TakeHit");

      if (HP <= 0)
      {
        anim.SetInteger("State", 2);
      }
    }
  }

  public void Attack()
  {
    bool animAttack1 = anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1");
    bool animTakeHit = anim.GetCurrentAnimatorStateInfo(0).IsName("TakeHit");
    bool animWait = anim.GetCurrentAnimatorStateInfo(0).IsName("Wait");

    if (HP > 0 && !animAttack1 && !animTakeHit && !animWait)
    {
      transform.localScale = new Vector3(direction * transform.localScale.y, transform.localScale.y, transform.localScale.z);
      anim.SetTrigger("Attack1");

      anim.SetInteger("Wait", Random.Range(1, 11));  // 1 ~ 10
      animLength = GetAnimLength("Attack1");
    }
  }

  private float GetAnimLength(string animName)
  {
    float time = 0;
    // rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

    RuntimeAnimatorController ac = anim.runtimeAnimatorController;
    for (int i = 0; i < ac.animationClips.Length; i++)
    {
      if (ac.animationClips[i].name == animName)
      {
        time = ac.animationClips[i].length;
      }
    }
    return time;
  }
}
