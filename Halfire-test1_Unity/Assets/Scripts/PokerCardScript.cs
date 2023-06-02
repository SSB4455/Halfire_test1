using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerCardScript : MonoBehaviour
{
	public Image pokerImage;
	public Sprite cardSprite;
	public Sprite cardBack;



	public void ShowCard()
	{
		pokerImage.sprite = cardSprite;
	}
}
