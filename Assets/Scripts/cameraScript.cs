using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public Transform ninja1; // Transform del primer ninja
    public Transform ninja2; // Transform del segundo ninja

    public float smoothTime = 0.3f; // Tiempo de suavizado
    private Vector3 velocity = Vector3.zero; // Velocidad para el suavizado

    public float minZoom = 5f; // Zoom mínimo de la cámara (cuando los ninjas están muy cerca)
    public float maxZoom = 15f; // Zoom máximo de la cámara (cuando los ninjas están muy lejos)
    public float zoomLimiter = 10f; // Limitador de zoom (ajusta este valor para controlar el comportamiento del zoom)

    public float maxHeight = 10; // Altura máxima a la que puede subir la cámara
    public float minHeight = -10; // Altura mínima a la que puede bajar la cámara

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>(); // Obtiene la cámara adjunta a este script
    }

    void LateUpdate()
    {
        if (ninja1 != null && ninja2 != null)
        {
            MoveCamera();
            ZoomCamera();
        }
    }

    void MoveCamera()
    {
        // Calcula la posición promedio entre los dos ninjas
        Vector3 newPosition = (ninja1.position + ninja2.position) / 2f;

        // Fija la posición en el eje Z a -10 para mantener la cámara en la posición correcta
        newPosition.z = -10f;

        // Asegura que la posición en Y de la cámara esté dentro de los límites
        newPosition.y = Mathf.Clamp(newPosition.y, minHeight, maxHeight);

        // Actualiza la posición de la cámara suavemente
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void ZoomCamera()
    {
        // Calcula la distancia entre los dos ninjas
        float distance = Vector3.Distance(ninja1.position, ninja2.position);

        // Invertimos el cálculo del zoom para que se acerque cuando los ninjas estén cerca y se aleje cuando estén lejos
        float newZoom = Mathf.Lerp(minZoom, maxZoom, distance / zoomLimiter);

        // Ajusta el tamaño de la cámara suavemente
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }
}
