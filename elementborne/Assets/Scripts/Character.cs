using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Damageable
{
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
    protected AnimatorOverrideController overrideController;
    protected bool attacking;
    private SpriteRenderer playerSprite;

    void Start()
    {
        // { Fire, Water, Stone, Electric, Wind };
        playerSprite = GetComponent<SpriteRenderer>();
        key = -1;
        magics = new GameObject[4];
        counters = new float[4];
        currentHealth = maxHealth;
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
        CdReduce();
        Pick();
        GroundCheck();
        Attack();
        Jump();
        Move();
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
        if(a.CompareTag("Player"))
            facingright = !facingright;
    }

    protected virtual void Attack()
    {
        if (!attacking)
        {
            if (Input.GetMouseButtonDown(0) && magics[0] != null && counters[0] <= 0)
            {
                key = 0;
            }
            else if (Input.GetMouseButtonDown(1) && magics[1] != null && counters[1] <= 0)
            {
                key = 1;
            }
            if (key != -1)
            {
                if (magics[key].GetComponent<MagicController>().ActivateOnAir(isGrounded))
                {
                    attacking = true;
                    playerSprite.material.SetColor("Color_RR", magics[key].GetComponent<MagicController>().elementcolor);
                    currentAttackAnim = magicAnimationsDict[magics[key].GetComponent<MagicController>()];
                    overrideController[replaceableAttackAnim.name] = currentAttackAnim;
                    anim.SetTrigger("attack");
                    counters[key] = magics[key].GetComponent<MagicController>().attackRate;
                }
                else
                    key = -1;
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
        a.GetComponent<Rigidbody2D>().AddForce(direction * a.GetComponent<MagicController>().speed,ForceMode2D.Impulse);
        attacking = false;
        key = -1;
    }
    protected virtual void Move()
    {
        float a = Input.GetAxis("Horizontal");
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

    protected virtual void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&isGrounded)
        {
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

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        anim.SetTrigger("hurt");
    }

    protected override void Die()
    {
        Debug.Log("şinee");
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
