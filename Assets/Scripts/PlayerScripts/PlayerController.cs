using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>Controla el movimiento del jugador y la cámara en primera persona.</summary>
public class PlayerController : MonoBehaviour {

    /// <summary>Velocidad de movimiento del jugador.</summary>
    [SerializeField] private float velocidadMovimiento;

    /// <summary>Sensibilidad de rotación de la cámara.</summary>
    [SerializeField] private float velocidadRotacion;

    /// <summary>CharacterController usado para mover al jugador.</summary>
    [SerializeField] private CharacterController characterController;

    /// <summary>Transform del cuerpo del jugador.</summary>
    [SerializeField] private Transform transformPlayer;

    /// <summary>Cámara del jugador (primera persona).</summary>
    [SerializeField] private Camera camaraPlayer;

    /// <summary>Dirección de movimiento actual.</summary>
    private Vector3 movimiento;

    /// <summary>Rotación vertical acumulada de la cámara.</summary>
    private float rotacionX;


    /// <summary>Reinicia la orientación del jugador y de la cámara y bloquea el cursor.</summary>
    void Start() {

        rotacionX = 0f;

        // Fuerza orientación inicial del jugador en el mundo
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);

        // Reinicia rotación local de la cámara
        camaraPlayer.transform.localRotation = Quaternion.identity;

        // Bloquea y oculta el cursor para control tipo FPS
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    /// <summary>Actualiza el movimiento y la cámara cada frame.</summary>
    private void Update() {

        MovimientoPlayer();
        MovimientoCamara();
    }


    /// <summary>Mueve al jugador con el teclado, relativo a su orientación.</summary>
    void MovimientoPlayer() {

        float movX = Input.GetAxis("Horizontal");
        float movZ = Input.GetAxis("Vertical");

        // Dirección relativa al jugador (forward/right)
        movimiento = transform.right * movX + transform.forward * movZ;

        // Aplicación del movimiento con CharacterController
        characterController.SimpleMove(movimiento * velocidadMovimiento);
    }


    /// <summary>Rota la cámara (vertical) y el cuerpo del jugador (horizontal) con el ratón.</summary>
    void MovimientoCamara() {

        float ratonX = Input.GetAxis("Mouse X") * velocidadRotacion;
        float ratonY = Input.GetAxis("Mouse Y") * velocidadRotacion;

        // Acumula rotación vertical (eje X)
        rotacionX -= ratonY;

        // Limita la rotación vertical para evitar giros completos
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);

        // Aplica rotación vertical a la cámara
        camaraPlayer.transform.localRotation = Quaternion.Euler(rotacionX, 0, 0);

        // Aplica rotación horizontal al cuerpo del jugador
        transformPlayer.Rotate(Vector3.up * ratonX);
    }
}