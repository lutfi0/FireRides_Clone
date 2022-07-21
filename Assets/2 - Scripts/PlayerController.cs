using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private LineRenderer lr;
    private Vector3 grapplePoint;
    public Transform ballGrapplePoint, player;
    private float maxDistance = 100f;
    private SpringJoint joint;
    private Rigidbody rb;
    public UIManager UIManager;
    public GameObject ParticleController;
    public GameObject ParticleController2;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    public void Start()
    {

        //I used to use this command to clear PlayerPrefs score saves.

        //PlayerPrefs.DeleteAll();

    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.GetComponent<Rigidbody>().useGravity = true;
            StartRope();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopRope();
        }

    }

    void LateUpdate()
    {
        DrawRope();
    }

    void StartRope()
    {
        LayerMask mask = ~(1 << 15);
        RaycastHit hit;

        if (Physics.Raycast(ballGrapplePoint.position, new Vector3(6, 10), out hit, maxDistance, mask))
        {
            StartCoroutine(CoolDown(1f));
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            rb.AddForce(new Vector3(85, 0, 0));
            rb.AddForce(new Vector3(0, 45, 0));

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.minDistance = distanceFromPoint * 0.25f;
            joint.maxDistance = distanceFromPoint * 0.5f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
        }
    }

    void DrawRope()
    {
        if (!joint) return;
        lr.SetPosition(0, ballGrapplePoint.position);
        lr.SetPosition(1, grapplePoint);

    }

    void StopRope()
    {
        StartCoroutine(ExecuteAfterTime(0.5f));
        lr.positionCount = 0;
        Destroy(joint);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "50")
        {
            GameManager.Instance.score += 50;
            ParticleController2.SetActive(true);
            StartCoroutine(ExecuteAfterTime(1f));
            Destroy(other.gameObject);

        }
        else if (other.gameObject.tag == "150")
        {
            GameManager.Instance.score += 150;
            ParticleController.SetActive(true);
            StartCoroutine(ExecuteAfterTime(1f));
            Destroy(other.gameObject);

        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield
        return new WaitForSeconds(time);

        ParticleController.SetActive(false);
        ParticleController2.SetActive(false);

    }

    IEnumerator CoolDown(float time)
    {
        yield
        return new WaitForSeconds(time);

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Gameover")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}