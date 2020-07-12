using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizePosition : MonoBehaviour
{
    public GameObject sphere;
    private float timer = 0;
    private float interval = 3f;
    private Vector3 target = Vector3.zero;
    private float speed = 1f;
    void Start()
    {
        sphere.transform.position = new Vector3(Random.Range(-4, 4), Random.Range(-4, 4), Random.Range(-4, 4));
        target = new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0);
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > interval)
        {
            target = new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0);
            timer = 0;
        }
        Vector3 direction = target - transform.position;
        direction.Normalize();
        transform.position += direction * speed * Time.deltaTime;
    }
}
