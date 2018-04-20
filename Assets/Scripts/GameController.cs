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
                Destroy(firstCard.gameObject);
                Destroy(secondCard.gameObject);
            }
            else
            {
                OnCardsDifferent();
            }

        }
        else
        {
            OnCardsDifferent();
        }
    }

    private void OnCardsDifferent()
    {
        firstCard.FlipCard();
        secondCard.FlipCard();
        firstCard = null;
        secondCard = null;
    }
}
