using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickScr : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Image joystikBG;
    [SerializeField] private Image imageSpell;
    [SerializeField] private Image joystickCooldown;
    [SerializeField] private Sprite notSpellSprite;
    private Vector2 inputVector;
    
    [SerializeField] private GameObject spellTargetRaduis;
    [SerializeField] private RectTransform spellTargetHandle;
    
    [SerializeField] private GameObject spellProjectileRaduis;
    [SerializeField] private RectTransform spellProjectileHandle;
    
    [SerializeField] private GameObject spellParticleRaduis;
    [SerializeField] private RectTransform spellParticleHandle;
    
    
    [SerializeField] private Image spellIcon;
    [SerializeField] private GameObject cancelSpell;

  //  [SerializeField] public CastType castType;

    public Spell spell;

    /*public CastType CastType
    {
        get => castType;
        set => castType = value;
    }*/

    /*public Image Joystik
    {
        get => imageSpell;
        set => imageSpell = value;
    }*/

    public Image JoystickCooldown
    {
        get => joystickCooldown;
        set => joystickCooldown = value;
    }

    private float _angle;

    private float difference;

    [SerializeField] private PlayerSpawner _playerSpawner;

    private Spell _currentSpellCooldown;

    private bool _isCancelledSpell;

    public void ChangeSpell(Spell spell)
    {
        this.spell = spell;
        if (spell != null)
        {
            imageSpell.sprite = spell.SpellIcon;
            CooldownManager.instance.TryGetSpell(spell.ID, out _currentSpellCooldown);
        }
        else
        {
            imageSpell.sprite = notSpellSprite;
            _currentSpellCooldown = null;
        }

        joystickCooldown.fillAmount = 0;
    }

    private void Update()
    {
        if (_currentSpellCooldown != null)
        {
            joystickCooldown.fillAmount =
                _currentSpellCooldown.currentSpellCooldown / _currentSpellCooldown.spellCooldown;
        }
    }


    public virtual void OnPointerDown(PointerEventData ped)
    {
        _playerSpawner.Player3.ClearElements();
        _playerSpawner.HpBar.IsInvulnerable = false;
        if (_currentSpellCooldown != null && _currentSpellCooldown.currentSpellCooldown != 0)
        {
            ChangeSpell(null);
        }
        else if(spell != null)
        {
            if (spell.castType == CastType.TARGET)
            {
                spellTargetRaduis.SetActive(true);
            }
            if (spell.castType == CastType.PROJECTILE)
            {
                spellProjectileRaduis.SetActive(true);
            }

            if (spell.castType == CastType.PARTICLE)
            {
                spellParticleRaduis.SetActive(true);
                CastSpell();
            }
            joystikBG.enabled = true;
            cancelSpell.SetActive(true);
            OnDrag(ped);
        }
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {

        if (spell != null)
        {
            if (_isCancelledSpell)
            {
                CancelCast();
            }
            else if (spell.castType == CastType.TARGET || spell.castType == CastType.PROJECTILE) CastSpell();
            
            if (spell.castType == CastType.TARGET)
            {
                spellTargetHandle.anchoredPosition = Vector2.zero;
                spellTargetRaduis.SetActive(false);
            }
            if (spell.castType == CastType.PROJECTILE)
            {
                spellProjectileHandle.anchoredPosition = Vector2.zero;
                spellProjectileRaduis.SetActive(false);
            }
            if (spell.castType == CastType.PARTICLE)
            {
                if (_playerSpawner.Shooting.WaterflowIsActive)
                {
                    _playerSpawner.Shooting.StopCoroutine(_playerSpawner.Shooting.Coroutine);
                    _playerSpawner.Shooting.WaterflowIsActive = false;
                }
                if (_playerSpawner.Shooting.LavaIsActive)
                {
                    _playerSpawner.Shooting.StopCoroutine(_playerSpawner.Shooting.Coroutine);
                    _playerSpawner.Shooting.LavaIsActive = false;
                }
                spellParticleHandle.anchoredPosition = Vector2.zero;
                spellParticleRaduis.SetActive(false);
            }
        }
        inputVector = Vector2.zero;
        imageSpell.rectTransform.anchoredPosition = Vector2.zero;
        ChangeSpell(null);
        // joystikBG.enabled = false;
        cancelSpell.SetActive(false);
        joystikBG.enabled = false;
    }
    public virtual void OnDrag(PointerEventData ped)
    {
        if (spell != null)
        {
            if (spell.castType == CastType.TARGET)
            {
                TargetCastSpellRadius(ped);
            }

            if (spell.castType == CastType.PROJECTILE)
            {
                ProjectileCastSpellRadius(ped);
            }
            if (spell.castType == CastType.PARTICLE)
            {
                ParticleCastSpellRadius(ped);
            }
        }
    }
    public float Horizontal() 
    {
        if (inputVector.x != 0) return inputVector.x;
        else return Input.GetAxis("Horizontal");
    }
    public float Vertical()
    {
        if (inputVector.y != 0) return inputVector.y;
        else return Input.GetAxis("Vertical");
    }

    public void TargetCastSpellRadius(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystikBG.rectTransform, ped.position, ped.pressEventCamera, out pos)) 
        {
            pos.x = (pos.x / joystikBG.rectTransform.sizeDelta.x);
            pos.y = (pos.y / joystikBG.rectTransform.sizeDelta.y);

            inputVector = new Vector2(pos.x * 2 - 1, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.00f) ? inputVector.normalized : inputVector;

            imageSpell.rectTransform.anchoredPosition = new Vector2(inputVector.x * (joystikBG.rectTransform.sizeDelta.x / 2), inputVector.y * (joystikBG.rectTransform.sizeDelta.y / 2));
        }

        difference = 250f;
        spellTargetHandle.anchoredPosition = new Vector2(Horizontal() * difference, Vertical() * difference);
    }

    public void ProjectileCastSpellRadius(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystikBG.rectTransform, ped.position, ped.pressEventCamera, out pos)) 
        {
            pos.x = (pos.x / joystikBG.rectTransform.sizeDelta.x);
            pos.y = (pos.y / joystikBG.rectTransform.sizeDelta.y);

            inputVector = new Vector2(pos.x * 2 - 1, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 0.00f) ? inputVector.normalized : inputVector;

            imageSpell.rectTransform.anchoredPosition = new Vector2(inputVector.x * (joystikBG.rectTransform.sizeDelta.x / 2), inputVector.y * (joystikBG.rectTransform.sizeDelta.y / 2));
        }
        _angle = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg - 90f;
        spellProjectileHandle.rotation = Quaternion.Euler(45f, 0, _angle);
        difference = 100f;
        spellProjectileHandle.anchoredPosition = new Vector2(Horizontal() * 100f, Vertical() * 100f);
    }

    public void ParticleCastSpellRadius(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystikBG.rectTransform, ped.position, ped.pressEventCamera, out pos)) 
        {
            pos.x = (pos.x / joystikBG.rectTransform.sizeDelta.x);
            pos.y = (pos.y / joystikBG.rectTransform.sizeDelta.y);

            inputVector = new Vector2(pos.x * 2 - 1, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 0.00f) ? inputVector.normalized : inputVector;

            imageSpell.rectTransform.anchoredPosition = new Vector2(inputVector.x * (joystikBG.rectTransform.sizeDelta.x / 2), inputVector.y * (joystikBG.rectTransform.sizeDelta.y / 2));
        }
        _angle = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg - 90f;
        spellParticleHandle.rotation = Quaternion.Euler(45f, 0, _angle);
        difference = 40f;
        spellParticleHandle.anchoredPosition = new Vector2(Horizontal() * difference, Vertical() * difference);
    }

    public void CastSpell()
    {
        if (spell.castType == CastType.TARGET)
        {
            Vector3 point = _playerSpawner.Camera.ScreenToWorldPoint(spellTargetHandle.transform.position);
            point.z = 0;
            // Debug.Log(point);
            _playerSpawner.Shooting.StartTargetAttack(spell, point);
        }

        if (spell.castType == CastType.PROJECTILE)
        {
            _playerSpawner.Shooting.StartAttack(spell);
        }

        if (spell.castType == CastType.PARTICLE)
        {
          //  _playerSpawner.Shooting.StartParticleAttack();
          _playerSpawner.Shooting.StartAttack(spell);
        }
    }

    public void CancelCast()
    {
        _isCancelledSpell = false;
    }

    public void ChangeCancelStateSpell(bool isCancelled)
    {
        _isCancelledSpell = isCancelled;
    }

}

public enum CastType
{
    PROJECTILE, PARTICLE, TARGET
}