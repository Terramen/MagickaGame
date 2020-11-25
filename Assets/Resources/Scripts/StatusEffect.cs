using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class StatusEffect : MonoBehaviourPun, IPunObservable
{
    private HpBar _hpBar;
    [SerializeField] private GameObject[] effects; // временно
    public List<int> burnTimeTicks = new List<int>();

    private GameObject burning;
    private GameObject iceblock;

    private bool _isFreezed;

    public bool IsFreezed
    {
        get => _isFreezed;
        set => _isFreezed = value;
    }
    [SerializeField] private bool _isBurning = false;

    public bool IsBurning
    {
        get => _isBurning;
        set => _isBurning = value;
    }

    [SerializeField] private PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        _hpBar = GetComponent<HpBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeEffectStatus(EffectType effectType, int ticks, float damage)
    {
        switch (effectType)
        {
            case EffectType.BURN: photonView.RPC("ApplyBurnRPC", RpcTarget.All, ticks, damage);
                break;
            case EffectType.FREEZE: photonView.RPC("ApplyFreezeRPC", RpcTarget.All, damage);
                break;
            case EffectType.NOTHING:
                break;
            case EffectType.DOUSED:
                break;
        }
    }

    [PunRPC]
    public void ApplyBurnRPC(int ticks, float damage)
    {
        _isBurning = true;
        burning = Instantiate (effects[0], transform.position, Quaternion.identity);
        burning.transform.parent = gameObject.transform;
        if (burnTimeTicks.Count <= 0)
        {
            burnTimeTicks.Add(ticks);
            StartCoroutine(Burn(damage));
        }
        else
        {
            burnTimeTicks.Add(ticks);
        }
    }

    [PunRPC]
    public void ApplyFreezeRPC(float damage)
    {
        if (_isFreezed)
        {
            _isFreezed = false;
            _hpBar.TakeDamageRPC(damage);
            Destroy(iceblock);
        }
        else
        {
            _isFreezed = true;
            iceblock = Instantiate(effects[1], transform.position, Quaternion.identity);
            iceblock.transform.parent = gameObject.transform;
            StartCoroutine(Freeze());
        }
    }

    [PunRPC]
    IEnumerator Burn(float damage)
    {
        while (burnTimeTicks.Count > 0)
        {
            for (int i = 0; i < burnTimeTicks.Count; i++)
            {
               // Debug.Log("burnTimeTicks: " + burnTimeTicks[i]);
                burnTimeTicks[i]--;
            }
            _hpBar.TakeDamageRPC(damage);
            burnTimeTicks.RemoveAll(i => i == 0);
            yield return new WaitForSeconds(0.75f);
        }
        Destroy(burning);
        _isBurning = false;
    }

    [PunRPC]
    IEnumerator Freeze()
    {
        yield return new WaitForSeconds(2f);
        Destroy(iceblock);
        _isFreezed = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_isFreezed);
            stream.SendNext(_isBurning);
        }
        else
        {
            if (stream.ReceiveNext() is bool b)
            {
                _isFreezed = b;
            }
            if (stream.ReceiveNext() is bool b2)
            {
                _isBurning = b2;
            }
        }
    }
}

public enum EffectType
{
    FREEZE, BURN, NOTHING, DOUSED
}
