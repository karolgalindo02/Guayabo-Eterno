using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController2 : MonoBehaviour
{
    public float speed;
    public float jumpForce = 5f;
    public float groundDist = 0.2f;
    public LayerMask groundLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;
    private Animator animator;
    [SerializeField] private GameObject blinkPanel;
    [SerializeField] private GameObject mirrorPrefab;
    public float customGravity = -20f;

    private CanvasGroup blinkCanvasGroup;
    private bool isGrounded;

    // Referencia al GameObject con el script ControllerParts
    [SerializeField] private GameObject controllerPartsObject;
    private ControllerParts controllerParts;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        blinkCanvasGroup = blinkPanel.GetComponent<CanvasGroup>();
        if (blinkCanvasGroup == null)
        {
            blinkCanvasGroup = blinkPanel.AddComponent<CanvasGroup>();
        }
        blinkCanvasGroup.alpha = 0;

        // Obtener la referencia al componente ControllerParts
        if (controllerPartsObject != null)
        {
            controllerParts = controllerPartsObject.GetComponent<ControllerParts>();
        }
    }

    void Update()
    {
        isGrounded = Physics.SphereCast(transform.position, 0.5f, Vector3.down, out RaycastHit hit, groundDist, groundLayer);

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(x, 0, y);
        rb.velocity = new Vector3(moveDir.x * speed, rb.velocity.y, moveDir.z * speed);

        if (x != 0 && x < 0)
        {
            sr.flipX = true;
        }
        else if (x != 0 && x > 0)
        {
            sr.flipX = false;
        }

        animator.SetBool("walking", x != 0.0f || y != 0.0f);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (!isGrounded)
        {
            rb.AddForce(Vector3.up * customGravity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == mirrorPrefab)
        {
            StartCoroutine(BlinkAndRestart());
        }
    }

    private IEnumerator BlinkAndRestart()
    {
        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            blinkCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }

        blinkCanvasGroup.alpha = 1;
        Vector3 newPlayerPosition = new Vector3(25.1000004f, 18.5f, -31.2999992f);
        transform.position = newPlayerPosition;


        // Activar objetos aleatorios desde el ControllerParts
        
        
        //controllerParts.ActivateRandomObjects();
        

        yield return new WaitForSeconds(1.2f); // el tiempo es para que el jugador2 tenga tiempo de ir a la pos del 1

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            blinkCanvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / duration));
            yield return null;
        }
        blinkCanvasGroup.alpha = 0;
    }
}
