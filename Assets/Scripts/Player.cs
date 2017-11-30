using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            int id = Random.Range(1, 19);
            Knapsack.Instance.StoreItem(id);
        }
    }
}
