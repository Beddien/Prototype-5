using UnityEngine;
using UnityEngine.InputSystem;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private float minSpeed = 12;
    private float maxSpeed = 16;
    private float maxTorque = 10;
    private float xRange = 4;
    private float ySpawnPos = -6;
    private GameManager gameManager;
    public int pointValue;
    public ParticleSystem explosionParticle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        transform.position = RandomSpawnPos();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    Vector3 RandomForce() {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque() {
        return Random.Range(-maxTorque, maxTorque);
    }

    Vector3 RandomSpawnPos() {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive) {
            if (Mouse.current.leftButton.wasPressedThisFrame) {
                Debug.Log("Mouse was clicked");
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);
                if (Physics.Raycast(ray, out RaycastHit hit)) {
                    if (hit.transform == transform) {
                        Destroy(gameObject);
                        Instantiate(explosionParticle, transform.position, 
                        explosionParticle.transform.rotation);
                        gameManager.UpdateScore(pointValue);
                    }
                }
            }
        }
    }

    void OnTriggerEnter() {
        Destroy(gameObject);
        if (!gameObject.CompareTag("Bad")) {
            gameManager.GameOver();
        }
    }
}
