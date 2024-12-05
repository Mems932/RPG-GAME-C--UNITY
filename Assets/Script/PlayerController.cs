using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    Rigidbody2D rb;
    Animator anim;

    [Header("Stat")]
    [SerializeField]
    float moveSpeed;
    public int currentHealth;
    public int maxHealth;

    [Header("Attack")]
    private float attackTime;
    [SerializeField] float timeBetweenAttack;
    public bool canMove = true, canAttack = true; // Propriétés utilisées dans d'autres scripts
    [SerializeField] Transform checkEnemy;
    public LayerMask whatIsEnemy;
    public bool canInteract = true; // Ajout d'un booléen pour gérer l'interaction
    public float range;

    public static PlayerController instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Supprime les doublons si une autre instance existe.
            return;
        }

        instance = this; // Assure que 'instance' pointe vers l'objet actuel.
    }

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (canAttack)
            Attack();
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time >= attackTime)
            {
                rb.velocity = Vector2.zero;
                anim.SetTrigger("attack");

                StartCoroutine(Delay());
                IEnumerator Delay()
                {
                    canMove = false; // Correction ici
                    yield return new WaitForSeconds(0.5f);
                    canMove = true; // Correction ici
                }

                attackTime = Time.time + timeBetweenAttack;
            }
        }
    }

    private void FixedUpdate()
    {
        if (canMove) // Correction ici
            Move();
    }

    void Move()
    {
        if (Input.GetAxis("Horizontal") > 0.1 || Input.GetAxis("Horizontal") < -0.1 || Input.GetAxis("Vertical") > 0.1 || Input.GetAxis("Vertical") < -0.1)
        {
            anim.SetFloat("lastInputX", Input.GetAxis("Horizontal"));
            anim.SetFloat("lastInputY", Input.GetAxis("Vertical"));
        }

        if (Input.GetAxis("Horizontal") > 0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x + range, transform.position.y, 0);
        }
        else if (Input.GetAxis("Horizontal") < -0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x - range, transform.position.y, 0);
        }

        if (Input.GetAxis("Vertical") > 0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x, transform.position.y + range, 0);
        }
        else if (Input.GetAxis("Vertical") < -0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x, transform.position.y - range, 0);
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(x, y) * moveSpeed * Time.fixedDeltaTime;

        rb.velocity.Normalize();

        // Si le joueur est en mouvement (x ou y non nul), on met à jour les paramètres d'animation
        if (x != 0 || y != 0)
        {
            anim.SetFloat("inputX", x);
            anim.SetFloat("inputY", y);
        }
    }

    public void OnAttack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(checkEnemy.position, 0.5f, whatIsEnemy);

        foreach (var enemy in enemies)
        {
            // Appliquez les effets de l'attaque ici
        }
    }
}
