using UnityEngine;

public class PuzzlePieceManager : MonoBehaviour
{
    public static int puzzlePieces;
    public int startingPieces;

    private void Start()
    {
        puzzlePieces += startingPieces;
    }

}
