using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public GameObject powerupIndicator;
    public float speed = 5f;
    public float powerupStrength = 15f;
    public bool hasPowerup;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();   
        focalPoint = GameObject.Find("Focal Point"); 
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);

        powerupIndicator.transform.position = transform.position + new Vector3(0,-0.5f,0);

        if(transform.position.y < -10){SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);}
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        powerupIndicator.SetActive(false);
        hasPowerup = false;
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRb = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayPlayer = (other.transform.position - transform.position);
            enemyRb.AddForce(awayPlayer * powerupStrength, ForceMode.Impulse);
        }    
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("PowerUp"))
        {
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }    
    }
}
