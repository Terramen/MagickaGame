using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ShootingOffline : MonoBehaviour
{
    public Rigidbody2D rb;

    [SerializeField] private GameObject skill;
    // [SerializeField] private UnityEngine.Object skill;
    public Transform firePoint;
    public Transform circle;
    //[SerializeField] private float speed = 5f;
    private Elements _elements;
    private GameObject[] _prefabPool;

    public GameObject Skill
    {
        get => skill;
        set => skill = value;
    }

    public GameObject[] PrefabPool => _prefabPool;
    
    
    public Transform endPoint;
    public LineRenderer lineRenderer;
    private GameObject waterflow;

    public LayerMask wallLayer;
    // private HpBar _hpBar;


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
         /*ShootWaterflow();*/
        if (CrossPlatformInputManager.GetButtonDown("ButtonAttack"))
        {
            
            
            /*Debug.Log("Counter" + _elements.Counter + "Sprite" + _elements.SpriteRenderers[_elements.Counter - 1].sprite + "Sprite1" + _elements.SpriteArray[1]);
            Debug.Log("Check" + (_elements.SpriteRenderers[_elements.Counter - 1].sprite == _elements.SpriteArray[1]));*/
            if (_elements.SpriteRenderers[0].sprite == _elements.SpriteArray[1])
            {
                skill = _prefabPool[1];
                Shoot();
            }
            if (_elements.SpriteRenderers[0].sprite == _elements.SpriteArray[2])
            {
                skill = _prefabPool[0];
                Shoot();
            }

            if (_elements.SpriteRenderers[0].sprite == _elements.SpriteArray[0])
            {
                skill = _prefabPool[3];
                if (waterflow == null)
                {
                    waterflow = Instantiate(skill, firePoint.position, firePoint.rotation);
                    lineRenderer = waterflow.GetComponent<LineRenderer>();
                    StartCoroutine(ShootLong(4f));
                }
                else
                {
                    waterflow.SetActive(true);
                    StartCoroutine(ShootLong(4f));
                }
                    
                  /*GameObject waterflow = Instantiate(skill, firePoint.position, firePoint.rotation);
                lineRenderer = waterflow.GetComponent<LineRenderer>();
                StartCoroutine(ShootLong(4f));
                Destroy(waterflow, 4f);*/

            }
            if (_elements.SpriteRenderers[0].sprite == _elements.SpriteArray[6])
            {
                skill = _prefabPool[2];
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
    
    // WATERFLOW
    


    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(firePoint.position, endPoint.position);
    }*/

    void Draw2DRay(Vector2 startPos,Vector2 endPos)
    {
        lineRenderer.SetPosition(0,startPos);
        lineRenderer.SetPosition(1,endPos);
    }

    private IEnumerator ShootLong(float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        waterflow.SetActive(false);
    }

    void PrefabCreation()
    {
        _prefabPool = Resources.LoadAll<GameObject>("Skills");
       // Debug.Log(_prefabPool.Length);
    }
}

