using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab; // Asignar el prefab del jugador en el inspector
    [SerializeField] private float followDistance = 2.0f; // Distancia mínima para seguir al jugador
    private Transform playerTransform;
    private bool shouldFollow = false;
    private Animator animator; // Referencia al componente Animator
    private SpriteRenderer sr; // Referencia al componente SpriteRenderer

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = playerPrefab.transform; // Asignar la referencia del prefab del jugador
        animator = GetComponentInChildren<Animator>(); // Obtener el Animator del propio objeto
        sr = GetComponentInChildren<SpriteRenderer>(); // Obtener el SpriteRenderer del propio objeto
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFollow)
        {
            Follow();
        }
    }

    public void StartFollowing()
    {
        shouldFollow = true;
    }

    private void Follow()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance > followDistance)
        {
            transform.position = Vector3.Lerp(transform.position, playerTransform.position, Time.deltaTime * 2); // Ajustar la velocidad según sea necesario
            animator.SetBool("walking", true); // Activar la animación de caminar
            animator.SetBool("idleside", false); // Desactivar la animación de idleside

            // Flip del sprite basado en la dirección
            if (transform.position.x < playerTransform.position.x)
            {
                sr.flipX = false;
            }
            else if (transform.position.x > playerTransform.position.x)
            {
                sr.flipX = true;
            }
        }
        else
        {
            animator.SetBool("idleside", true); // Activar la animación de idleside
        }
    }
}