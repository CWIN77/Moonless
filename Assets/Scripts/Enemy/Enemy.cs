using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  [SerializeField] private GameObject sliceEffect;

  private Animator anim;
  private Rigidbody2D rb;
  private GameObject player;

  private sbyte direction = 1;
  private float stopLength = 0;
  private bool findPlayer = false;
  private float waitAttackTime = 0;
  private float walkAroundTime = 0;
  private bool animAttack1 = false;
  private bool animTakeHit = false;
  private bool animWait = false;
  private bool isAction = false;
  private sbyte randDirection = 0;

  private int HP = 100;

  private void Start()
  {
    anim = GetComponent<Animator>();
    rb = GetComponent<Rigidbody2D>();
    player = GameObject.FindGameObjectWithTag("Player");
  }

  private void Update()
  {
    // ActiveBehaviour = Attack1 | TakeHit | Wait
    StopActiveBehaviour();
    PlayBehaviour();
  }

  private void PlayBehaviour()
  {
    WaitAttack();

    if (HP > 0 && IsPlayActiveBehavior())
    {
      float playerDistance = player.transform.position.x - gameObject.transform.position.x;
      direction = (sbyte)((playerDistance < 0) ? -1 : 1); // -1 : Left // 1 : Right

      if (Mathf.Abs(playerDistance) < 1.12) { Attack(); } // 플레이어가 범위내 있을때 공격
      else if (2.5f > Mathf.Abs(playerDistance) || findPlayer) { GoToPlayer(); } // 플레이어가 범위내 있을때 걸어가기
      else { WalkAround(); } // Idle
    }
  }

  private void WalkAround()
  {
    if (walkAroundTime > 1.5)
    {
      walkAroundTime = 0;
      randDirection = (sbyte)Random.Range(-1, 4);
    }
    else if (randDirection == 1 || randDirection == -1)
    {
      transform.localScale = new Vector3(randDirection * transform.localScale.y, transform.localScale.y, transform.localScale.z);
      rb.velocity = new Vector2(randDirection * 0.4f, rb.velocity.y);
      anim.SetInteger("State", 1);
    }
    else
    {
      anim.SetInteger("State", 0);
      rb.velocity = new Vector2(0 * 0.4f, rb.velocity.y);
    }

    walkAroundTime += Time.deltaTime;
  }

  private void StopActiveBehaviour()
  {
    if (stopLength > 0 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < stopLength)
    {
      rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }
    else if (rb.constraints.ToString() == "FreezePositionX, FreezeRotation")
    {
      rb.constraints &= ~RigidbodyConstraints2D.FreezeAll;
      rb.constraints = RigidbodyConstraints2D.FreezeRotation;
      stopLength = 0;
    }
  }

  private void GoToPlayer()
  {
    findPlayer = true;
    transform.localScale = new Vector3(direction * transform.localScale.y, transform.localScale.y, transform.localScale.z);
    rb.velocity = new Vector2(direction * 0.8f, rb.velocity.y);
    anim.SetInteger("State", 1);
  }

  private bool IsPlayActiveBehavior()
  {
    animAttack1 = anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1");
    animTakeHit = anim.GetCurrentAnimatorStateInfo(0).IsName("TakeHit");
    animWait = anim.GetCurrentAnimatorStateInfo(0).IsName("Wait");
    return (!animTakeHit && !animAttack1 && !animWait);
  }

  public void TakeDamage(int dmg)
  {
    animTakeHit = anim.GetCurrentAnimatorStateInfo(0).IsName("TakeHit");
    if (HP > 0 && !animTakeHit)
    {
      isAction = animAttack1 || animWait;
      anim.SetTrigger("TakeHit");
      HP -= dmg;
      stopLength = GetAnimLength("TakeHit");
    }

    if (HP <= 0) // Death
    {
      anim.SetInteger("State", 2);
    }
  }

  private void Attack()
  {
    transform.localScale = new Vector3(direction * transform.localScale.y, transform.localScale.y, transform.localScale.z);
    anim.SetTrigger("Attack1");
    if (!isAction)
    {
      anim.SetInteger("Wait", Random.Range(1, 10));  // 1 ~ 9
    }
    else
    {
      isAction = false;
    }
    stopLength = GetAnimLength("Attack1");
  }

  private void WaitAttack()
  {
    float playerDistance = player.transform.position.x - gameObject.transform.position.x;
    if (Mathf.Abs(playerDistance) < 1.2) // 플레이어가 일정 범위를 넘어가면 멈춤
    {
      if (anim.GetInteger("Wait") > 0 && waitAttackTime > 0.2)
      {
        waitAttackTime = 0;
        anim.SetInteger("Wait", anim.GetInteger("Wait") - 1);
      }
      else if (anim.GetInteger("Wait") > 0)
      {
        waitAttackTime += Time.deltaTime;
      }
    }
    else if (animWait)
    {
      anim.SetTrigger("StopWait");
    }
  }

  private float GetAnimLength(string animName)
  {
    float time = 0;
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

  private void OnTriggerEnter2D(Collider2D coll)
  {
    if (coll.gameObject.CompareTag("PlayerAttackBox") && HP > 0)
    {
      TakeDamage(10);

      GameObject obj = MonoBehaviour.Instantiate(sliceEffect);
      obj.name = "SliceEffect";
      obj.transform.position = new Vector3(gameObject.transform.position.x + 0.06f, gameObject.transform.position.y, 0);
      obj.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
    }
  }
}
