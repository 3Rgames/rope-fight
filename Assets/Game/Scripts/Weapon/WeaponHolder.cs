using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private List<GameObject> _weapons = new List<GameObject>();

    public List<Material> ShowRandomWeapon()
    {
        for (int i = 0; i < _weapons.Count; i++)
        {
            _weapons[i].SetActive(false);
        }

        int index = Random.Range(0, _weapons.Count);
        _weapons[index].SetActive(true);

        List<Material> _materials = new List<Material>();
        MeshRenderer _meshRenderer = _weapons[index].GetComponent<MeshRenderer>();
        for (int i = 0; i < _meshRenderer.materials.Length; i++)
        {
            _materials.Add(_meshRenderer.materials[i]); 
        }
        return _materials;
    }

}
