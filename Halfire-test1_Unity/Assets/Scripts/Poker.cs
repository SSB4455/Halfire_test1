using System;
using System.Collections.Generic;
using System.Linq;

public class Poker
{
	public string tag { get; private set; }
	public int num { get; private set; }



	public Poker(string tag, int num)
	{
		this.tag = tag;
		this.num = num;
	}
	
	public override string ToString()
	{
		string nStr = num.ToString();
		switch (num)
		{
			case 11: nStr = "J"; break;
			case 12: nStr = "Q"; break;
			case 13: nStr = "K"; break;
			case 14: nStr = "A"; break;
		}
		return tag + nStr;
	}
	

	public static List<List<Poker>> SevenToFiveGroups(List<Poker> map)
	{
		List<List<Poker>> hand5List = new List<List<Poker>>();
		for (int a = 0; a < 3; a++)
		{
			for (int b = a + 1; b < 4; b++)
			{
				for (int c = b + 1; c < 5; c++)
				{
					for (int d = c + 1; d < 6; d++)
					{
						for (int e = d + 1; e < 7; e++)
						{
							List<Poker> hand = new List<Poker>{ map[a], map[b], map[c], map[d], map[e] };
							hand5List.Add(hand);
						}
					}
				}
			}
		}
		return hand5List;
	}

	public static List<Poker> GetBestHand5(List<Poker> hand7)
	{
		List<List<Poker>> hand5List = SevenToFiveGroups(hand7);
		List<Poker> bestHand = hand5List[0];
		foreach(List<Poker> hand in hand5List)
		{
			if (CompareHands(bestHand, hand) > 0)
			{
				bestHand = hand;
			}
		}
		return bestHand;
	}
	

	public enum HandRank
	{
		HighCard,
		OnePair,
		TwoPairs,
		ThreeOfAKind,
		Straight,
		Flush,
		FullHouse,
		FourOfAKind,
		StraightFlush,
		RoyalFlush
	}

	public static int CompareHands(List<Poker> hand1, List<Poker> hand2)
	{
		List<Poker> tempHand1 = new List<Poker>(hand1);
		List<Poker> tempHand2 = new List<Poker>(hand2);
		var rank1 = GetHandRank(tempHand1);
		var rank2 = GetHandRank(tempHand2);

		if (rank1 > rank2)
		{
			return -1;
		}
		else if (rank2 > rank1)
		{
			return 1;
		}
		else
		{
			// 比较最大的单牌
			int maxCard1 = GetMaxCardValue(tempHand1);
			int maxCard2 = GetMaxCardValue(tempHand2);
			switch (rank1)
			{
			case HandRank.OnePair:
				maxCard1 = GetCardValueCounts(tempHand1).FirstOrDefault(pair => pair.Value == 2).Key;
				maxCard2 = GetCardValueCounts(tempHand2).FirstOrDefault(pair => pair.Value == 2).Key;
				if (maxCard1 > maxCard2)
				{
					return -1;
				}
				else if (maxCard2 > maxCard1)
				{
					return 1;
				}
				tempHand1.RemoveAll(card => card.num == maxCard1);
				tempHand2.RemoveAll(card => card.num == maxCard2);
				break;
			case HandRank.TwoPairs:
				List<int> tempHand1Pairs = GetCardValueCounts(tempHand1).Where(card => card.Value == 2).Select(card => card.Key).OrderBy(value => value).ToList();
				List<int> tempHand2Pairs = GetCardValueCounts(tempHand2).Where(card => card.Value == 2).Select(card => card.Key).OrderBy(value => value).ToList();
				for (int i = 0; i < 2; i++)
				{
					maxCard1 = tempHand1Pairs[i];
					maxCard2 = tempHand2Pairs[i];
					if (maxCard1 > maxCard2)
					{
						return -1;
					}
					else if (maxCard2 > maxCard1)
					{
						return 1;
					}
				}
				tempHand1.RemoveAll(card => tempHand1Pairs.Contains(card.num));
				tempHand2.RemoveAll(card => tempHand2Pairs.Contains(card.num));
				break;
			case HandRank.ThreeOfAKind:
				maxCard1 = GetCardValueCounts(tempHand1).FirstOrDefault(pair => pair.Value == 3).Key;
				maxCard2 = GetCardValueCounts(tempHand2).FirstOrDefault(pair => pair.Value == 3).Key;
				if (maxCard1 > maxCard2)
				{
					return -1;
				}
				else if (maxCard2 > maxCard1)
				{
					return 1;
				}
				tempHand1.RemoveAll(card => card.num == maxCard1);
				tempHand2.RemoveAll(card => card.num == maxCard2);
				break;
			case HandRank.Straight:
				break;
			case HandRank.Flush:
				break;
			case HandRank.FullHouse:
				maxCard1 = GetCardValueCounts(tempHand1).FirstOrDefault(pair => pair.Value == 3).Key;
				maxCard2 = GetCardValueCounts(tempHand2).FirstOrDefault(pair => pair.Value == 3).Key;
				if (maxCard1 > maxCard2)
				{
					return -1;
				}
				else if (maxCard2 > maxCard1)
				{
					return 1;
				}
				tempHand1.RemoveAll(card => card.num == maxCard1);
				tempHand2.RemoveAll(card => card.num == maxCard2);
				maxCard1 = GetCardValueCounts(tempHand1).FirstOrDefault(pair => pair.Value == 2).Key;
				maxCard2 = GetCardValueCounts(tempHand2).FirstOrDefault(pair => pair.Value == 2).Key;
				if (maxCard1 > maxCard2)
				{
					return -1;
				}
				else if (maxCard2 > maxCard1)
				{
					return 1;
				}
				tempHand1.RemoveAll(card => card.num == maxCard1);
				tempHand2.RemoveAll(card => card.num == maxCard2);
				break;
			case HandRank.FourOfAKind:
				maxCard1 = GetCardValueCounts(tempHand1).FirstOrDefault(pair => pair.Value == 4).Key;
				maxCard2 = GetCardValueCounts(tempHand2).FirstOrDefault(pair => pair.Value == 4).Key;
				if (maxCard1 > maxCard2)
				{
					return -1;
				}
				else if (maxCard2 > maxCard1)
				{
					return 1;
				}
				tempHand1.RemoveAll(card => card.num == maxCard1);
				tempHand2.RemoveAll(card => card.num == maxCard2);
				break;
			case HandRank.StraightFlush:
				if (tempHand1.Any(card => card.num == 14))
				{
					return 1;
				}
				else if (tempHand2.Any(card => card.num == 14))
				{
					return -1;
				}
				break;
			}

			var maxCard1s = tempHand1.Select(card => card.num).OrderByDescending(value => value).ToList();
			var maxCard2s = tempHand2.Select(card => card.num).OrderByDescending(value => value).ToList();
			for (int i = 0; i < maxCard1s.Count && i < maxCard2s.Count; i++)
			{
				if (maxCard1s[i] > maxCard2s[i])
				{
					return -1;
				}
				else if (maxCard2s[i] > maxCard1s[i])
				{
					return 1;
				}
			}
			return 0; // 平局
		}
	}

