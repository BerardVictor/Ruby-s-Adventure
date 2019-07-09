using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float RubySpeed = 8.0f;

    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    
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

        if(Input.GetKeyDown(KeyCode.C))
        {
           Launch();
        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvicible)
                return;

            isInvicible = true;
            invicibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        Debug.Log(currentHealth + "/" + maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }
}
