using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackSpawn : MonoBehaviour
{
    [SerializeField] bool isCrack;

    [SerializeField] List<GameObject> cracksGroup;

    public bool GetCrackState()
    {
        return isCrack;
    }

    public void CrackSpawnSetting()
    {
        int crackBe = Random.Range(0, 2);

        if(crackBe == 1)
        {
            isCrack = true;
        }
        else
        {
            isCrack = false;
        }

        if(isCrack)
        {
            int i = Random.Range(0, cracksGroup.Count);

            cracksGroup[i].SetActive(true);
        }
    }
}
