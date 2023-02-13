using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    public Boat owner;
    public Image bar;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(owner == null)
		{
            Destroy(this.gameObject);
            return;
		}

        bar.fillAmount = owner.GetLifePercent();

        transform.position = owner.transform.position + offset;
    }
}
