using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private float turnSpeed = 0.5f;

    private Transform player;
    private bool isChasing = false;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update() {
        if (isChasing) {
            FollowPlayer();
        }
    }

    private void FollowPlayer() {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null && player != null) {
            Vector2 blobPosition = new Vector2(transform.position.x, transform.position.z);
            Vector2 playerPosition = new Vector2(player.position.x, player.position.z);

            Vector2 direction = (playerPosition - blobPosition).normalized;

            rb.AddForce(new Vector3(direction.x, 0, direction.y) * moveSpeed);

            Vector3 targetDirection = new Vector3(direction.x, 0, direction.y);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            float boostVelocity = 0f;
            float terminalVelocity = 10f;
            rb.linearVelocity = new Vector3(
                Mathf.Clamp(rb.linearVelocity.x, -terminalVelocity - boostVelocity, terminalVelocity + boostVelocity),
                rb.linearVelocity.y,
                Mathf.Clamp(rb.linearVelocity.z, -terminalVelocity - boostVelocity, terminalVelocity + boostVelocity)
            );
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !isChasing) {
            isChasing = true;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Player")) {
            GameManager.Instance.ReducePlayerHealth();
        }
    }

    public virtual void Hit() {
        Destroy(gameObject);
    }
}
