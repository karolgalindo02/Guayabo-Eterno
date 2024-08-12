using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public float speed;
    public float groundDist;
    public LayerMask groundLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>(); // Obtener el Animator del hijo
    }

    void Update()
{
    RaycastHit hit;
    Vector3 castPos = transform.position;
    castPos.y -= 1f;

    Debug.DrawRay(castPos, -transform.up * Mathf.Infinity, Color.red); // Dibuja el Raycast en la escena

    if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, groundLayer))
    {
        if (hit.collider != null)
        {
            //Debug.Log("Ground detected at position: " + hit.point);

            Vector3 movePos = transform.position;
            movePos.y = hit.point.y + groundDist; // Cambié esto para ajustar correctamente la posición
            transform.position = movePos;
        }
    }

    float x = Input.GetAxis("Horizontal");
    float y = Input.GetAxis("Vertical");
    Vector3 moveDir = new Vector3(x, 0, y);
    rb.velocity = moveDir * speed;

    if (x != 0 && x < 0)
    {
        sr.flipX = true;
    }
    else if (x != 0 && x > 0)
    {
        sr.flipX = false;
    }

    // Establecer el parámetro "walking" del Animator
    animator.SetBool("walking", x != 0.0f || y != 0.0f);
}
}