using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [SerializeField] private LayerMask jumpableGround;

  private float jumpForce = 4f;
  private sbyte attack_cnt = 0;
  private float timeSinceAttack = 0.0f;
  private enum MovementState { idle, run, jump, fall, slide }
  private float moveSpeed = 1.9f;
  private sbyte direction = 1;
  private int HP = 100;
  private float animLength = 0;

  private Animator anim;
  private Rigidbody2D rb;
  private BoxCollider2D coll;
  private MovementState state;

  // Start is called before the first frame update
  private void Start()
  {
    anim = GetComponent<Animator>();
    rb = GetComponent<Rigidbody2D>();
    coll = GetComponent<BoxCollider2D>();
  }

  // Update is called once per frame
  private void Update()
  {
    float dirX = Input.GetAxisRaw("Horizontal");
    timeSinceAttack += Time.deltaTime;

    bool isSlide = anim.GetCurrentAnimatorStateInfo(0).IsName("Slide");
    bool animAttack1 = anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1");
    bool animAttack2 = anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2");
    bool animTakeHit = anim.GetCurrentAnimatorStateInfo(0).IsName("TakeHit");

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

    // Attack
    if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.4f && !isSlide)
    {
      if (attack_cnt > 1 || timeSinceAttack > 0.6f) { attack_cnt = 0; }
      anim.SetTrigger("Attack" + (attack_cnt + 1));
      attack_cnt++;
      timeSinceAttack = 0.0f;
      animLength = GetAnimLength("Attack" + (attack_cnt + 1));
    }

    if (!animAttack1 && !animAttack2 && !animTakeHit)
    {
      // if (Input.GetKeyDown("e"))
      // {
      //   TakeDamage(1);
      // }

      // -1 = Left, 1 = Right
      if (dirX < 0f && direction != -1)
      {
        transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y, 0);
        direction = -1;
      }
      else if (dirX > 0f && direction != 1)
      {
        transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y, 0);
        direction = 1;
      }

      transform.localScale = new Vector3(direction * 1.9F, 1.9F, 1);

      // Move Input
      if (!isSlide) { rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y); }

      // Jump Input
      if (Input.GetKeyDown("space") && IsGrounded() && !isSlide) { rb.velocity = new Vector2(rb.velocity.x, jumpForce); }

      if (dirX != 0f && IsGrounded()) { state = MovementState.run; } // Run
      else if (rb.velocity.y > .1f) { state = MovementState.jump; } // Jump   
      else if (rb.velocity.y < -.1f) { state = MovementState.fall; } // Fall
      else { state = MovementState.idle; } // Idle

      if (Input.GetKeyDown(KeyCode.LeftShift) && IsGrounded() && !animTakeHit) // Slide
      {
        if (dirX == 0) { rb.velocity = new Vector2(direction * moveSpeed * 1.3f, rb.velocity.y); }
        else { rb.velocity = new Vector2(direction * moveSpeed * 1.8f, rb.velocity.y); }
        state = MovementState.slide;
      }

      anim.SetInteger("State", (int)state);
    }
  }

  private bool IsGrounded()
  {
    return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
  }

  public void TakeDamage(int dmg)
  {
    HP -= dmg;
    anim.SetTrigger("TakeHit");
    animLength = GetAnimLength("TakeHit");
  }

  private float GetAnimLength(string animName)
  {
    float time = 0;
    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

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
