using UnityEngine;
using UnityEngine.UI;
public class ResultBar : MonoBehaviour
{
    public GameObject ballPre;
    public Transform spawnPoint;
    public Transform parentObject;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameController.isBallColiResultBar)
        {
            GameController.isBallColiResultBar = false;
            if (collision.gameObject.CompareTag("Ball"))
            {
                AudioController.Instance.PlaySFX("ball");
                string collidedObjectName = collision.gameObject.name;
                GameController.resultString = collision.gameObject.name;
                Debug.Log("Collided with: " + collidedObjectName);

                GameObject spawnedBall = Instantiate(ballPre, new Vector3(spawnPoint.transform.position.x + Random.RandomRange(-1, 1), spawnPoint.transform.position.y, spawnPoint.transform.position.z), Quaternion.identity);
                spawnedBall.gameObject.name = collidedObjectName;
                spawnedBall.GetComponent<SpriteRenderer>().color = collision.gameObject.GetComponent<SpriteRenderer>().color;
                spawnedBall.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = collision.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
                spawnedBall.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().color = collision.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().color;
                collision.gameObject.name = "remove";
               // LeanTween.scale(collision.gameObject, new Vector3(2,2,2), 0.5f).setEaseInOutElastic();
                //Destroy(collision.gameObject);
            }
            FindObjectOfType<GameController>().ResultShow();
            FindObjectOfType<GameController>().resultImage.color = collision.gameObject.GetComponent<SpriteRenderer>().color;
            FindObjectOfType<GameController>().resultImage.gameObject.transform.GetChild(0).GetComponent<Text>().text = collision.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
            //if (GameController.isRestartGame)
            //{
            //    GameController.isRestartGame = false;
            //    Destroy(collision.gameObject);
            //}
            //collision.gameObject.transform.SetParent(gameObject.transform);
        }

       
        //StartCoroutine(WheelRotateAnimation());
    }

    
}
