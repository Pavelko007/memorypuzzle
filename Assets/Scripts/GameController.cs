using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class GameController : MonoBehaviour
{
    public GameObject cardPrefab;

    public float Size = .5f;
    public float Gap = .25f;

    public Vector3 UpperLeftPos = new Vector3(0,0,0);

    public int numRows = 2;
    public int numCols = 2;

    public float ShowCardsDelay = 1f;

    public static GameController Instance;
    private Card firstCard;
    private Card secondCard;


    enum State
    {
        Default,
        ShowingCards
    }

    private State state = State.Default;

    void Awake()
    {
        Instance = this;
    }

    void Start ()
    {
        var cardNumbers = CreateRandomNumberPairs();

        CreateCards(cardNumbers);
    }

    private void CreateCards(List<int> cardNumbers)
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                var horOffset = Vector3.right * col * (Size + Gap);
                var vertOffset = Vector3.down * row * (Size + Gap);
                var card = Instantiate(cardPrefab, UpperLeftPos + horOffset + vertOffset, Quaternion.identity)
                    .GetComponent<Card>();

                var linearIndex = col + row * numCols;
                Debug.Log(linearIndex);
                card.Init(cardNumbers[linearIndex]);
            }
        }
    }

    private List<int> CreateRandomNumberPairs()
    {
        List<int> cardNumbers = new List<int>();
        for (int cardNum = 1; cardNum <= numRows * numCols / 2; cardNum++)
        {
            cardNumbers.AddRange(new List<int>() {cardNum, cardNum});
        }

        var rand = new Random();
        cardNumbers = cardNumbers.OrderBy(item => rand.Next()).ToList();
        return cardNumbers;
    }

    public void OnCardFlipped(Card card)
    {
        if (state == State.ShowingCards)
        {
            CancelInvoke("HideCards");
            HideCards();
        }

        if (null == firstCard)
        {
            firstCard = card;
        }
        else if (null == secondCard)
        {
            secondCard = card;

            if (firstCard.IsSame(secondCard))
            {
                OnCardsMatch();
            }
            else
            {
                OnCardsDifferent();
            }
        }
        else
        {
            Debug.LogError("you already flipped two cards, this should be impossible");
        }
    }

    private void OnCardsMatch()
    {
        StartCoroutine(ShowCardsBeforeHide(true));
    }

    private void OnCardsDifferent()
    {
        StartCoroutine(ShowCardsBeforeHide(false));
    }

    private IEnumerator ShowCardsBeforeHide(bool remove)
    {
        List<Card> cards = new List<Card>(){firstCard,secondCard};

        firstCard = null;
        secondCard = null;

        state = State.ShowingCards; 

        Invoke("HideCards", ShowCardsDelay);

        yield return new WaitWhile(()=>State.ShowingCards == state);

        foreach (var card in cards)
        {
            if (remove)
            {
                Destroy(card.gameObject);
            }
            else
            {
                card.FlipCard();
            }
        }

        state = State.Default;
    }

    private void HideCards()
    {
        state = State.Default;
    }

    public bool CanFlipCard()
    {
        return state != State.ShowingCards;
    }
}
