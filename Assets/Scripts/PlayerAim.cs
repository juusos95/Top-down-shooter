using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    Vector3 mousePos;
    [SerializeField] Transform mouseTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos = new Vector3(mousePos.x, 1, mousePos.z);

        Debug.Log(mousePos);
    }
}
