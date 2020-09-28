using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Elements : MonoBehaviour
{
    public GameObject elements;
    [SerializeField] private int _counter = 0;
    [SerializeField] private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();
    [SerializeField] private Sprite[] _spriteArray;

    public int Counter
    {
        get => _counter;
        set => _counter = value;
    }

    public Sprite[] SpriteArray
    {
        get => _spriteArray;
        set => _spriteArray = value;
    }

    public List<SpriteRenderer> SpriteRenderers
    {
        get => _spriteRenderers;
        set => _spriteRenderers = value;
    }

    void Awake()
    {
        _spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
        _spriteRenderers.Remove(GetComponent<SpriteRenderer>());
    }

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("ButtonFire"))
        {
        //    Debug.Log(_spriteRenderers[i].sprite == spriteArray[3]);
            
            if (_spriteRenderers[_counter].sprite == _spriteArray[3])
            {
                _spriteRenderers[_counter].sprite = _spriteArray[1];
            }

            _spriteRenderers[_counter].enabled = true;
            if (_counter < _spriteRenderers.Count - 1)
            {
                ++_counter;
            }
        };

        if (CrossPlatformInputManager.GetButtonDown("ButtonEarth"))
        {
            if (_spriteRenderers[_counter].sprite == _spriteArray[1])
            {
                _spriteRenderers[_counter].sprite = _spriteArray[2];
            }
   //         Debug.Log(_spriteRenderers[i].color == Color.white);

            if (_spriteRenderers[_counter].sprite == _spriteArray[3])
            {
                _spriteRenderers[_counter].sprite = _spriteArray[2];
            }

            _spriteRenderers[_counter].enabled = true;
            if (_counter < _spriteRenderers.Count - 1)
            {
                ++_counter;
            }
        };

        if (CrossPlatformInputManager.GetButtonDown("ButtonWater"))
        {
  //          Debug.Log(_spriteRenderers[i].color == Color.white);

            if (_spriteRenderers[_counter].sprite == _spriteArray[3])
            {
                _spriteRenderers[_counter].sprite = _spriteArray[0];
            }

            _spriteRenderers[_counter].enabled = true;
            if (_counter < _spriteRenderers.Count - 1)
            {
                ++_counter;
            }
        };


        /*if (CrossPlatformInputManager.GetButtonDown("ButtonAttack"))
        {
            Debug.Log("Кнопка нажата");
            foreach (var r in _spriteRenderers)
            {
                r.enabled = false;
                r.sprite = spriteArray[3];
            }
            i = 0;
        };*/

    }

 //   void CheckColor()
//    {
//        Debug.Log(renderer[i].material.GetColor());
//    }
}
