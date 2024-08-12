using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerParts : MonoBehaviour
{
    public GameObject[] gameObjects;  // Array para asignar los GameObjects
    private int activeIndex1 = -1;
    private int activeIndex2 = -1;



    void Start()
    {
        ActivateRandomObjects();
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




}
