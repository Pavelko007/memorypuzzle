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

    public enum CardOrientation
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
        SetOrientation(CardOrientation.Front);
    }

    private float GetSize()
    {
        var rectTransform = FrontFace.GetComponent<RectTransform>();
        return rectTransform.rect.width * rectTransform.localScale.x;
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

    public void SetOrientation(CardOrientation newOrientation)
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
        GameController.Instance.OnCardFlipped(this);
    }

    public bool IsSame(Card secondCard)
    {
        return Number == secondCard.Number;
    }

    public void FaceFront()
    {
        SetOrientation(Card.CardOrientation.Front);
    }

    public void FaceBack()
    {
        SetOrientation(Card.CardOrientation.Back);
    }

    public void SetSize(float newSize)
    {
        var scaleMult = newSize / GetSize();
        transform.localScale = Vector3.one * scaleMult;
    }
}
