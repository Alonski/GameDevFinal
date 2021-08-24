using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGroundScript : MonoBehaviour
{
    public GameObject gunOnPlayer;
    public Transform[] points;

    void OnMouseDown()
    {
        if (gunOnPlayer.activeSelf)
        {
            return;
        }
        gunOnPlayer.SetActive(true);

        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        var position = Random.RandomRange(0, points.Length);
        transform.position = points[position].position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
