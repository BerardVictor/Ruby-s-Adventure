using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float RubySpeed = 8.0f;

    public GameObject rubyChar;

    public int maxHealth = 5;
    public float timeInvincible = 2.0f;

    AudioSource audioSource;
    public AudioClip projectileClip;
    public AudioClip rubyhitClip;
    public AudioClip rubywalkClip;
    
    public int health { get { return currentHealth;}}
    int currentHealth;
    bool isInvicible;
    float invicibleTimer;
    
    public GameObject projectilePrefab;

    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
                
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
                
        Vector2 position = rigidbody2d.position;
                
        position = position + move * RubySpeed * Time.deltaTime;
  
        rigidbody2d.MovePosition(position);


        if (isInvicible)
        {
            invicibleTimer -= Time.deltaTime;
            if (invicibleTimer < 0)
                isInvicible = false;
        }

        if (currentHealth == 0)
        {
            currentHealth += 6;
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
           Launch();
           PlaySound(projectileClip);
        }


        if (Input.GetKeyDown(KeyCode.V))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPCs"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }


    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvicible)
            {
                return;
            }
            isInvicible = true;
            PlaySound(rubyhitClip);
            invicibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 500);

        animator.SetTrigger("Launch");
    }
}
