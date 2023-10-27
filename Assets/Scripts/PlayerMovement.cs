using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D body;

    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;

    public float runSpeed = 20.0f;
    public float bulletSpeed = 10f;

    public GameObject bullet;
    public GameObject laser;
    public GameObject fire;

    public Slider healthSlider;
    public float health = 100f;
    public float healthRegenSpeed = 5f;

    public enum FireState { 
        None,
        Bullet,
        Laser,
        Fire,
    }

    FireState currentState = FireState.Bullet;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        healthSlider.minValue = 0f;
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    void Update()
    {
        // Gives a value between -1 and 1
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePosition - this.transform.position;

            GameObject liveBullet = null;

            if (currentState == FireState.Bullet) liveBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            else if (currentState == FireState.Laser) liveBullet = Instantiate(laser, transform.position, Quaternion.identity);
            if (currentState == FireState.Fire) liveBullet = Instantiate(fire, transform.position, Quaternion.identity);

            liveBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * bulletSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Damage(10f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) currentState = FireState.Bullet;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentState = FireState.Laser;
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentState = FireState.Fire;
    }

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);

        health += Time.deltaTime * healthRegenSpeed;
        if (health >= 100) health = 100f;
        healthSlider.value = health;
    }

    void Damage(float damage) {
        health -= damage;
        if (health <= 0) {
            Destroy(this.gameObject);
        }
    }
}
