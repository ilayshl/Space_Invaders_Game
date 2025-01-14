using System;
using UnityEngine;

/// <summary>
/// Responsible for the player's movement and shooting commands.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    private Shoot shoot;
    private float shootInterval = 0.7f;
    private float lastShotTime;
    int bulletType = 0;

    private void Awake()
    {
        shoot = GetComponent<Shoot>();
    }

    void Update()
    {
        MoveToMouse();
        CheckForFiring();
    }

    private void MoveToMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 currentPosition = transform.position;
        transform.position =
        Vector2.MoveTowards(currentPosition, mousePosition, moveSpeed * Time.deltaTime);
    }

    private void CheckForFiring()
    {
        if (Input.GetMouseButton(0))
        {
            if (Time.time > lastShotTime + shootInterval)
            {
                lastShotTime = Time.time;
                shoot.ShootBullet(bulletType, transform.position, transform.rotation);
            }
        }
    }

    public void IncreaseSpeed(float increase)
    {
        moveSpeed += increase;
    }

    //Percentage of how faster the player will shoot; lower shootInterval = faster fire rate.
    public void IncreaseFireSpeed(float increase)
    {
        shootInterval *= 1-(increase/100);
        if (shootInterval < 0.1) { shootInterval = 0.1f; }
    }

    public void IncreaseDamage(int increase) {
        bulletType+=increase;
        bulletType = Mathf.Min(bulletType, shoot.BulletTypes()-1);
    }
}
