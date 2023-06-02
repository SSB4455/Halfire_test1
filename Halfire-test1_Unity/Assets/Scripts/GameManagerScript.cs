using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
	List<Poker> allPockerList = new List<Poker>();
	List<Poker> showCardList = new List<Poker>();
	int dealCount;
	public PokerCardScript pokerCardPerfab;
	public Sprite[] cardSprites;
	public RectTransform dealPanel;
	public Button dealButton;
	public Button compareButton;
	public Text resultText;
	PlayerScript[] players;




	// Start is called before the first frame update
	void Start()
	{
		players = GameObject.FindObjectsOfType<PlayerScript>();
		for (int i = 0; i < players.Length; i++)
		{
			players[i].gameManager = this;
		}

		ReGame();

		/*List<Poker> hand1 = new List<Poker>
		{
			new Poker("3", 13),
			new Poker("4", 11),
			new Poker("3", 2),
			new Poker("4", 2),
			new Poker("3", 14)
		};
		List<Poker> hand2 = new List<Poker>
		{
			new Poker("3", 4),
			new Poker("4", 9),
			new Poker("3", 2),
			new Poker("1", 2),
			new Poker("2", 14)
		};
		List<Poker> hand3 = new List<Poker>
		{
			new Poker("3", 14),
			new Poker("4", 14),
			new Poker("3", 6),
			new Poker("1", 8),
			new Poker("2", 10),
			new Poker("1", 12),
			new Poker("2", 9)
		};
		Debug.Log($"Hand 1: {string.Join(", ", hand1)}, HandRank: {Poker.GetHandRank(hand1)}");
		Debug.Log($"Hand 2: {string.Join(", ", hand2)}, HandRank: {Poker.GetHandRank(hand2)}");
		Debug.Log($"Hand 3: {string.Join(", ", hand3)}, BestHand5: {string.Join(", ", Poker.GetBestHand5(hand3))}");
		Debug.Log($"Winner: {Poker.CompareHands(hand1, hand2)}");
		*/
	}

	public void ReGame()
	{
		resultText.text = "";
		dealButton.interactable = true;
		compareButton.interactable = false;
		Text buttonText = compareButton.GetComponentInChildren<Text>();
		buttonText.text = "COMPARE";
		compareButton.onClick.RemoveAllListeners();
		compareButton.onClick.AddListener(CompareButton);
		dealCount = 0;
		allPockerList.Clear();
		showCardList.Clear();
		string[] tags = { "♥", "♠", "♣", "♦" };
		for (int i = 0; i < 4; i++)
		{
			string tag = tags[i];
			for (int j = 0; j < 13; j++)
			{
				allPockerList.Add(new Poker(tag, j + 2));
			}
		}
		//Debug.Log(string.Join(", ", allPockerList));
		RandomPoker();
		//Debug.Log(string.Join(", ", allPockerList));

		for (int i = 0; i < players.Length; i++)
		{
			players[i].ReGame();
			players[i].AddCard(GetCard());
			players[i].AddCard(GetCard());
		}
		
		while (dealPanel.childCount > 0)
		{
			DestroyImmediate(dealPanel.GetChild(0).gameObject);
		}
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

	public Poker GetCard()
	{
		Poker card = allPockerList[0];
		allPockerList.RemoveAt(0);
		return card;
	}

	public Sprite GetCardSprite(Poker card)
	{
		int index = 0;
		switch (card.tag)
		{
			case "♥":
			index = 0 * 13;
			break;
			case "♠":
			index = 1 * 13;
			break;
			case "♣":
			index = 2 * 13;
			break;
			case "♦":
			index = 3 * 13;
			break;
		}
		index += (card.num - 1) % 13;
		return cardSprites[index];
	}

	public void DealButton()
	{
		int loopCount = dealCount == 0 ? 3 : 1;
		for (int i = 0; i < loopCount; i++)
		{
			Poker card = GetCard();
			PokerCardScript pokerCardObj = Instantiate(pokerCardPerfab, dealPanel);
			pokerCardObj.cardSprite = GetCardSprite(card);
			pokerCardObj.ShowCard();
			showCardList.Add(card);
			if (showCardList.Count > 4)
			{
				dealButton.interactable = false;
				compareButton.interactable = true;
			}
			Debug.Log($"GetCard {card}\tshowCardList.Count {showCardList.Count}");
		}
		dealCount++;
	}

	public void CompareButton()
	{
		List<List<Poker>> playerHands = new List<List<Poker>>();
		for (int i = 0; i < players.Length; i++)
		{
			players[i].ShowHandCard();
			List<Poker> hand = players[i].GetBestHand(showCardList);
			playerHands.Add(hand);
			Debug.Log($"{players[i].name}'s best Hand: {string.Join(", ", hand)}, HandRank: {Poker.GetHandRank(hand)}");
		}

		int result = Poker.CompareHands(playerHands[0], playerHands[1]);

		resultText.text = result != 0 ? (result > 0 ? $"{players[1].name} Win" : $"{players[0].name} Win") : "平局";

		Text buttonText = compareButton.GetComponentInChildren<Text>();
		buttonText.text = "NEW";
		compareButton.onClick.RemoveAllListeners();
		compareButton.onClick.AddListener(ReGame);
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
