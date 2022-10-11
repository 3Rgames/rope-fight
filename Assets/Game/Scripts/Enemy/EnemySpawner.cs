using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Component")]
    [Space]
    [SerializeField] private Transform _player;
    [SerializeField] private EnemyController _enemy;
    [SerializeField] private LayerMask _targetLayer;
    [Space]
    [Header("Parameters")]
    [Space]
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private float _deadZoneRadius = 2f;
    [SerializeField] private float _timeToSpawn = 3f;
    [SerializeField] private int _countToSpawn = 1;
    [Space]
    [SerializeField] private bool _isSpawning = true;

    private void Start()
    {
        SpawnCor();
    }

    private void SpawnCor()
    {
        IEnumerator Cor()
        {
            while (_isSpawning)
            {
                yield return new WaitForSeconds(_timeToSpawn);
                for (int i = 0; i < _countToSpawn; i++)
                {
                    bool _wasSpawned = false;
                    int counter = 0;
                    while(!_wasSpawned)
                    {
                        Vector2 playerPos = new Vector2(_player.transform.position.x, _player.transform.position.z);
                        Vector2 pos = playerPos + Random.insideUnitCircle * _spawnRadius;

                        if ((pos.x * pos.x) + (pos.y * pos.y) > _deadZoneRadius * _deadZoneRadius)
                        {
                            counter++;
                            Vector3 rayStart = new Vector3(pos.x, _enemy.transform.position.y + 5f, pos.y);

                            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity, _targetLayer))
                            {
                                Instantiate(_enemy, new Vector3(pos.x, _enemy.transform.position.y, pos.y), Quaternion.identity, transform);
                                _wasSpawned = true;
                            }
                        }
                        if (counter >= 50)
                            yield break;
                    }
                }
            }
        }
        StartCoroutine(Cor());
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(_player.transform.position, Vector3.up, _spawnRadius, 5f);
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(_player.transform.position, Vector3.up, _deadZoneRadius, 5f);
    }
#endif
}
