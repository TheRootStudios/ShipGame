using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    Rigidbody2D rb;
    public Boat Owner;
    public GameObject explosion;
    [SerializeField] float velocity;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 10);
    }

	private void Update()
	{
        //rb.velocity = transform.right * velocity * Time.deltaTime;
        transform.position += transform.right * velocity * Time.deltaTime;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.TryGetComponent(out Boat boat))
        {
            if (boat != Owner)
            {
                Vector3 dir = (collision.transform.position - transform.position).normalized;

                boat.GetDamage(1, dir,0);
                Explode();
            }

        }
        else
        {
            Explode();
        }
    }

    void Explode()
	{
        GameObject.Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

}
