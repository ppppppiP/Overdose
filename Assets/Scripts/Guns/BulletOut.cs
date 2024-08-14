using UnityEngine;

public class BulletOut : MonoBehaviour
{
    Rigidbody Bullet;
    public float BulletSpeed;
    public float BulletRotate;
    public float Time;
    float randomSpeed;
    float randomRotate;

    public void Start()
    {
        Bullet = GetComponent<Rigidbody>();
        randomSpeed = Random.Range(0, 300);
        randomRotate = Random.Range(0, 10);
        BulletSpeed = BulletSpeed + randomSpeed;
        BulletRotate = BulletRotate + randomRotate;
    }
    public void FixedUpdate()
    {

        if (Time > 0)
        {
            Bullet.AddForce(transform.up * BulletSpeed);
            Bullet.AddForce(transform.right * BulletSpeed);
            Bullet.AddTorque(transform.up * BulletRotate);
            Time -= 1;
        }
        else
        {
            Bullet.AddForce(transform.up * 0);
            Bullet.AddTorque(transform.up * 0);
        }
        
    }
}
