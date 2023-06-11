using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed = 5f;

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;

        if (rb != null)
        {
            // Push rigidbodies
            rb.velocity = transform.forward * speed;
        }
        else if (other.CompareTag("Player"))
        {
            // Push character controller
            CharacterController controller = other.GetComponent<CharacterController>();

            if (controller != null)
            {
                Vector3 moveDirection = transform.forward * speed;
                controller.Move(moveDirection * Time.deltaTime);
            }
        }
    }
}
