using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Asegúrate de tener esta referencia para usar TMP_Text

public class ControllerParts : MonoBehaviour
{
    public GameObject[] gameObjects;  // Array para asignar los GameObjects
    public TMP_Text collectedPartsText; // Campo para el texto en el canvas
    private int activeIndex1 = -1;
    private int activeIndex2 = -1;
    private int collectedPartsCount = 0; // Contador de piezas recogidas

    void Start()
    {
        ActivateRandomObjects();
        UpdateCollectedPartsText(); // Inicializar el texto del contador
    }

    void Update() {
        
    }

    public void ActivateRandomObjects()
    {
        // Desactivar todos los GameObjects primero
        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(false);
        }

        // Seleccionar dos índices aleatorios diferentes
        activeIndex1 = Random.Range(0, gameObjects.Length);
        do
        {
            activeIndex2 = Random.Range(0, gameObjects.Length);
        } while (activeIndex2 == activeIndex1);

        // Activar los GameObjects seleccionados
        gameObjects[activeIndex1].SetActive(true);
        gameObjects[activeIndex2].SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto con el que colisionamos es una de las piezas activas
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (other.gameObject == gameObjects[i] && gameObjects[i].activeSelf)
            {
                gameObjects[i].SetActive(false); // Desactivar la pieza recogida
                collectedPartsCount++; // Incrementar el contador de piezas recogidas
                UpdateCollectedPartsText(); // Actualizar el texto del contador
                break;
            }
        }
    }

    private void UpdateCollectedPartsText()
    {
        collectedPartsText.text = "" + collectedPartsCount;
    }
}