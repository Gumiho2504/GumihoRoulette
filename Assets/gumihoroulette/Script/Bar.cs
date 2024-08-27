using UnityEngine;

public class Bar : MonoBehaviour
{
    //public float rotationSpeed = 50f; // Speed at which the bar rotates
    public float ballPushStrength = 1f; // Strength at which the balls are pushed or pulled

    void Update()
    {
        // Rotate the bar around the center point
       // transform.RotateAround(transform.parent.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            if (ballRigidbody != null)
            {
                
                // Calculate the direction from the bar to the ball
                /// Vector2 directionToBall = collision.transform.position - transform.position;

                // Apply a force to the ball to push or pull it
                ballRigidbody.AddForce(-ballRigidbody.transform.position* ballPushStrength);;
            }
        }
    }
}
