using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Importar para reiniciar la escena
using UnityEngine.UI; // Importar para manejar UI

public class PlayerController2 : MonoBehaviour
{
    public float speed;
    public float jumpForce = 5f; // Fuerza del salto
    public float groundDist = 0.2f; // Distancia para detectar el suelo
    public LayerMask groundLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;
    private Animator animator;
    [SerializeField] private GameObject blinkPanel; // Panel para el efecto de blink
    [SerializeField] private GameObject mirrorPrefab; // Prefab del espejo
    public float customGravity = -20f; // Gravedad personalizada

    private CanvasGroup blinkCanvasGroup;
    private bool isGrounded; // Verificar si el jugador está en el suelo

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>(); // Obtener el Animator del hijo

        // Verificar si el CanvasGroup está adjunto, si no, agregarlo
        blinkCanvasGroup = blinkPanel.GetComponent<CanvasGroup>();
        if (blinkCanvasGroup == null)
        {
            blinkCanvasGroup = blinkPanel.AddComponent<CanvasGroup>();
        }
        blinkCanvasGroup.alpha = 0; // Asegurarse de que el alpha esté en 0 al inicio
    }

    void Update()
    {
        // Detectar si el jugador está en el suelo usando SphereCast
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

        // Establecer el parámetro "walking" del Animator
        animator.SetBool("walking", x != 0.0f || y != 0.0f);

        // Detectar entrada de salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Aplicar gravedad personalizada si no está en el suelo
        if (!isGrounded)
        {
            rb.AddForce(Vector3.up * customGravity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == mirrorPrefab) // Comparar con el prefab del espejo
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}