using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject cardPrefab;
    public float Size = .5f;
    public float Gap = .25f;
    public Vector3 UpperLeftPos = new Vector3(0,0,0);
    public int numRows = 2;
    public int numCols = 2;
    public static GameController Instance;
    private Card firstCard;
    private Card secondCard;
    public float ShowCardsDelay = 1f;

    void Awake()
    {
        Instance = this;
    }

    void Start ()
    {
	    for (int row = 0; row < numRows; row++)
	    {
	        for (int col = 0; col < numCols; col++)
	        {
	            var horOffset = Vector3.right * col * (Size + Gap);
	            var vertOffset = Vector3.down * row * (Size + Gap);
	            var card = Instantiate(cardPrefab, UpperLeftPos + horOffset + vertOffset, Quaternion.identity).GetComponent<Card>();
                card.Init(Random.Range(1,10));
	        }
	    }
	}
	
	//void Update () {
		
	//}

    public void OnCardFlipped(Card card)
    {
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
        StartCoroutine(RemoveCardsAfterDelay());
    }

    private IEnumerator RemoveCardsAfterDelay()
    {
        var cardsToRemove = new List<Card>(){firstCard,secondCard};

        firstCard = null;
        secondCard = null;

        yield return new WaitForSeconds(ShowCardsDelay);

        foreach (var card in cardsToRemove)
        {
            Destroy(card.gameObject);
        }
    }

    private void OnCardsDifferent()
    {
        StartCoroutine(FlipCardsAfterDelay());
    }

    private IEnumerator FlipCardsAfterDelay()
    {
        List<Card> cardsToFlipBack = new List<Card>(){firstCard,secondCard};

        firstCard = null;
        secondCard = null;

        yield return new WaitForSeconds(ShowCardsDelay);

        foreach (var card in cardsToFlipBack)
        {
            card.FlipCard();
        }
    }
}
