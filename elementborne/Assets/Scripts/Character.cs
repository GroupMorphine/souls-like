using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    public NeuralNetwork brain;
    public float fitness = 0;
}

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

    [SerializeField]
    protected Text genText;

    protected int epoch = 0;
    protected int gen = 0;
    protected NeuralNetwork brain;
    protected Genetic neuroevolution;
    List<Player> players = new List<Player>();
    public GameObject[] dusmanlar;
    Vector3 playerTransform;
    public GameObject[] pos;

    void Awake()
    {
        playerTransform = gameObject.transform.localPosition;
        for (int i = 0; i < pos.Length; i++)
        {
            GameObject a = Instantiate(dusmanlar[Random.Range(0, dusmanlar.Length)], pos[i].transform.position, Quaternion.identity);
        }
    }

    void Start()
    {
        brain = new NeuralNetwork(3, 8, 8, 3);

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
    float timeLeft = 40f;
    void Update()
    {
        genText.text = $"Genome: {(epoch % 5)+1} Gen: {gen+1}";

        if (isGrounded)
        {
            airJump = 1;
        }
        CdReduce();
        Pick();
        GroundCheck();

        Transform nrObstacle = GetComponent<Nearest>().NearestObstacle();
        Transform nrEnemy = GetComponent<Nearest>().NearestEnemy();

        double[,] inputs = { { nrEnemy.localPosition.x - transform.position.x, nrObstacle.localPosition.x-transform.position.x, transform.localPosition.x + 35.5f } };
        //Debug.Log(nrEnemy.transform.localPosition.x);
        Matrix values = brain.Predict(inputs);

        Attack(values[0, 0]);
        Jump(values[0, 1]);
        Move(values[0, 2]);

        timeLeft -= Time.deltaTime;

        Debug.Log(values[0, 0]+" --- "+ values[0, 1]+" --- "+ values[0, 2]);

        if (timeLeft < 0)
        {
            Die();
        }
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
            if (value >= 0.25)
            {
                a = -1;
            }
            else
            {
                a = 0;
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
            rb.velocity = new Vector2(a * speed, rb.velocity.y);
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
        else if (value >= 0.5 && airJump <= 0 && isGrounded)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);
        }

        /*
        if (Input.GetKeyDown(KeyCode.Space) && airJump > 0)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);
            airJump--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && airJump <= 0 && isGrounded)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);
        }*/
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
        Player player = new Player();
        player.fitness = transform.localPosition.x;
        player.brain = this.brain.Copy();

        players.Add(player);

        timeLeft = 40f;
        epoch += 1;
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(item);
        }
        transform.localPosition = playerTransform;
        for (int i = 0; i < pos.Length; i++)
        {
            GameObject a = Instantiate(dusmanlar[Random.Range(0, dusmanlar.Length)],pos[i].transform.position ,Quaternion.identity);
        }

        
        this.brain = new NeuralNetwork(3, 8, 8, 3);
        if (epoch % 5 == 0)
        {
            players.Sort((x, y) => x.fitness.CompareTo(y.fitness));
            Player parent1 = players[0];
            Player parent2 = players[1];
            Debug.LogError(parent1.fitness + " --- " + parent2.fitness + " --- " + players[players.Count - 1].fitness);
            players.Clear();

            Genetic neuroevolution = new Genetic(parent1.brain.Copy(), parent2.brain.Copy());

            NeuralNetwork new_brain =  neuroevolution.CrossOver();
            new_brain = Genetic.Mutate(new_brain.Copy(), mutation_rate: 0.1f).Copy();
            brain = new_brain.Copy();
            gen+=1;
        }
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
