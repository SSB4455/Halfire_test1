using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	public GameManagerScript gameManager;
	List<Poker> hasPockerList = new List<Poker>();



	// Start is called before the first frame update
	void Start()
	{

	}

	public void AddCard(Poker card)
	{
		hasPockerList.Add(card);
		
	}

	public void Reset()
	{
		hasPockerList.Clear();
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
