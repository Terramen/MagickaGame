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
    private GameObject water;

    private Coroutine _burningCoroutine;
    private Coroutine _dousedCoroutine;
    private Coroutine _freezeCoroutine;

    private bool _isFreezed;

    private Player_transform _playerTransform;

    public bool IsFreezed
    {
        get => _isFreezed;
        set => _isFreezed = value;
    }
    [SerializeField] private bool _isBurning;

    public bool IsBurning
    {
        get => _isBurning;
        set => _isBurning = value;
    }

    private bool _isDoused;

    public bool IsDoused
    {
        get => _isDoused;
        set => _isDoused = value;
    }

    [SerializeField] private PhotonView photonView;
    private PlayerSpawner _playerSpawner;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerSpawner = FindObjectOfType<PlayerSpawner>();
        _playerTransform = GetComponent<Player_transform>();
        _hpBar = GetComponent<HpBar>();
    }
    
    public void ChangeEffectStatus(EffectType effectType, int ticks, float damage, float effectTime)
    {
        switch (effectType)
        {
            case EffectType.BURN: photonView.RPC("ApplyBurnRPC", RpcTarget.All, ticks, damage, effectTime);
                break;
            case EffectType.FREEZE: photonView.RPC("ApplyFreezeRPC", RpcTarget.All, damage, effectTime);
                break;
            case EffectType.DOUSED: photonView.RPC("ApplyDousedRPC", RpcTarget.All, effectTime);
                break;
            case EffectType.NOTHING:
                break;
        }
    }

    [PunRPC]
    public void ApplyBurnRPC(int ticks, float damage, float time)
    {

        if (!_isBurning)
        {
            _isBurning = true;
            burning = Instantiate(effects[0], transform.position, Quaternion.identity);
            burning.transform.parent = gameObject.transform;
            if (burnTimeTicks.Count <= 0)
            {
                burnTimeTicks.Add(ticks); 
                _burningCoroutine = StartCoroutine(Burn(damage, time));
            }
            else
            {
                burnTimeTicks.Add(ticks);
            }
        }
    }

    [PunRPC]
    public void ApplyFreezeRPC(float damage, float time)
    {
        if (_isFreezed)
        {
            _isFreezed = false;
            _hpBar.TakeDamageRPC(damage);
            _playerTransform.enabled = true;
           // _playerTransform.moveSpeed = 3.5f;
            Destroy(iceblock);
            //PhotonNetwork.Instantiate("Prefabs/IceBlockCrush", transform.position, transform.rotation);
        }
        else
        {
            if (_isBurning)
            {
                StopBurningEffect();
            }
            _isFreezed = true;
            iceblock = Instantiate(effects[1], transform.position, Quaternion.identity);
            iceblock.transform.parent = gameObject.transform;
           // _playerTransform.moveSpeed = 0; // ???
            _playerTransform.enabled = false;
            _freezeCoroutine = StartCoroutine(Freeze(time));
        }
    }

    [PunRPC]
    public void ApplyDousedRPC(float time)
    {
        if (!_isDoused)
        {
            if (_isBurning)
            {
                StopBurningEffect();
            }
            _isDoused = true;
            water = Instantiate(effects[2], transform.position, Quaternion.identity);
            water.transform.parent = gameObject.transform;
            var position = water.transform.position;
            position = new Vector3(position.x, position.y - 0.2f, position.z);
            water.transform.position = position;
            _dousedCoroutine = StartCoroutine(Doused(time));
        }
    }

    [PunRPC]
    IEnumerator Burn(float damage, float time)
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
            //yield return new WaitForSeconds(0.75f);
            yield return new WaitForSeconds(time);
        }
        Destroy(burning);
        _isBurning = false;
    }

    [PunRPC]
    IEnumerator Freeze(float time)
    {
        /*yield return new WaitForSeconds(2f);*/
        yield return new WaitForSeconds(time);
        Destroy(iceblock);
        _playerTransform.enabled = true;
        _isFreezed = false;
    }

    [PunRPC]
    IEnumerator Doused(float time)
    {
        _playerSpawner.Shooting.Multiplier = 1.5f;
        yield return new WaitForSeconds(time);
        Destroy(water);
        _isDoused = false;
        _playerSpawner.Shooting.Multiplier = 1f;
    }

    /*public void StopStatusEffect(Coroutine coroutine, GameObject effect, bool action)
    {
        action = false;
    }*/

    [PunRPC]
    public void StopBurningEffect()
    {
        StopCoroutine(_burningCoroutine);
        burnTimeTicks.Clear();
        Destroy(burning);
        IsBurning = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_isFreezed);
            stream.SendNext(_isBurning);
            stream.SendNext(_isDoused);
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
            if (stream.ReceiveNext() is bool b3)
            {
                _isDoused = b3;
            }
        }
    }
}

public enum EffectType
{
    FREEZE, BURN, NOTHING, DOUSED
}
