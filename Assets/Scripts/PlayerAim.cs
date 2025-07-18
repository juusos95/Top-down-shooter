using System.Collections;
using UnityEngine;
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

    [SerializeField] LineRenderer line;
    [SerializeField] float shotPower;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;

        distance = transform.position.y + cam.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Mouse.current.position.ReadValue();
        /*point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 9));

        target.position = new Vector3(point.x, transform.position.y, point.z);

        transform.LookAt(target, Vector3.up);*/

        Look();
    }

    void OnFire()
    {
        Shoot();
    }

    void Look()
    {
        Ray ray = cam.ScreenPointToRay(mousePos);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red);

        //target.position = new Vector3(ray.GetPoint(distance).x, transform.position.y, ray.GetPoint(distance).z);

        //transform.LookAt(target);

        Vector3 pos = ray.GetPoint(distance);
        direction = pos - transform.position;
        direction.y = transform.position.y;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        }
    }

    void Shoot()
    {
        Ray ray = new Ray(shotOrigin.position, direction);

        line.SetPosition(0, shotOrigin.position);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            line.SetPosition(1, hit.point);
            line.gameObject.SetActive(true);

            hit.rigidbody.AddForce(direction * shotPower);
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
}
