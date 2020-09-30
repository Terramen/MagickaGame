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
    
    void Start()
    {
      
    }
    
    // _spriteArray 0 - вода, 1 - огонь, 2 - земля, 3 - пустой, 4 - молния, 5 - лава, 6 - холод
    void Update()
    {
        // ВОЗМОЖНО ОПТИМИЗИРОВАТЬ КОД
        if (CrossPlatformInputManager.GetButtonDown("ButtonFire"))
        {
            if (_spriteRenderers[_counter].sprite == _spriteArray[3]) // + огонь из пустого
            {
                _spriteRenderers[_counter].sprite = _spriteArray[1];
                _spriteRenderers[_counter].enabled = true;
            }
            
            else if (_spriteRenderers[_counter].sprite == _spriteArray[1] && _counter < _spriteRenderers.Count) // + огонь (если пред огонь)
            {
                if (_counter < _spriteRenderers.Count - 1)
                {
                    ++_counter; 
                }
                _spriteRenderers[_counter].sprite = _spriteArray[1];
                _spriteRenderers[_counter].enabled = true;
            }
            
            else if (_spriteRenderers[_counter].sprite == _spriteArray[0] && _counter <= _spriteRenderers.Count - 1)   // вода + огонь
            {
                _spriteRenderers[_counter].sprite = _spriteArray[4];
                if (_counter < _spriteRenderers.Count - 1)
                {
                    ++_counter; 
                }
            }
            
            else if (_spriteRenderers[_counter].sprite == _spriteArray[2] && _counter <= _spriteRenderers.Count - 1) // земля + огонь
            {
                _spriteRenderers[_counter].sprite = _spriteArray[5];
                if (_counter < _spriteRenderers.Count - 1)
                {
                    ++_counter; 
                }
            }
        };

        if (CrossPlatformInputManager.GetButtonDown("ButtonEarth"))
        {



            if (_spriteRenderers[_counter].sprite == _spriteArray[3]) // + земля из пустого
            {
                _spriteRenderers[_counter].sprite = _spriteArray[2];
                _spriteRenderers[_counter].enabled = true;
            }
            
            else if (_spriteRenderers[_counter].sprite == _spriteArray[2] && _counter <= _spriteRenderers.Count - 1) // + земля (если пред земля)
            {
                if (_counter < _spriteRenderers.Count - 1)
                {
                    ++_counter; 
                }
                _spriteRenderers[_counter].sprite = _spriteArray[2];
                _spriteRenderers[_counter].enabled = true;
            }

            else if (_spriteRenderers[_counter].sprite == _spriteArray[0] && _counter <= _spriteRenderers.Count - 1)   // вода + земля
            {
                _spriteRenderers[_counter].sprite = _spriteArray[6];
                if (_counter < _spriteRenderers.Count - 1)
                {
                    ++_counter; 
                }
            }
            
            else if (_spriteRenderers[_counter].sprite == _spriteArray[1] && _counter <= _spriteRenderers.Count - 1) // огонь + земля
            {
                _spriteRenderers[_counter].sprite = _spriteArray[5];
                if (_counter < _spriteRenderers.Count - 1)
                {
                    ++_counter; 
                }
            }
        };

        if (CrossPlatformInputManager.GetButtonDown("ButtonWater"))
        {
            if (_spriteRenderers[_counter].sprite == _spriteArray[3]) // + вода из пустого
            {
                _spriteRenderers[_counter].sprite = _spriteArray[0];
                _spriteRenderers[_counter].enabled = true;
            }
            
            else if (_spriteRenderers[_counter].sprite == _spriteArray[0] && _counter <= _spriteRenderers.Count - 1) // + вода (если пред вода)
            {
                if (_counter < _spriteRenderers.Count - 1)
                {
                    ++_counter; 
                };
                _spriteRenderers[_counter].sprite = _spriteArray[0];
                _spriteRenderers[_counter].enabled = true;
            }
            
            else if (_spriteRenderers[_counter].sprite == _spriteArray[1] && _counter <= _spriteRenderers.Count - 1)   // огонь + вода
            {
                _spriteRenderers[_counter].sprite = _spriteArray[4];
                if (_counter < _spriteRenderers.Count - 1)
                {
                    ++_counter; 
                }
            }
            
            else if (_spriteRenderers[_counter].sprite == _spriteArray[2] && _counter <= _spriteRenderers.Count - 1) // земля + вода
            {
                _spriteRenderers[_counter].sprite = _spriteArray[6];
                if (_counter < _spriteRenderers.Count - 1)
                {
                    ++_counter; 
                }
            }
        };
    }
}
