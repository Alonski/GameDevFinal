using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    public GameObject GunInHand;
    public GameObject GunOnGround;
    public GameObject aCamera;
    [SerializeField] public GameObject target;
    private LineRenderer lr;
    public GameObject MuzzleEnd;
    private AudioSource sound;
    public ParticleSystem MuzzleFlash;

    public GameObject[] Allies;
    public GameObject[] Enemies;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        sound = GunInHand.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("TouchBtn") && GunInHand.activeSelf)
        {
            RaycastHit hit;
            if (Physics.Raycast(aCamera.transform.position, aCamera.transform.forward, out hit))
            {
                target.transform.position = hit.point;
                StartCoroutine(ShowShot());

                foreach (var enemy in Enemies)
                {
                    if (hit.transform.gameObject.name == enemy.gameObject.name)
                    {
                        Animator a = enemy.GetComponent<Animator>();
                        a.SetBool("IsDying", true);
                    }
                }
            }
        }
    }
    public IEnumerator ShowShot()
    {
        lr.SetPosition(0, MuzzleEnd.transform.position);
        lr.SetPosition(1, target.transform.position);
        lr.enabled = true;
        target.SetActive(true);
        MuzzleFlash.Play();
        sound.Play();
        yield return new WaitForSeconds(0.1f);
        lr.enabled = false;
        target.SetActive(false);
    }
}
