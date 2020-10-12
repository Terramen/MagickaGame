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
    private GameObject[] _prefabPool;

    public GameObject[] PrefabPool => _prefabPool;
    
    
    public Transform endPoint;
    public LineRenderer lineRenderer;
    public GameObject waterflow;

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
                skill = _prefabPool[2];
                GameObject waterflow = Instantiate(skill, firePoint.position, firePoint.rotation);
                lineRenderer = waterflow.GetComponent<LineRenderer>();
                // waterflow.SetActive(true);
                StartCoroutine(ShootLong(4f));
                Destroy(waterflow, 4f);
                // skill = 
                 
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
    
    void ShootWaterflow()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, circle.transform.up, 2, wallLayer);
        //_hpBar = hit.collider.gameObject.GetComponent<HpBar>();

        if (hit)
        {
                HpBar hpBar = hit.collider.GetComponent<HpBar>();
                if (hpBar != null)
                {
                    hpBar.TakeDamage(0.05f);
                }
        }
        
        if (hit.collider != null)
        {
        //    _hpBar.TakeDamage(0.1f);
           // Debug.Log(hit.collider.name + " + " + hit.collider.gameObject);
            Draw2DRay(firePoint.position, hit.point);
        }
        else
        {
         //   _hpBar.TakeDamage(0.1f);
           // Debug.Log(hit.collider.name);
            Draw2DRay(firePoint.position, endPoint.position);
        }

            
    }

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
            //Debug.Log("Ку" + time);
            ShootWaterflow();
            time -= Time.deltaTime;
            yield return null;
        }
        // lineRenderer = skill.GetComponent<LineRenderer>();
        //waterflow.SetActive(true);
        // Instantiate(skill, firePoint.position, firePoint.rotation);
        // yield return new WaitForSeconds(time);
        // waterflow.SetActive(false);
    }

    void PrefabCreation()
    {
 
        _prefabPool = Resources.LoadAll<GameObject>("Skills");
        Debug.Log(_prefabPool.Length);

    }
}
