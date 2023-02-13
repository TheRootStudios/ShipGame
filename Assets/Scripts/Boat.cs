using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Boat : MonoBehaviour
{
    

    [Header("Components")]
    protected Rigidbody2D rb;
    protected Collider2D col;
    [SerializeField] SpriteRenderer[] rends;

    [Header("Sprites")]
    [SerializeField] Sprite[] hullSprites;
    [SerializeField] Sprite[] sailSprites;


    [Header("Parameters")]
    [SerializeField] protected float speed = 3f;
    [SerializeField] protected float rotationSpeed = 100f;
    [SerializeField] protected float maxLife = 10f;
    [SerializeField] protected float currentLife;

    [Header("Weapons")]
    [SerializeField] GameObject canonBallPrefab;
    [SerializeField] GameObject deathExplosion;
    [SerializeField] protected Transform frontalWeapon;
    [SerializeField] Transform[] rightWeapons;
    [SerializeField] Transform[] leftWeapons;




    public event Action<Boat> OnDeath;



    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rends = GetComponentsInChildren<SpriteRenderer>();
        currentLife = maxLife;
    }

    public virtual void Update()
    {
        if(currentLife <= 0) { return; }
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
	{
        ContactPoint2D contact = collision.GetContact(0);
		Vector3 pos = contact.point;
        Vector3 dir = (transform.position - pos).normalized;
        
        GetDamage(1,dir);
    }

    public void GetDamage(float damage, Vector3 direction, float collisionForce = 100)
	{
        currentLife -= damage;

        SetDestruction((int)(currentLife * hullSprites.Length / maxLife));

        if(currentLife > 0)
		{
            rb.AddForce(collisionForce * direction);

            DamageAnimation();
		} else
		{
            Death();
		}

    }


    void Death()
	{
        rb.velocity = Vector2.zero;

        DeathAnimation();

        OnDeath?.Invoke(this);

        Destroy(this.gameObject, 0.6f);
    }


    void DamageAnimation()
	{
        transform.localScale = new Vector3(1, 1, 1);
        transform.DOPunchScale(new Vector2(.2f, .2f), 0.1f);

        foreach (var rend in rends)
        {
            rend.material.DOColor(Color.white, 0);
        }

        foreach (var rend in rends)
		{
            rend.material.DOColor(Color.red, 0.1f).SetEase(Ease.InFlash, 4, 0);
		}

    }

    void DeathAnimation()
    {
        GameObject.Instantiate(deathExplosion, transform.position, Quaternion.identity);
        col.enabled = false;


        transform.DOLocalRotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear);
        foreach (var rend in rends)
        { 
            rend.material.DOColor(Color.clear, 0.5f);
        }
    }
    

    public void SetDestruction(int destructionIntensity)
	{
        if (destructionIntensity < 0) return;

        if(destructionIntensity == 0 && currentLife > 0)
		{
            destructionIntensity = 1;
		}

        if(destructionIntensity < hullSprites.Length)
		{
            rends[0].sprite = hullSprites[destructionIntensity];
		}

        if (destructionIntensity < sailSprites.Length)
        {
            rends[1].sprite = sailSprites[destructionIntensity];
        }
    }

    public void ShotFrontalWeapon()
	{
        GameObject ball = GameObject.Instantiate(canonBallPrefab, frontalWeapon.position + frontalWeapon.right * 0.3f, Quaternion.identity); ;
        ball.transform.right = frontalWeapon.right;
        ball.GetComponent<CannonBall>().Owner = this;
	}

    public void ShotRightWeapons()
    {
		foreach (var item in rightWeapons)
		{
            GameObject ball = GameObject.Instantiate(canonBallPrefab, item.position + item.right * 0.3f, Quaternion.identity); ;
            ball.transform.right = item.right;
            ball.GetComponent<CannonBall>().Owner = this;
        }
    }

    public void ShotLeftWeapons()
    {
        foreach (var item in leftWeapons)
        {
            GameObject ball = GameObject.Instantiate(canonBallPrefab, item.position + item.right * 0.3f, Quaternion.identity); ;
            ball.transform.right = item.right;
            ball.GetComponent<CannonBall>().Owner = this;
        }
    }

    public float GetLifePercent()
	{
        return currentLife / maxLife;
	}
}
