using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlassOut : MonoBehaviour
{
    public GameObject glass_in;
    //public GameObject wheel;
    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            glass_in.SetActive(true);
            AudioController.Instance.PlaySFX("ball");
        }
        //StartCoroutine(WheelRotateAnimation());
    }

    //IEnumerator WheelRotateAnimation()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    LeanTween.rotateAround(wheel, Vector3.forward, -315f, 4f)
    //                .setEase(LeanTweenType.easeOutQuad).setLoopClamp() ;


       
    //    //yield return new WaitForSeconds(4f);
    //    //LeanTween.cancel(wheel);

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            glass_in.SetActive(true);
            AudioController.Instance.PlaySFX("ball");
        }

    }


}
