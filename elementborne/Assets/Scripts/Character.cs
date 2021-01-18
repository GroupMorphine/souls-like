using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public new string name;
    public float maxHealth;
    [SerializeField]
    private float currentHealth;
    public Slider healthBar;

    [SerializeField]
    private AnimationClip replaceableAttackAnim;
    [SerializeField]
    private AnimationClip defaultAttackAnim;
    [SerializeField]
    private AnimationClip currentAttackAnim;
    [SerializeField]
    private GameObject[] magics;
    private int magicIndex;
    private int key;
    [SerializeField]
    private MagicAnimations[] magicAnimations;
    Dictionary<MagicController, AnimationClip> magicAnimationsDict;


    [SerializeField]
    protected LayerMask[] targetLayers;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float jumpForce;
    [SerializeField]
    protected float[] counters;
    [SerializeField]
    protected bool facingright;
    Animator anim;
    Rigidbody2D rb;
    public Transform groundCheck;
    [SerializeField]
    protected float groundCheckDistance;
    protected bool isGrounded;
    int airJump;
    protected AnimatorOverrideController overrideController;
    protected bool attacking;
    protected bool canMove;

    protected NeuralNetwork brain;

    void Start()
    {
        brain = new NeuralNetwork(4, 8, 8, 8, 3);

        canMove = true;
        airJump = 0;
        key = -1;
        counters = new float[4];
        currentHealth = maxHealth;
        healthBar.maxValue = currentHealth;
        healthBar.value = currentHealth;
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        overrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = overrideController;
        currentAttackAnim = defaultAttackAnim;

        magicAnimationsDict = new Dictionary<MagicController, AnimationClip>();
        foreach(MagicAnimations x in magicAnimations)
        {
            magicAnimationsDict.Add(x.magic, x.clip);
        }
    }

    void Update()
    {
        if (isGrounded)
        {
            airJump = 1;
        }
        CdReduce();
        Pick();
        GroundCheck();

        Transform nrObstacle = GetComponent<Nearest>().NearestObstacle();
        Transform nrEnemy = GetComponent<Nearest>().NearestEnemy();

        double[,] inputs = { { nrObstacle.position.x, nrObstacle.position.y, nrEnemy.position.x, transform.position.x + 35.5f } };

        Matrix values = brain.Predict(inputs);

        Attack(values[0, 0]);
        Jump(values[0, 1]);
        Move(values[0, 2]);
    }
   
    private void Pick()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D pickable = Physics2D.OverlapCircle(transform.position, 1.5f,targetLayers[1]);
            if (pickable != null && pickable.GetComponent<MagicController>().IsPickable)
            {
                if (magicIndex < magics.Length)
                {
                    magicIndex++;
                }
                else
                {
                    magicIndex = 1;
                }
                magics[magicIndex-1] = pickable.gameObject;
            }
        }
    }

    protected void Flip(GameObject a)
    {
        Vector3 scale = a.transform.localScale;
        scale.x *= -1;
        a.transform.localScale = scale;
        if (a.CompareTag("Player"))
        {
            facingright = !facingright;
        }
    }

    protected virtual void Attack(double value)
    {
        if (!attacking)
        {
            if (value >= 0.5 && magics[1] != null && counters[1] <= 0)
            {
                key = 1;
            }
            /*
            if (Input.GetMouseButtonDown(0) && magics[0] != null && counters[0] <= 0)
            {
                key = 0;
            }
            else if (Input.GetMouseButtonDown(1) && magics[1] != null && counters[1] <= 0)
            {
                key = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Q) && magics[2] != null && counters[2] <= 0)
            {
                key = 2;
                canMove = false;
            }
            else if (Input.GetKeyDown(KeyCode.E) && magics[3] != null && counters[3] <= 0)
            {
                key = 3;
                canMove = false;
            }*/
            if (key != -1)
            {
                if (magics[key].GetComponent<MagicController>().ActivateOnAir(isGrounded))
                {
                    attacking = true;
                    currentAttackAnim = magicAnimationsDict[magics[key].GetComponent<MagicController>()];
                    overrideController[replaceableAttackAnim.name] = currentAttackAnim;
                    anim.SetTrigger("attack");
                    counters[key] = magics[key].GetComponent<MagicController>().attackRate;
                }
                else
                {
                    attacking = false;
                    canMove = true;
                    key = -1;
                }
            }
        }
    }
    protected void CdReduce()
    {
        for (int i = 0; i < counters.Length; i++)
        {
            if(counters[i]>0)
                counters[i] -= Time.deltaTime;
        }
    }
    protected virtual void Magic()
    {
        GameObject a = Instantiate(magics[key], transform.GetChild(1).position, Quaternion.identity);
        a.GetComponent<MagicController>().IsPickable = false;
        if (facingright)
        {
            Flip(a);
        }
        Vector2 direction = (transform.GetChild(1).position - transform.position);
        direction.Normalize();
        direction.y = 0;
        a.GetComponent<Rigidbody2D>().AddForce(direction * a.GetComponent<MagicController>().speed, ForceMode2D.Impulse);
        ThreadPool.QueueUserWorkItem(delegate {
            if(attacking)
            {
                Thread.Sleep(500);
                attacking = false;
                canMove = true;
                key = -1;
            }
        });
    }

    protected virtual void Move(double value)
    {
        if(canMove)
        {

            float a = 0;
            if (value >= 0.5)
            {
                a = -1;
            }

            //float a = Input.GetAxis("Horizontal");
            if (facingright && a < 0)
            {
                Flip(gameObject);
            }
            else if (!facingright && a > 0)
            {
                Flip(gameObject);
            }
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
            anim.SetFloat("run", Mathf.Abs(a));
        }
    }

    protected virtual void Jump(double value)
    {
        if (value >= 0.5 && airJump > 0)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);
            airJump--;
        }

        /*
        if (Input.GetKeyDown(KeyCode.Space) && airJump > 0)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);
            airJump--;
        }*/
        else if (Input.GetKeyDown(KeyCode.Space) && airJump <= 0 && isGrounded)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);
        }
    }
    protected virtual void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance);
        
        if (hit.collider==null || !hit.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        else if (hit.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        anim.SetBool("isgrounded", isGrounded);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        if (currentHealth <= 0)
        {
            Die();
        }
        else if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    protected void Die()
    {
        Destroy(gameObject);
    }

    [System.Serializable]
    private struct MagicAnimations
    {
        public MagicController magic;
        public AnimationClip clip;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}
