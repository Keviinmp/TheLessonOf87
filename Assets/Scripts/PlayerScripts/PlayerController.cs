using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float velocidadRotacion;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform transformPlayer;
    [SerializeField] private Camera camaraPlayer;

    private Vector3 movimiento;
    private float rotacionX;

    void Start()
    {
    rotacionX = 0f;

    transform.rotation = Quaternion.Euler(0f, 90f, 0f);
    camaraPlayer.transform.localRotation = Quaternion.identity;

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    }

    private void Update()
    {
        MovimientoPlayer();
        MovimientoCamara();
    }

    void MovimientoPlayer()
    {
        float movX = Input.GetAxis("Horizontal");
        float movZ = Input.GetAxis("Vertical");

        movimiento = transform.right * movX + transform.forward * movZ;
        characterController.SimpleMove(movimiento * velocidadMovimiento);
    }

    void MovimientoCamara()
    {
        float ratonX = Input.GetAxis("Mouse X") * velocidadRotacion;
        float ratonY = Input.GetAxis("Mouse Y") * velocidadRotacion;

        rotacionX -= ratonY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);

        camaraPlayer.transform.localRotation = Quaternion.Euler(rotacionX, 0, 0);
        transformPlayer.Rotate(Vector3.up * ratonX);
    }
}