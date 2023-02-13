using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoat : Boat
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

		if (Input.GetKeyDown(KeyCode.Space)) { ShotFrontalWeapon(); }
		if (Input.GetKeyDown(KeyCode.Q)) { ShotLeftWeapons(); }
		if (Input.GetKeyDown(KeyCode.E)) { ShotRightWeapons(); }

        rb.AddForce(transform.up * speed * vertical * Time.deltaTime, ForceMode2D.Impulse);
        transform.Rotate(0, 0, -horizontal * rotationSpeed * Time.deltaTime);
    }
}
