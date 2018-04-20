using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public GameObject FrontFace;
    public GameObject BackFace;
    public Text CardText;

    private Canvas frontCanvas;
    private Canvas backCanvas;

    private CardOrientation curOrientation = CardOrientation.Front;
    private int number;

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

    public void Init(int newNumber)
    {
        Number = newNumber;
    }

    public int Number
    {
        get { return number; }
        set
        {
            number = value;
            CardText.text = number.ToString();
        }
    }

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
        if (!GameController.Instance.CanFlipCard()) return;

        FlipCard();
        GameController.Instance.OnCardFlipped(this);
    }

    public bool IsSame(Card secondCard)
    {
        return Number == secondCard.Number;
    }
}
