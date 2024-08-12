using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{

    [SerializeField] private GameObject dialogueMark;
    private bool isPlayerInRange;

    [SerializeField] private GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject == playerPrefab)
    //     {
    //         Debug.Log("hay colision");
    //         isPlayerInRange = true;
    //         dialogueMark.SetActive(true);
    //     }
    // }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == playerPrefab)
        {
            
            isPlayerInRange = true;
            dialogueMark.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject == playerPrefab)
        {
            isPlayerInRange = false;
            dialogueMark.SetActive(false);
        }
    }

    // private void OnCollisionExit(Collision collision)
    // {
    //     if (collision.gameObject == playerPrefab)
    //     {
    //         isPlayerInRange = false;
    //         dialogueMark.SetActive(false);
    //     }
    // }

}
