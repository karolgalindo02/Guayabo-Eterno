using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  // Referencias serializadas a los objetos Transform del juego y las piezas.
  [SerializeField] private Transform gameTransform; 
  [SerializeField] private Transform piecePrefab; 
  
  // Lista que almacenará las piezas del juego.
  private List<Transform> pieces; 
  // Índice de la ubicación vacía en el tablero.
  private int emptyLocation; 
  // Tamaño del tablero (4x4, por ejemplo).
  private int size; 
  // Indica si se está mezclando el tablero.
  private bool shuffling = false; 

  // Método para crear las piezas del juego con un espacio entre ellas (gap).
  private void CreateGamePieces(float gapThickness) {
    // Calcula el ancho de cada pieza en función del tamaño del tablero.
    float width = 1 / (float)size;
    // Recorre las filas del tablero.
    for (int row = 0; row < size; row++) {
      // Recorre las columnas del tablero.
      for (int col = 0; col < size; col++) {
        // Instancia una nueva pieza en la escena.
        Transform piece = Instantiate(piecePrefab, gameTransform);
        pieces.Add(piece);
        // Posiciona la pieza en el tablero, que va de -1 a +1 en el espacio local.
        piece.localPosition = new Vector3(-1 + (2 * width * col) + width,
                                          +1 - (2 * width * row) - width,
                                          0);
        // Ajusta la escala de la pieza según el ancho y el espacio.
        piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
        // Nombra la pieza según su posición en el tablero.
        piece.name = $"{(row * size) + col}";
        // Si es la última posición, desactiva la pieza para crear un espacio vacío.
        if ((row == size - 1) && (col == size - 1)) {
          emptyLocation = (size * size) - 1;
          piece.gameObject.SetActive(false);
        } else {
          // Ajusta las coordenadas UV de la pieza para mapear la textura correctamente.
          float gap = gapThickness / 2;
          Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
          Vector2[] uv = new Vector2[4];
          // Asigna las coordenadas UV en el orden adecuado.
          uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
          uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap));
          uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
          uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap));
          // Asigna las nuevas coordenadas UV al mesh.
          mesh.uv = uv;
        }
      }
    }
  }

  // Método llamado al inicio del juego.
  void Start() {
    // Inicializa la lista de piezas.
    pieces = new List<Transform>();
    // Define el tamaño del tablero.
    size = 3;
    // Crea las piezas del juego con un pequeño espacio entre ellas.
    CreateGamePieces(0.02f);
  }

  // Método llamado una vez por cuadro.
  void Update() {
    // Verifica si el rompecabezas está completo.
    if (!shuffling && CheckCompletion()) {
      shuffling = true;
      StartCoroutine(WaitShuffle(0.5f));
    }

    // Detecta si se ha hecho clic en el ratón.
    if (Input.GetMouseButtonDown(0)) {
      RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
      if (hit) {
        // Recorre la lista de piezas para encontrar la que se ha hecho clic.
        for (int i = 0; i < pieces.Count; i++) {
          if (pieces[i] == hit.transform) {
            // Verifica si se puede intercambiar la pieza en alguna dirección válida.
            if (SwapIfValid(i, -size, size)) { break; }
            if (SwapIfValid(i, +size, size)) { break; }
            if (SwapIfValid(i, -1, 0)) { break; }
            if (SwapIfValid(i, +1, size - 1)) { break; }
          }
        }
      }
    }
  }

  // Verifica si es válido intercambiar la pieza actual con la posición vacía.
  private bool SwapIfValid(int i, int offset, int colCheck) {
    if (((i % size) != colCheck) && ((i + offset) == emptyLocation)) {
      // Intercambia las piezas en la lista y en sus posiciones en el juego.
      (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
      (pieces[i].localPosition, pieces[i + offset].localPosition) = ((pieces[i + offset].localPosition, pieces[i].localPosition));
      // Actualiza la ubicación vacía.
      emptyLocation = i;
      return true;
    }
    return false;
  }

  // Verifica si las piezas están en orden para completar el juego.
  private bool CheckCompletion() {
    for (int i = 0; i < pieces.Count; i++) {
      if (pieces[i].name != $"{i}") {
        return false;
      }
    }
    return true;
  }

  // Espera antes de mezclar el tablero.
  private IEnumerator WaitShuffle(float duration) {
    yield return new WaitForSeconds(duration);
    Shuffle();
    shuffling = false;
  }

  // Método que mezcla las piezas del tablero de manera aleatoria.
  private void Shuffle() {
    int count = 0;
    int last = 0;
    while (count < (size * size * size)) {
      // Selecciona una posición aleatoria.
      int rnd = Random.Range(0, size * size);
      // Evita deshacer el último movimiento.
      if (rnd == last) { continue; }
      last = emptyLocation;
      // Intenta mover las piezas circundantes si es un movimiento válido.
      if (SwapIfValid(rnd, -size, size)) {
        count++;
      } else if (SwapIfValid(rnd, +size, size)) {
        count++;
      } else if (SwapIfValid(rnd, -1, 0)) {
        count++;
      } else if (SwapIfValid(rnd, +1, size - 1)) {
        count++;
      }
    }
  }
}
