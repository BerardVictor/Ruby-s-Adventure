using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
	public GameObject healDrop;

	Transform lootpos;

	//Transform droppos;
    // Start is called before the first frame update
    void Start()
    {
    	lootpos = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Loot()
    {
    	Destroy(gameObject);
    	Instantiate(healDrop, lootpos.position, Quaternion.identity);
    }
}
