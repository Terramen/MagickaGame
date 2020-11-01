using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    private HpBar _hpBar;
    [SerializeField] private GameObject[] effects; // временно
    public List<int> burnTimeTicks = new List<int>();

    private GameObject burning;
    private GameObject iceblock;

    private bool _isFreezed;
    // Start is called before the first frame update
    void Start()
    {
        _hpBar = GetComponent<HpBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyBurn(int ticks)
    {
        burning = Instantiate (effects[0], transform.position, Quaternion.identity);
        burning.transform.parent = gameObject.transform;
        if (burnTimeTicks.Count <= 0)
        {
            burnTimeTicks.Add(ticks);
            StartCoroutine(Burn());
        }
        else
        {
            burnTimeTicks.Add(ticks);
        }
    }

    public void ApplyFreeze()
    {
        if (_isFreezed)
        {
            _hpBar.TakeDamage(10);
            _isFreezed = false;
            Destroy(iceblock);
        }
        else
        {
            iceblock = Instantiate(effects[1], transform.position, Quaternion.identity);
            iceblock.transform.parent = gameObject.transform;
            StartCoroutine(Freeze());
            _isFreezed = true;
        }
    }

    IEnumerator Burn()
    {
        while (burnTimeTicks.Count > 0)
        {
            for (int i = 0; i < burnTimeTicks.Count; i++)
            {
                burnTimeTicks[i]--;
            }

            _hpBar.TakeDamage(5);
            burnTimeTicks.RemoveAll(i => i == 0);
            yield return new WaitForSeconds(0.75f);
        }
        Destroy(burning);
    }

    IEnumerator Freeze()
    {
        yield return new WaitForSeconds(2f);
        Destroy(iceblock);
        _isFreezed = false;
    }
}
