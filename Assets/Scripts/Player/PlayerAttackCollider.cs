using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
  [SerializeField] private GameObject prefab_obj;

  private void OnTriggerEnter2D(Collider2D coll)
  {
    if (coll.gameObject.CompareTag("Monster"))
    {
      coll.gameObject.GetComponent<Enemy>().TakeDamage(10);

      GameObject obj = MonoBehaviour.Instantiate(prefab_obj);
      obj.name = "SliceEffect";
      obj.transform.position = new Vector3(coll.gameObject.transform.localPosition.x + 0.06f, coll.gameObject.transform.localPosition.y, 0);
      obj.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
    }
  }
}
