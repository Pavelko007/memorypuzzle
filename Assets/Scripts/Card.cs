using UnityEngine;

public class Card : MonoBehaviour
{
    public GameObject FrontFace;
    public GameObject BackFace;

    private Canvas frontCanvas;
    private Canvas backCanvas;

    private CardOrientation curOrientation = CardOrientation.Front;

    enum CardOrientation
    {
        Front,
        Back
    }

    void Awake()
    {
        frontCanvas = FrontFace.GetComponent<Canvas>();
        backCanvas = BackFace.GetComponent<Canvas>();
    }

    void Start()
    {
        SetOrientation(CardOrientation.Back);
    }

    [ContextMenu("flip card")]
    public void FlipCard()
    {
        switch (curOrientation)
        {
            case CardOrientation.Front:
                SetOrientation(CardOrientation.Back);
                break;
            case CardOrientation.Back:
                SetOrientation(CardOrientation.Front);
                break;
        }
    }

    void SetOrientation(CardOrientation newOrientation)
    {
        switch (newOrientation)
        {
            case CardOrientation.Back:
                transform.rotation = Quaternion.Euler(new Vector3(180, 0, 0));
                backCanvas.sortingOrder = 1;
                frontCanvas.sortingOrder = -1;
                break;
            case CardOrientation.Front:
                transform.rotation = Quaternion.Euler(Vector3.zero);
                frontCanvas.sortingOrder = 1;
                backCanvas.sortingOrder = -1;
                break;
        }
        curOrientation = newOrientation;
    }

    public void OnBackFaceClicked()
    {
        FlipCard();
    }
}
