using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTowards : MonoBehaviour
{
    public GameObject unit;
    public GameObject enemy;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        unit.transform.position = Vector3.MoveTowards(unit.transform.position, enemy.transform.position, speed);
    }
}
