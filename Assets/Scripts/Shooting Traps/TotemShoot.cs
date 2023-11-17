using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemShoot : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _shootDelay;
    [SerializeField] private int _detectionRange;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private bool _showDetection;
    [SerializeField] private Transform _projectileSpawnPoint;
    private float _detectionDelay = 0.3f;
    private Vector2 _detectionPoint;
    private Vector2 _detectionSize;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        var orientation = (float)(_detectionRange) / 2 + 0.6f;

        if (transform.rotation.y == 1) 
        { 
            orientation = -orientation;
        }

        _detectionPoint = new Vector2(transform.position.x + orientation, transform.position.y - 0.65f);
        _detectionSize = new Vector2(_detectionRange, 1);
    }

    private void Start()
    {
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine() 
    { 
        while (true)
        {
            if (Detect() == true) 
            {
                _animator.SetTrigger("ShootTrigger");
                yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
                yield return new WaitWhile(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("Totem_shoot"));
                Instantiate(_projectilePrefab, _projectileSpawnPoint.position, _projectileSpawnPoint.rotation);
                yield return new WaitForSeconds(_shootDelay);
            }
            yield return new WaitForSeconds(_detectionDelay);
        }
    }

    private bool Detect() 
    {
        Collider2D collider = Physics2D.OverlapBox(_detectionPoint, _detectionSize, 0, _layerMask);
        if (collider != null)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (_showDetection == true)
        {
            var orientation = (float)(_detectionRange) / 2 + 0.6f;

            if (transform.rotation.y == 1)
            {
                orientation = -orientation;
            }

            Gizmos.color = new Color(1, 0.92f, 0.016f, 0.5f);
            Gizmos.DrawCube(new Vector2(transform.position.x + orientation, transform.position.y - 0.65f), new Vector2(_detectionRange, 1));
        }
    }
}
