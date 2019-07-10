using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
	public float BotSpeed = 5.0f;
	public bool vertical;
	public float changeTime = 2.0f;
    public bool broken = true;
    public ParticleSystem smokeEffect;

    AudioSource audioSource;
    public AudioClip fixClip;

	Rigidbody2D rigidbody2d;

	float timer;
	int direction = 1;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
       audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        if(!broken)
        {
            return;
        }

    	timer -= Time.deltaTime;

    	if (timer < 0)
    	{
    		direction = -direction;
    		timer = changeTime;
    	}

        Vector2 position = rigidbody2d.position;

        if (vertical)
        {
        	position.y = position.y + Time.deltaTime * BotSpeed * direction;
            
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else
        {
        	position.x = position.x + Time.deltaTime * BotSpeed * direction;
        
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }

        rigidbody2d.MovePosition(position);

    }

    void OnCollisionEnter2D(Collision2D other)
	{
   		RubyController player = other.gameObject.GetComponent<RubyController>();

   		if (player != null)
    	{
			player.ChangeHealth(-1);
		}
	}
    

    public void Fix()
    {
        animator.SetTrigger("Hit");
        broken = false;
        GetComponent<Rigidbody2D>().simulated = false;
        smokeEffect.Stop();
    }
}