using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float maxSpeed = 50,minSpeed=5;
    public float sprint = 10.0f;
    public float mouseSensitivity = 100f;  // Sensibilidad del rat�n para rotaci�n
    public float cameraSpeedSensitivityKey=5, cameraSpeedSensitivityWheel = 2;
    private CharacterController controller;
    private float rotationY = 0f;
    private float rotationX = 0f;
    float currentSpeed;
    void Start()
    {
        // Obtiene el componente CharacterController adjunto al objeto
        controller = GetComponent<CharacterController>();
        currentSpeed = speed;
    }

    void Update()
    {
        float scroll = Mathf.Clamp(Input.GetAxis("MouseWheel"),-1,1);
        float scroll2 = Input.GetAxis("CameraSpeed");
        if (scroll != 0||scroll2!=0)
        {
            currentSpeed += scroll* cameraSpeedSensitivityWheel + scroll2 * Time.deltaTime* cameraSpeedSensitivityKey;
            currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
        }
        //print(currentSpeed);
        // Detecta la entrada horizontal del usuario (teclas A/D o flechas izquierda/derecha)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //float sprintInput = (Input.GetAxis("Fire1"))* sprint;
        float upDownInput = Input.GetAxis("UpDown");
        // Crea un vector de movimiento horizontal
        Vector3 move = Camera.main.transform.forward * (verticalInput * currentSpeed + verticalInput) + Camera.main.transform.right*(horizontalInput * currentSpeed);
        Vector3 moveUp = Camera.main.transform.up * (upDownInput * currentSpeed + upDownInput);
        // Mueve el CharacterController en la direcci�n deseada multiplicada por la velocidad y el tiempo
        controller.Move((move + moveUp)  * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Aplica la rotaci�n horizontal al personaje (eje Y)
        rotationX += mouseX;

        // Controla la rotaci�n vertical (limitando la inclinaci�n de la c�mara)
        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f); // Limita el �ngulo vertical entre -90 y 90 grados

        // Aplica la rotaci�n vertical al componente de c�mara
        Camera.main.transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);
    }
}
