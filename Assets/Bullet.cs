using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(0,10)]
    public float DeadSecond = 3f;
    [Range(0,200)]
    public float Speed = 100f;

    public Vector3 StartPos { get; set;}
    public Vector3 TargetPos {get; set;}

    public void Init(Vector3 startPos,Vector3 targetPos) 
    {
        StartPos = startPos;
        TargetPos = targetPos;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        float time = 0f;
        transform.position = StartPos;
        var vec = (TargetPos - StartPos).normalized;
        var look = Quaternion.LookRotation(vec);
        this.transform.localRotation = look;
        while(time < DeadSecond)
        {
            time += Time.deltaTime;
            transform.position+= vec * Speed * Time.deltaTime;
            yield return null;
        }

        Object.Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
    
    }
}
