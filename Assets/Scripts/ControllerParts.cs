using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Asegúrate de tener esta referencia para usar TMP_Text

public class ControllerParts : MonoBehaviour
{
    public List<GameObject> gameObjects;  // Array para asignar los GameObjects
    public TMP_Text collectedPartsText; // Campo para el texto en el canvas
    private int activeIndex1 = -1;
    private int activeIndex2 = -1;
    public int collectedPartsCount = 0; // Contador de piezas recogidas

    [SerializeField] private GameObject GameBoard;
    [SerializeField] private GameObject GameManager;

    void Start()
    {
        ActivateRandomObjects();
        UpdateCollectedPartsText(); // Inicializar el texto del contador
    }

    void Update() {
        
    }

    public void ActivateRandomObjects()
    {
        Debug.Log(gameObjects.Count);
        if(gameObjects.Count != 0)
        { 
            List<GameObject> validGameObjects = new List<GameObject>();
            // Desactivar todos los GameObjects primero
            foreach (GameObject obj in gameObjects)
            {
                obj.SetActive(false);
            }

            // Seleccionar dos índices aleatorios diferentes
            activeIndex1 = Random.Range(0, gameObjects.Count);
            do
            {
                activeIndex2 = Random.Range(0, gameObjects.Count);
            } while (activeIndex2 == activeIndex1);

            // Activar los GameObjects seleccionados
            gameObjects[activeIndex1].SetActive(true);
            gameObjects[activeIndex2].SetActive(true);
        }else
        {
            //logica para activar el puzzle
            //Debug.Log("Piezas recolectadas");
            GameManager.SetActive(true);
            GameBoard.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        // Verificar si el objeto con el que colisionamos es una de las piezas activas
        for (int i = 0; i < gameObjects.Count; i++)
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

    public void UpdateCollectedPartsText()
    {
        collectedPartsText.text = "" + collectedPartsCount;
    }
}