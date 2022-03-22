using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArea : MonoBehaviour
{
    public GameObject[] EnemyList;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var enemy in EnemyList)
        {
            enemy.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            foreach(var enemy in EnemyList)
            {
                enemy.SetActive(true);
            }
            var collider = GetComponent<Collider>();
            collider.enabled = false;
        }   
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
