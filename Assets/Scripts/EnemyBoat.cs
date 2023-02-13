using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoat : Boat
{
    public Boat target;
    [SerializeField] float range;
    [SerializeField] float shotCooldown = 1;
    float cooldown;




    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (target == null) return;

        transform.up = Vector3.Lerp(transform.up, target.transform.position - transform.position, rotationSpeed * Time.deltaTime);


        if(Vector2.Distance(transform.position,target.transform.position) > range)
		{
            rb.AddForce(transform.up * speed * Time.deltaTime, ForceMode2D.Impulse);
        }
		else
		{
            if (frontalWeapon != null)
            {
                cooldown += Time.deltaTime;
                if (cooldown >= shotCooldown)
                {
                    ShotFrontalWeapon();
                    cooldown = 0;
                }
            }
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if(range == 0)
		{
            if (collision.otherCollider.TryGetComponent(out Boat boat))
            {
                if (boat)
                {
                    Vector3 dir = (collision.transform.position - transform.position).normalized;

                    boat.GetDamage(1, dir, 200);
                    this.GetDamage(100, Vector3.zero);
                }

            }
        }


    }

}
