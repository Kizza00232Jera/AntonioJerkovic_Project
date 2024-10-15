using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
       Instantiate(enemyPrefab, new Vector3(3,0.3f,20),enemyPrefab.transform.rotation); 
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
