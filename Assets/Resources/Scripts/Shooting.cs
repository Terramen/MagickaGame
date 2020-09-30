using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Shooting : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject skill;
   // [SerializeField] private UnityEngine.Object skill;
    public Transform firePoint;
    public Transform circle;
    //[SerializeField] private float speed = 5f;
    private Elements _elements;
    public GameObject[] prefabPool;

    void Awake()
    {
  //      player = GameObject.Find("/Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        PrefabCreation();
        // skill = Resources.Load("Assets/Magicka/Prefab/Boulder");
        _elements = FindObjectOfType<Elements>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (CrossPlatformInputManager.GetButtonDown("ButtonAttack"))
        {
            
            /*Debug.Log("Counter" + _elements.Counter + "Sprite" + _elements.SpriteRenderers[_elements.Counter - 1].sprite + "Sprite1" + _elements.SpriteArray[1]);
            Debug.Log("Check" + (_elements.SpriteRenderers[_elements.Counter - 1].sprite == _elements.SpriteArray[1]));*/
            if (_elements.SpriteRenderers[0].sprite == _elements.SpriteArray[1])
            {
                skill = prefabPool[1];
                Shoot();
            }
            if (_elements.SpriteRenderers[0].sprite == _elements.SpriteArray[2])
            {
                skill = prefabPool[0];
                Shoot();
            }
            foreach (var r in _elements.SpriteRenderers)
            {
                r.enabled = false;
                r.sprite = _elements.SpriteArray[3];
            }
            _elements.Counter = 0;
            // Debug.Log("Кнопка нажата");
        }
    }

    void Shoot()
    {
        //skill = Resources.Load<GameObject>("Assets/Magicka/Prefab/Boulder");
        Instantiate(skill, firePoint.position, firePoint.rotation);
    }

    void PrefabCreation()
    {
 
        prefabPool = Resources.LoadAll<GameObject>("Skills");
        Debug.Log(prefabPool.Length);

    }
}
