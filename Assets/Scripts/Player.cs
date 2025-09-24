using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.SearchService;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [Header("Player Movement Settings")]
    public float speed = 5f; // Velocidad del movimiento del jugador
    public float jumpForce = 4f; // Fuerza aplicada cuando el jugador salta
    private Rigidbody2D rb2D; // Referencia al componente Rigidbody2D
    private float move;
    
    [Header("Ground Check Settings")]
    private bool isGrounded; // Compruebe si el jugador está en el suelo
    public Transform groundCheck; // Transformación para verificar el suelo
    public float groundCheckRadius = 0.1f; // Radio para la verificación del suelo
    public LayerMask groundLayer; // Capa que representa el suelo 

    [Header("Coin Settings")]
    public TMP_Text coinText = null; // Referencia al componente TMP_Text para mostrar el contador de monedas
    private int coins = 0; // Contador de monedas recogidas
    
    private Animator animator; // Referencia al componente Animator para animaciones
    private bool isDead = false;

    // Start se llama una vez antes de la primera ejecución de Update después de que se crea MonoBehaviour
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>(); // Obtenga el componente Rigidbody2D adjunto a este GameObject
        animator = GetComponent<Animator>(); // Obtenga el componente Animator adjunto a este GameObject
    }

    // Update is called once per frame
    // La actualización se llama una vez por cuadro
    void Update()
    {
        if (isDead) return; 
        move = Input.GetAxis("Horizontal"); // Obtener entrada horizontal (teclas A/D o flechas izquierda/derecha)
        rb2D.linearVelocity = new Vector2(move * speed, rb2D.linearVelocity.y); // Establezca la velocidad horizontal mientras mantiene la velocidad vertical sin cambios

        if(move != 0)
        {
            // Gira el sprite del jugador según la dirección del movimiento.
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1);
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce); // Aplica una fuerza de salto al Rigidbody2D
        }

        animator.SetFloat("Speed", Mathf.Abs(move)); // Establece la velocidad en el parámetro del animador
        animator.SetFloat("VerticalVelocity", rb2D.linearVelocity.y); // Establece la velocidad vertical en el parámetro del animador
        animator.SetBool("IsGrounded", isGrounded); // Establece si el jugador está en el suelo en el parámetro del animador
    }

    private void FixedUpdate()
    {
        // Comprueba si el jugador está en el suelo usando un círculo de verificación
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            coins++; // Incrementa el contador de monedas
            if(coinText != null)
            {
                coinText.text = coins.ToString(); // Actualiza el texto en pantalla
            }
        }

        if (collision.transform.CompareTag("Spikes"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarga la escena actual
        }

        if(collision.transform.CompareTag("Barrel"))
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            rb2D.linearVelocity = Vector2.zero; // Reinicia la velocidad antes de aplicar el retroceso
            rb2D.AddForce(knockbackDirection * 5f, ForceMode2D.Impulse); // Aplica la fuerza de retroceso

            BoxCollider2D barrelCollider = collision.GetComponent<BoxCollider2D>();
            barrelCollider.enabled = false;

            collision.gameObject.GetComponent<Animator>().enabled = true; // Activa la animación de explosión del barril
            Destroy(collision.gameObject, 0.5f); // Destruye el barril después de 0.5 segundos
        }
    }

     public void PlayerDamaged()
    {
        if (isDead) return;
        isDead = true; // ✅ lo ponemos aquí antes de arrancar la corrutina
        rb2D.linearVelocity = Vector2.zero; // Detiene al jugador
        StartCoroutine(PlayDamageAnimation());
    }

    private IEnumerator PlayDamageAnimation()
    {
        animator.Play("Death");
        yield return new WaitForSeconds(2f); // espera animación   
        animator.Play("Idle"); // si quieres que reviva
        isDead = false; // ya puede moverse
    }
}
