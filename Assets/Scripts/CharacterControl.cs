using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CharacterControl : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;

    public Animator animator;
    public Rigidbody2D rb2D;

    public Transform groundCheckPosition;
    public float groundCheckRadius;
    public LayerMask groundCheckLayer;
    public bool grounded;

    public Image filler;
    public float maxCounter = .5f;

    private float _previousHealth;
    private float _counter = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        _previousHealth = GameManager.manager.health;

        Debug.Log("CharacterControl started");
    }

    // Update is called once per frame
    void Update()
    {

        grounded = Physics2D.OverlapCircle(groundCheckPosition.position, groundCheckRadius, groundCheckLayer);

        transform.Translate(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0, 0);

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            // This means you either have a or d pressed down
            transform.localScale = new Vector3(Input.GetAxisRaw("Horizontal"), 1, 1);
            animator.SetBool("Walk", true);
        }
        else
        {
            // Nothing is pressed
            animator.SetBool("Walk", false);
        }

        if (Input.GetButtonDown("Jump") && grounded)
        {

            rb2D.velocity = new Vector2(0, jumpForce);
            animator.SetTrigger("Jump");

        }

        _counter = (_counter + Time.deltaTime) % maxCounter;

        float t = _counter / maxCounter;
        filler.fillAmount = Mathf.SmoothStep(_previousHealth / GameManager.manager.maxHealth, GameManager.manager.health / GameManager.manager.maxHealth, t);

        if (_counter + Time.deltaTime > maxCounter)
        {
            _previousHealth = GameManager.manager.health;
        }
    }

    void TakeDamage(float damage)
    {
        _counter = 0;
        _previousHealth = filler.fillAmount * GameManager.manager.maxHealth;
        GameManager.manager.health -= damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            TakeDamage(15);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LevelEnd"))
        {
            SceneManager.LoadScene("IcyMap");
        }
        else if (collision.gameObject.CompareTag("Star"))
        {
            Destroy(collision.gameObject);
            GameManager.manager.currentStars++;
        }
    }
}
