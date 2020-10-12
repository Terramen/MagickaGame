using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] private float hp;
    public Transform hpbar;

    public float Hp
    {
        get => hp;
        set => hp = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        hp = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (damage > hp)
        {
           
            hp = 0;
        }
        else
        {
            hp -= damage;
            Debug.Log(damage/100);
            hpbar.localScale = new Vector3(hpbar.localScale.x - damage/100, hpbar.localScale.y, hpbar.localScale.z);
          //  hpbar.localScale = Vector3.Lerp(hpbar.localScale, newScale, 1f);
            
        }
            

        if (hp == 0)
        {
            Destroy(gameObject);
        }
    }
}
