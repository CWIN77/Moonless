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
  private float moveSpeed = 2.5f;
  private sbyte direction = 1;
  // private float attackRange = 0.5f;

  private Animator anim;
  private Rigidbody2D rb;
  private BoxCollider2D coll;
  private MovementState state;

  // Start is called before the first frame update
  private void Start()
  {
    float dirX = Input.GetAxisRaw("Horizontal");

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
    bool isAttack1 = anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1");
    bool isAttack2 = anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2");

    if (!isAttack1 && !isAttack2)
    {
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
    }

    // Move Input
    if (!isSlide) { rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y); }

    // Jump Input
    if (Input.GetKeyDown("space") && IsGrounded() && !isSlide && !isAttack1 && !isAttack2)
    {
      rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    // Attack
    if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.4f && !isSlide)
    {
      if (attack_cnt > 1 || timeSinceAttack > 0.65f) { attack_cnt = 0; }
      anim.SetTrigger("Attack" + (attack_cnt + 1));
      attack_cnt++;
      timeSinceAttack = 0.0f;

      moveSpeed = 0;
    }
    else if (timeSinceAttack > 0.6f) { moveSpeed = 1.8f; }

    if (dirX != 0f && moveSpeed > 0 && IsGrounded()) { state = MovementState.run; } // Run
    else if (rb.velocity.y > .1f) { state = MovementState.jump; } // Jump
    else if (rb.velocity.y < -.1f) { state = MovementState.fall; } // Fall
    else { state = MovementState.idle; } // Idle

    if (Input.GetKeyDown(KeyCode.LeftShift) && IsGrounded()) // Slide
    {
      if (dirX == 0) { rb.velocity = new Vector2(direction * moveSpeed * 1.3f, rb.velocity.y); }
      else { rb.velocity = new Vector2(direction * moveSpeed * 1.75f, rb.velocity.y); }
      state = MovementState.slide;
    }

    anim.SetInteger("State", (int)state);
  }

  private bool IsGrounded()
  {
    return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
  }
}
