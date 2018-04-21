using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class GameController : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject GameOverPanel;

    public int numRows = 2;
    public int numCols = 2;

    public float ShowCardsDelay = 1f;

    public static GameController Instance;

    private Card firstCard;
    private Card secondCard;

    private State state = State.Default;
    List<Card> cards = new List<Card>();

    private int numFoundPairs;

    enum State
    {
        Default,
        AllCardsPreview,
        ShowingCards
    }

    void Awake()
    {
        Instance = this;
        GameOverPanel.SetActive(false);
    }

    void Start ()
    {
        StartGameSession();
    }

    public void StartGameSession()
    {
        numFoundPairs = 0;

        var cardNumbers = CreateRandomNumberPairs();

        CreateCards(cardNumbers);
        foreach (var card in cards)
        {
            card.FaceFront();
        }
        state = State.AllCardsPreview;
        Invoke("EndCardPreview", 1f);
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W))
        {
            GameOver();
        }
#endif
    }

    private void GameOver()
    {
        GameOverPanel.SetActive(true);
    }

    void EndCardPreview()
    {
        state = State.Default;
        foreach (var card in cards)
        {
            card.FaceBack();
        }
    }

    private void CreateCards(List<int> cardNumbers)
    {
        if (cards.Count == 0)
        {
            var point1 = Camera.main.ViewportToWorldPoint(Vector3.zero);
            var point2 = Camera.main.ViewportToWorldPoint(Vector3.right);

            var screenWidthWorld = Vector3.Distance(point2, point1);

            var spacePerCard = screenWidthWorld / numCols;

            var gapSize = spacePerCard / 10;

            var cardSize = (screenWidthWorld - gapSize * (numRows + 1)) / numRows;

            var upperLeftCardPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 1)) +
                               cardSize * (Vector3.right + Vector3.down) / 2 +
                               gapSize*(Vector3.right + Vector3.down);

            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    var horOffset = Vector3.right * col * (cardSize + gapSize);

                    var vertOffset = Vector3.down * row * (cardSize + gapSize);

                    var position = upperLeftCardPos + horOffset + vertOffset;

                    var card = Instantiate(cardPrefab, position, Quaternion.identity).GetComponent<Card>();

                    card.SetSize(cardSize);
                    cards.Add(card);
                }
            }
        }
        else
        {
            foreach (var card in cards)
            {
                card.gameObject.SetActive(true);
            }
        }

        for (var i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            card.Init(cardNumbers[i]);
        }
    }

    private List<int> CreateRandomNumberPairs()
    {
        List<int> cardNumbers = new List<int>();
        for (int cardNum = 1; cardNum <= NumPairs(); cardNum++)
        {
            cardNumbers.AddRange(new List<int>() {cardNum, cardNum});
        }

        var rand = new Random();
        cardNumbers = cardNumbers.OrderBy(item => rand.Next()).ToList();
        return cardNumbers;
    }

    private int NumPairs()
    {
        return numRows * numCols / 2;
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
        numFoundPairs++;
        if(numFoundPairs == NumPairs()) GameOver();
        StartCoroutine(ShowCardsBeforeHide(true));
    }

    private void OnCardsDifferent()
    {
        StartCoroutine(ShowCardsBeforeHide(false));
    }

    private IEnumerator ShowCardsBeforeHide(bool remove)
    {
        List<Card> cardPair = new List<Card>(){firstCard,secondCard};

        firstCard = null;
        secondCard = null;

        state = State.ShowingCards; 

        Invoke("HideCards", ShowCardsDelay);

        yield return new WaitWhile(()=>State.ShowingCards == state);

        foreach (var card in cardPair)
        {
            if (remove)
            {
                card.gameObject.SetActive(false);
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
