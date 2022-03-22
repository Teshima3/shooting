using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform[] RoutePoints;

    [Range(0,200)]
    public float Speed = 10f;

    [Range(0,1000)]
    public float MoveSpeed = 10f;
    public float MoveRange = 40f;

    public float _initialLife = 100;
    public float Life = 100;
    public Image LifeGage;

    bool _isHitRoutePoint;

    IEnumerator Move()
    {
        var prevPointPos = transform.position;
        var basePosition = transform.position;
        var movedPos = Vector2.zero;

        foreach(var nextPoint in RoutePoints)
        {
            _isHitRoutePoint = false;

            while (!_isHitRoutePoint)
            {

            
                var vec = nextPoint.position - prevPointPos;
                vec.Normalize();

            
                basePosition += vec * Speed * Time.deltaTime;

             
                movedPos.x += Input.GetAxis("Horizontal")*MoveSpeed*Time.deltaTime;
                movedPos.y += Input.GetAxis("Vertical")*MoveSpeed*Time.deltaTime;
                movedPos = Vector2.ClampMagnitude(movedPos,MoveRange);
                var worldMovedPos = Matrix4x4.Rotate(transform.rotation).MultiplyVector(movedPos);
                
             
                transform.position = basePosition + worldMovedPos;

            
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vec,Vector3.up),0.5f);

                yield return null;
            }

            prevPointPos = nextPoint.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "RoutePoint")
        {
            other.gameObject.SetActive(false);
            _isHitRoutePoint = true;
        }
        else if (other.gameObject.tag == "Enemy")
        {
            Life -= 10f;
            LifeGage.fillAmount = Life / _initialLife;

            other.gameObject.SetActive(false);
            Object.Destroy(other.gameObject);

            if(Life <= 0)
            {
                Camera.main.transform.SetParent(null);
                gameObject.SetActive(false);
                var sceneManager = Object.FindObjectOfType<SceneManager>();
                sceneManager.ShowGameOver();
            }
        }
        else if(other.gameObject.tag == "ClearRoutePoint")
        {
            var sceneManager = Object.FindObjectOfType<SceneManager>();
            sceneManager.ShowClear();
            _isHitRoutePoint = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Background")
        {
            Life = 0;
            LifeGage.fillAmount = Life / _initialLife;

            Camera.main.transform.SetParent(null);
            gameObject.SetActive(false);
            var sceneManager = Object.FindObjectOfType<SceneManager>();
            sceneManager.ShowGameOver();
        }
    }

    public Bullet BulletPrefab;

    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move());
    }

    public void ShotBullet(Vector3 targetPos)
    {
        var bullet = Object.Instantiate(BulletPrefab,transform.position,Quaternion.identity);
        bullet.Init(transform.position,targetPos);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
