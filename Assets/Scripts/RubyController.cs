using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Runtime.CompilerServices;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;

    public float defaultSpeed = 3.0f;

    public int maxHealth = 5;

    public GameObject projectilePrefab;

    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip npcTalk;
    public AudioClip winSound;

    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    AudioSource audioSource;

    public ParticleSystem hitEffect;
    public ParticleSystem healEffect;

    public UIFixedRobotCount uIFixedRobotCount;

    public GameObject winUI;
    public GameObject loseUI;
    public GameObject notUI;

    public float bootsTimer;
    public bool fastBoots;

    public bool collectedBlueStrawb;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

        winUI.SetActive(false);
        loseUI.SetActive(false);
        notUI.SetActive(true);

        fastBoots = false; 
        collectedBlueStrawb = false;

    }

    // Update is called once per frame
    void Update()
    {
        //print(currentHealth);

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    PlaySound(npcTalk);
                    character.DisplayDialog();
                }
            }
        }
        //Activates win function
        if ( uIFixedRobotCount.fixedRobotsCount == 4 && collectedBlueStrawb == true)
        {
            //PlaySound(winSound);
            audioSource.PlayOneShot(winSound);
            Win();
        }

        //Activates lose function
        if (currentHealth <= 0)
        {
            Lose();
        }

        if (fastBoots == true)
        {
            bootsTimer -= Time.deltaTime;
        }

        if ( bootsTimer <= 0.1f)
        {
            speed = defaultSpeed;
            fastBoots = false;
        }

        if(collectedBlueStrawb == true)
        {
            notUI.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);

 
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            
            hitEffect.Play();
            
            PlaySound(hitSound);
            
        }
        if (amount > 0)
        {
            healEffect.Play();
        }


        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

       // Debug.Log("before calling setvalue");
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
       // Debug.Log("after calling setvalue");
    }

    void Launch()
    {

            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);
        if( fastBoots == true)
        {
            projectile.Launch(lookDirection, 300);
        }

            animator.SetTrigger("Launch");

            PlaySound(throwSound);
   

    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void Lose()
    {
        loseUI.SetActive(true);
        speed = 0.0f;

        

        if(Input.GetKeyDown(KeyCode.R))
        {
            resetGame();
        }
    }

    public void Win()
    {
        winUI.SetActive(true);
        

       // speed = 0.0f;
    }

    public void resetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void speedBoots()
    {
        fastBoots = true;
        bootsTimer = 15.0f;

        speed = 6.0f;
    }

}