	public static HandRank GetHandRank(List<Poker> hand)
	{
		if (IsRoyalFlush(hand))
		{
			return HandRank.RoyalFlush;
		}
		else if (IsStraightFlush(hand))
		{
			return HandRank.StraightFlush;
		}
		else if (IsFourOfAKind(hand))
		{
			return HandRank.FourOfAKind;
		}
		else if (IsFullHouse(hand))
		{
			return HandRank.FullHouse;
		}
		else if (IsFlush(hand))
		{
			return HandRank.Flush;
		}
		else if (IsStraight(hand))
		{
			return HandRank.Straight;
		}
		else if (IsThreeOfAKind(hand))
		{
			return HandRank.ThreeOfAKind;
		}
		else if (IsTwoPairs(hand))
		{
			return HandRank.TwoPairs;
		}
		else if (IsOnePair(hand))
		{
			return HandRank.OnePair;
		}
		else
		{
			return HandRank.HighCard;
		}
	}

	private static int GetMaxCardValue(List<Poker> hand)
	{
		var values = hand.Select(card => card.num).OrderByDescending(value => value);
		
		//Debug.Log($"GetMaxCardValue Hand: {string.Join(", ", hand)}, MaxCardValue: {values.First()}");
		return values.First();
	}

	// 检查是否是皇家同花顺
	private static bool IsRoyalFlush(List<Poker> hand)
	{
		return IsFlush(hand) && hand.Sum(card => card.num) == 60;
	}

	// 检查是否是同花顺
	private static bool IsStraightFlush(List<Poker> hand)
	{
		return IsFlush(hand) && IsStraight(hand);
	}

	// 检查是否是四条
	private static bool IsFourOfAKind(List<Poker> hand)
	{
		var valueCounts = GetCardValueCounts(hand);
		return valueCounts.ContainsValue(4);
	}

	// 检查是否是葫芦
	private static bool IsFullHouse(List<Poker> hand)
	{
		var valueCounts = GetCardValueCounts(hand);
		return valueCounts.ContainsValue(3) && valueCounts.ContainsValue(2);
	}

	// 检查是否是同花
	private static bool IsFlush(List<Poker> hand)
	{
		for (int i = 1; i < hand.Count; i++)
		{
			if (hand[i].tag != hand[i - 1].tag)
			{
				return false;
			}
		}
		return true;
	}

	// 检查是否是顺子
	private static bool IsStraight(List<Poker> hand)
	{
		var values = hand.Select(card => card.num).OrderBy(value => value).ToList();
		if (values.Contains(14))
		{
			values.Remove(14);
			values.Insert(0, 1);
		}

		for (var i = 1; i < values.Count; i++)
		{
			if (values[i] - 1 != values[i - 1])
			{
				return false;
			}
		}

		return true;
	}

	// 检查是否是三条
	private static bool IsThreeOfAKind(List<Poker> hand)
	{
		var valueCounts = GetCardValueCounts(hand);
		return valueCounts.ContainsValue(3);
	}

	// 检查是否是两对
	private static bool IsTwoPairs(List<Poker> hand)
	{
		var valueCounts = GetCardValueCounts(hand).Values;
		return valueCounts.Count(value => value == 2) == 2;
	}

	// 检查是否是一对
	private static bool IsOnePair(List<Poker> hand)
	{
		var valueCounts = GetCardValueCounts(hand).Values;
		return valueCounts.Count(value => value == 2) == 1;
	}

	private static Dictionary<int, int> GetCardValueCounts(List<Poker> hand)
	{
		Dictionary<int, int> valueCounts = new Dictionary<int, int>();
		foreach (Poker card in hand)
		{
			if (valueCounts.ContainsKey(card.num))
			{
				valueCounts[card.num]++;
			}
			else
			{
				valueCounts[card.num] = 1;
			}
		}

		return valueCounts;
	}
}