using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject cardPrefab;
    public float Size = .5f;
    public float Gap = .25f;
    public Vector3 UpperLeftPos = new Vector3(0,0,0);
    public int numRows = 2;
    public int numCols = 2;

    void Start ()
    {
	    for (int row = 0; row < numRows; row++)
	    {
	        for (int col = 0; col < numCols; col++)
	        {
	            var horOffset = Vector3.right * col * (Size + Gap);
	            var vertOffset = Vector3.down * row * (Size + Gap);
	            Instantiate(cardPrefab, UpperLeftPos + horOffset + vertOffset, Quaternion.identity);
	        }
	    }
	}
	
	void Update () {
		
	}
}
