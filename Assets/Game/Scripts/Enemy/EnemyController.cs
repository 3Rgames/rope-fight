using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Components")]
    [Space]
    [SerializeField] private Animator _animator;
    [SerializeField] private CopyLimb _copyLimb;
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    private Material _material;
    [SerializeField] private Color _gray = Color.gray;
    public Rigidbody Rb;
    public string DeadEnemyLayer;
    [Space]
    [Header("Controlling")]
    [Space]
    public float ForcePower = 15f;
    [SerializeField] private float _timeToNewLayer = 1f;
    [SerializeField] private float _timeToChangeColor = 0.5f;
    [SerializeField] private float _timeToDead = 3f;
    [SerializeField] private float _distanceVisible = 5f;
    [SerializeField] private float _movingSpeed = 5f;
    [SerializeField] private float _rotatingSpeed = 3f;
    [Space]
    [SerializeField, ReadOnly] private Transform _target;
    [ReadOnly] public bool IsDead = false;
    [Space]
    [Header("Weapons")]
    [SerializeField] private List<WeaponHolder> _weaponHolders = new List<WeaponHolder>();
    [Header("VFX")]
    [SerializeField] private List<ParticleSystem> _bloods = new List<ParticleSystem>();
    private List<Material> _weaponMat = new List<Material>();

    private void Start()
    {
        _target = GameObject.FindObjectOfType<PlayerController>().transform;
        _weaponMat = _weaponHolders[Random.Range(0, _weaponHolders.Count)].ShowRandomWeapon();
        _material = _skinnedMeshRenderer.material;
    }

    private void FixedUpdate()
    {
        if (IsDead)
        {
            _copyLimb.IsPlayerActive = false;
            return;
        }
        if (!IsTargetVisible(_target))
        {
            return;
        }

        LookAtTarget(_target);
        Move();
    }

    private void LookAtTarget(Transform target)
    {
        Quaternion lookTarget = Quaternion.LookRotation(target.position - transform.position);
        Rb.rotation = Quaternion.Lerp(Rb.rotation, lookTarget, Time.fixedDeltaTime * _rotatingSpeed);
    }

    private void Move()
    {
        _animator.SetBool("Run", true);
        Rb.velocity = new Vector3(transform.forward.x * _movingSpeed, Rb.velocity.y, transform.forward.z * _movingSpeed);
    }

    private bool IsTargetVisible(Transform target)
    {
        if (Vector3.Distance(target.position, transform.position) <= _distanceVisible)
            return true;
        else
            return false;
    }

    public void BloodSpawn(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        var particle = Instantiate(_bloods[Random.Range(0, _bloods.Count)], position, rotation, null);
        particle.transform.localScale = scale;
        particle.Play();
    }

    public Coroutine LayerCor;
    public Coroutine DeadCor;
    public Coroutine ColorCor;

    public IEnumerator ColorChangeCor()
    {
        Color startColor = _material.color;
        Color startColorWeapon = _weaponMat[0].color;
        for (float t = 0; t < 1; t += Time.deltaTime / _timeToChangeColor)
        {
            _material.color = Color.Lerp(startColor, _gray, t);
            for (int i = 0; i < _weaponMat.Count; i++)
            {
                _weaponMat[i].color = Color.Lerp(startColorWeapon, _gray, t);
            }
            yield return null;
        }
        _material.color = _gray;
        for (int i = 0; i < _weaponMat.Count; i++)
        {
            _weaponMat[i].color = _gray;
        }
    }

    public IEnumerator DeadChangeCor()
    {
        yield return new WaitForSeconds(_timeToDead);
        Destroy(this.gameObject);
    }

    public IEnumerator LayerChangeCor()
    {
        yield return new WaitForSeconds(_timeToNewLayer);
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer(DeadEnemyLayer);
        }
    }
}
