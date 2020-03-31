using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 90;
    public float force = 700f;
    public int damage = 100;
    public int health = 100;
    public bool FindPlayer;
    Rigidbody rb;
    Transform t;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            rb.velocity += this.transform.forward * speed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S))
            rb.velocity -= this.transform.forward * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
            t.rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
        else if (Input.GetKey(KeyCode.A))
            t.rotation *= Quaternion.Euler(0, -rotationSpeed * Time.deltaTime, 0);

        if (Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(t.up * force);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void GiveDamage()
    {
        if (FindPlayer)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                //GetComponent<HealthSystem>().Damaged();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindPlayer = true;
    }

}