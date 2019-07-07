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
    
    



    Rigidbody2D rigidbody2d;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 position = rigidbody2d.position;
        position.x = position.x + RubySpeed * horizontal * Time.deltaTime;
        position.y = position.y + RubySpeed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        if (isInvicible)
        {
            invicibleTimer -= Time.deltaTime;
            if (invicibleTimer < 0)
                isInvicible = false;
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
}
