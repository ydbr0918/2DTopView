using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class ItemObject : MonoBehaviour
{
    [SerializeField] ItemSO data;              //Inspector 드래그

    public int GetPoint()
    {
        return data.point; //ItemSo의 Point값 반환
    }
}

