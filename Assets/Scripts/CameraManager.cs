using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
  private Vector3 Start_Pos;
  private void Start()
  {
    Start_Pos = transform.localPosition;
  }
  public IEnumerator Shake(float duration, float shakeAmount, float shakeSpeed)
  {
    Vector3 originPosition = gameObject.transform.localPosition;
    float elapsedTime = 0;
    while (elapsedTime < duration)
    {
      Vector3 randPoint = originPosition + Random.insideUnitSphere * shakeAmount;
      gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, randPoint, Time.deltaTime * shakeSpeed);
      yield return null;

      elapsedTime += Time.deltaTime;
    }
    gameObject.transform.localPosition = originPosition;
  }
}