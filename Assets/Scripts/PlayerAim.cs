using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    Vector3 mousePos;
    Vector3 point;

    [SerializeField] Transform target;
    float distance;
    Vector3 direction;

    Camera cam;

    [SerializeField] Transform shotOrigin;
    [SerializeField] LayerMask enemy;
    [SerializeField] LayerMask environment;
    [SerializeField] LayerMask collectible;

    [SerializeField] LineRenderer line;
    [SerializeField] float shotPower;
    [SerializeField] float shotTime;
    float timer;

    [SerializeField] Score score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;

        distance = transform.position.y + cam.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < shotTime)
        {
            timer += Time.deltaTime;
        }

        mousePos = Mouse.current.position.ReadValue();
        /*point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 9));

        target.position = new Vector3(point.x, transform.position.y, point.z);

        transform.LookAt(target, Vector3.up);*/

        Look();
        line.SetPosition(0, shotOrigin.position);
    }

    void OnFire()
    {
        if (timer > shotTime)
        {
            Shoot();
            timer = 0;
        }
    }

    void Look()
    {
        Ray ray = cam.ScreenPointToRay(mousePos);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red);

        //target.position = new Vector3(ray.GetPoint(distance).x, transform.position.y, ray.GetPoint(distance).z);

        //transform.LookAt(target);

        Vector3 pos = ray.GetPoint(distance);
        direction = pos - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        }
    }

    void Shoot()
    {
        Ray ray = new Ray(shotOrigin.position, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            line.SetPosition(1, hit.point);
            line.gameObject.SetActive(true);

            if (hit.collider.gameObject.CompareTag("Collectible"))
            {
                score.addPoint();
                Destroy(hit.collider.gameObject);
            }
            else if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                hit.rigidbody.AddForce(direction * shotPower);
                StartCoroutine(gotShot(hit.rigidbody.gameObject, hit.rigidbody));
            }
        }
        else
        {
            line.SetPosition(1, direction * 100);
            line.gameObject.SetActive(true);
        }

        StartCoroutine(shotTimer());
    }

    IEnumerator shotTimer()
    {
        yield return new WaitForSeconds(0.05f);
        line.gameObject.SetActive(false);
    }
    IEnumerator gotShot(GameObject victim, Rigidbody victimRb)
    {
        NavMeshAgent agent = victim.GetComponent<NavMeshAgent>();
        agent.enabled = false;

        yield return new WaitForSeconds(0.5f);

        victimRb.isKinematic = true;
        victimRb.isKinematic = false;

        agent.enabled = true;
    }
}
