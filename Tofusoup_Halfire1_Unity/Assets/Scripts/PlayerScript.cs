using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	internal GameManagerScript gameManager;
	List<Poker> hand = new List<Poker>();
	List<PokerCardScript> cardObjList = new List<PokerCardScript>();
	public RectTransform playerPanel;



	public void ReGame()
	{
		hand.Clear();
		cardObjList.Clear();
		while (playerPanel.childCount > 0)
		{
			DestroyImmediate(playerPanel.GetChild(0).gameObject);
		}
	}

	public void AddCard(Poker card)
	{
		hand.Add(card);
		
		PokerCardScript pokerCardObj = Instantiate(gameManager.pokerCardPerfab, playerPanel);
		pokerCardObj.cardSprite = gameManager.GetCardSprite(card);
		cardObjList.Add(pokerCardObj);
	}

	public List<Poker> GetBestHand(List<Poker> cards)
	{
		hand.AddRange(cards);
		/*Debug.Log($"{name} GetBestHand {string.Join(", ", hand)}");
		List<List<Poker>> hand5s = Poker.SevenToFiveGroups(hand);
		foreach(var handd in hand5s)
		{
			Debug.Log($"{name} Hand5 {string.Join(", ", handd)}, rank {Poker.GetHandRank(handd)}");
		}*/
		return Poker.GetBestHand5(hand);
	}

	public void ShowHandCard()
	{
		foreach(PokerCardScript cardObj in cardObjList)
		{
			cardObj.ShowCard();
		}
	}
}
