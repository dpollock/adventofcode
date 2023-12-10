using System;
using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2023.Day07
{
    [ProblemName("Camel Cards")]
    class Solution : ISolver
    {
        public object PartOne(string input)
        {
            var lines = input.ReadLinesToType<string>().ToList();

            var hands = new List<(List<int> cards, int bet, HandType type)>();
            foreach (var line in lines)
            {
                var hand = (line.Split(" ")[0].Select(x => MapCardToValue(x)).ToList(), int.Parse(line.Split(" ")[1]),
                    HandType.HighCard);

                hands.Add((hand.Item1, hand.Item2, DetermineHandType(hand.Item1)));
            }

            hands.Sort(new HandComparator());

            var sum = 0;
            foreach (var (hand, index) in hands.Select((x, index) => (x, index + 1)))
            {
                sum += (hand.bet * index);
            }

            return sum;
        }

        public object PartTwo(string input)
        {
            var lines = input.ReadLinesToType<string>().ToList();

            var hands = new List<(List<int> cards, int bet, HandType type)>();
            foreach (var line in lines)
            {
                var hand = (line.Split(" ")[0].Select(x => MapCardToValue(x, true)).ToList(),
                    int.Parse(line.Split(" ")[1]), HandType.HighCard);

                
                hands.Add((hand.Item1, hand.Item2, DetermineHandType(hand.Item1)));
            }

            hands.Sort(new HandComparator());

            var sum = 0;
            foreach (var (hand, index) in hands.Select((x, index) => (x, index + 1)))
            {
                sum += (hand.bet * index);
            }

            return sum;
        }

       


        private static HandType DetermineHandType(List<int> cards)
        {
            var cardsCount = cards.GroupBy(c => c).Select(x => new { Card = x.Key, Appears = x.Count() });

            var handType = HandType.HighCard;
            if (cardsCount.Any(x => x.Appears == 5))
            {
                handType = HandType.FiveOfAKind;
            }
            else if (cardsCount.Any(x => x.Appears == 4))
            {
                handType = HandType.FourOfAKind;
            }
            else if (cardsCount.Any(x => x.Appears == 3) && cardsCount.Any(x => x.Appears == 2))
            {
                handType = HandType.FullHouse;
            }
            else if (cardsCount.Any(x => x.Appears == 3))
            {
                handType = HandType.ThreeOfAKind;
            }
            else if (cardsCount.Count(x => x.Appears == 2) == 2)
            {
                handType = HandType.TwoPair;
            }
            else if (cardsCount.Count(x => x.Appears == 2) == 1)
            {
                handType = HandType.OnePair;
            }

            //Joker
            var jokerCount = cardsCount.Where(x => x.Card == 1).Sum(x => x.Appears);
            if (jokerCount > 0)
            {
                handType = handType switch
                {
                    HandType.FourOfAKind => HandType.FiveOfAKind,
                    HandType.FullHouse => jokerCount == 1 ? HandType.FourOfAKind : HandType.FiveOfAKind,
                    HandType.ThreeOfAKind => HandType.FourOfAKind,
                    HandType.TwoPair => jokerCount == 1 ? HandType.FullHouse : HandType.FourOfAKind,
                    HandType.OnePair => HandType.ThreeOfAKind,
                    HandType.HighCard => HandType.OnePair,
                    _ => handType
                };
            }

            return handType;
        }

        private class HandComparator : Comparer<(List<int> cards, int bet, HandType type)>
        {
            public override int Compare((List<int> cards, int bet, HandType type) x, (List<int> cards, int bet, HandType type) y)
            {
                int handXType = (int)x!.type;
                int handYType = (int)y!.type;
                if (handXType != handYType)
                    return handYType.CompareTo(handXType);

                int num = 0;
                while (x.cards[num] == y.cards[num] && num < 4)
                {
                    num++;
                }

                return x.cards[num].CompareTo(y.cards[num]);
            }
        }


        private int MapCardToValue(char key, bool useJoker = false)
        {
            switch (key)
            {
                case 'T': return 10;
                case 'J': return useJoker ? 1 : 11;
                case 'Q': return 12;
                case 'K': return 13;
                case 'A': return 14;
                default:
                    return int.Parse(key.ToString());
            }
        }

        enum HandType
        {
            FiveOfAKind = 0,
            FourOfAKind = 1,
            FullHouse = 2,
            ThreeOfAKind = 3,
            TwoPair = 4,
            OnePair =5 ,
            HighCard = 6
        }
    }

    
}