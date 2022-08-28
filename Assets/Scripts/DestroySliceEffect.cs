using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySliceEffect : MonoBehaviour
{
  // Start is called before the first frame update
  private void Start()
  {
    Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
  }
}
