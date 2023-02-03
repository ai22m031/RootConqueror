using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public const float speed = 10f;
    public Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, this.transform.position + (Vector3)direction, speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        if(collision.gameObject.tag == "Enemy") {
            Debug.Log("Destruction");
            collision.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(1);
            Destroy(this.gameObject);
        }
    }
}
