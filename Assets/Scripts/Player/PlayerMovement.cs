using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [SerializeField] private LayerMask jumpableGround;

  private float jumpForce = 4f;
  private int attack_cnt = 0;
  private float timeSinceAttack = 0.0f;
  private enum MovementState { idle, run, jump, fall, slide }
  private float moveSpeed = 2.5f;
  // private float attackRange = 0.5f;

  private Animator anim;
  private Rigidbody2D rb;
  private BoxCollider2D coll;
  private MovementState state;

  // Start is called before the first frame update
  void Start()
  {
    anim = GetComponent<Animator>();
    rb = GetComponent<Rigidbody2D>();
    coll = GetComponent<BoxCollider2D>();
  }

  // Update is called once per frame
  void Update()
  {
    float dirX = Input.GetAxisRaw("Horizontal");
    timeSinceAttack += Time.deltaTime;

    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
    {
      rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }

    // Jump Input
    if (Input.GetKeyDown("space") && IsGrounded() && !anim.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
    {
      rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    // Attack
    if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.3f)
    {
      if (attack_cnt > 1)
      {
        attack_cnt = 0;
      }
      if (timeSinceAttack > 0.5f)
      {
        attack_cnt = 0;
      }
      anim.SetTrigger("Attack" + (attack_cnt + 1));
      attack_cnt++;
      moveSpeed = 0;
      timeSinceAttack = 0.0f;
    }
    else if (timeSinceAttack > 0.6f)
    {
      moveSpeed = 1.8f;
    }

    if (dirX != 0f && moveSpeed > 0 && IsGrounded()) // Run
    {
      state = MovementState.run;
      if (dirX < 0f)
      {
        transform.localScale = new Vector3(-1.9F, 1.9F, 1);
      }
      else
      {
        transform.localScale = new Vector3(1.9F, 1.9F, 1);
      }
    }
    else if (rb.velocity.y > .1f) // Jump
    {
      state = MovementState.jump;
    }
    else if (rb.velocity.y < -.1f) // Fall
    {
      state = MovementState.fall;
    }
    else // Idle
    {
      state = MovementState.idle;
    }

    if (Input.GetKeyDown(KeyCode.LeftShift) && IsGrounded())
    {
      int isLeft = -1;
      if (transform.localScale.x > 0)
      {
        isLeft = 1;
      }
      if (dirX == 0)
      {
        rb.velocity = new Vector2(isLeft * moveSpeed * 1.3f, rb.velocity.y);
      }
      else
      {
        rb.velocity = new Vector2(isLeft * moveSpeed * 1.75f, rb.velocity.y);
      }
      state = MovementState.slide;
    }

    anim.SetInteger("State", (int)state);
  }

  private bool IsGrounded()
  {
    return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
  }
}
