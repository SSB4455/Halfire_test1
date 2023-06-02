using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
	List<Poker> allPockerList = new List<Poker>();



	// Start is called before the first frame update
	void Start()
	{
		string[] tags = { "♥", "♠", "♣", "♦" };
		for (int i = 0; i < 4; i++)
		{
			string tag = tags[i];
			for (int j = 0; j < 13; j++)
			{
				allPockerList.Add(new Poker(tag, j + 2));
			}
		}
		Debug.Log(string.Join(", ", allPockerList));
		RandomPoker();
		Debug.Log(string.Join(", ", allPockerList));
		
		
		List<Poker> hand1 = new List<Poker>();
		List<Poker> hand2 = new List<Poker>();

		for (int i = 0; i < 2; i++)
		{
			hand1.Add(GetCard());
			hand2.Add(GetCard());
		}
		for (int i = 0; i < 3; i++)
		{
			Poker card = GetCard();
			hand1.Add(card);
			hand2.Add(card);
		}

		hand1 = new List<Poker>
        {
            new Poker("3", 7),
            new Poker("4", 7),
            new Poker("3", 4),
            new Poker("4", 4),
            new Poker("3", 14)
        };
		hand2 = new List<Poker>
        {
            new Poker("3", 7),
            new Poker("4", 7),
            new Poker("3", 4),
            new Poker("1", 4),
            new Poker("2", 1)
        };

		List<int> hand1Pairs = hand1.Where(card => card.num == 2).Select(card => card.num).OrderBy(value => value).ToList();
		Debug.Log($"hand1Pairs: {string.Join(", ", hand1Pairs)}");
		

		Debug.Log($"Hand 1: {string.Join(", ", hand1)}, HandRank: {Poker.GetHandRank(hand1)}");
		Debug.Log($"Hand 2: {string.Join(", ", hand2)}, HandRank: {Poker.GetHandRank(hand2)}");
		Debug.Log($"Winner: {Poker.CompareHands(hand1, hand2)}");
	}

	public void RandomPoker()
	{
		int index = 0;
		Poker temp;
		for (int i = 0; i < allPockerList.Count; i++)
		{
			index = Random.Range(0, allPockerList.Count - 1);
			if (index != i)
			{
				temp = allPockerList[i];
				allPockerList[i] = allPockerList[index];
				allPockerList[index] = temp;
			}
		}
	}

	public Poker GetCard(PlayerScript player = null)
	{
		Poker card = allPockerList[0];
		allPockerList.RemoveAt(0);
		return card;
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
