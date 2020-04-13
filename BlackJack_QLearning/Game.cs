using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack_QLearning
{
    //Programmed by Nimit Johri | nimit.johri@temple.edu

    /// <summary>
    /// The QTable while being trained, the application first tries to stay for whatever had is dealt to the player and checks if it won
    /// WHile training it also hits for the same hand and checks if it survived the hit
    /// If the program survived the previous hit then again it will try to stay and again hit
    /// The cycle continues untill player is bust, learning on which hand hit could have been survived and on which hand stay is the best policy
    /// 
    /// While playing, the application compares the ration of hits survived and hits attempted with stay won and stay attempted for that particular hand
    /// If the ratio for his is higher the application hits and the cycle restars and if the ratio for stay is higher then the application stays at that hand
    /// With significant number of runs, when the QTable is analyzed, we can that the QTable has learned to always hit when the sum of hand is around 3 or 4 
    /// And always stay when the sum of hands is 21
    /// </summary>


    //Class containing the components of the game
    class Game
    {
        public List<int> deck;
        public List<int> playercards;
        public List<int> dealercards;

        public int played = 0;
        public int win = 0;
        public int loss = 0;

        public Game()
        {
            this.deck = new List<int>();
            this.playercards = new List<int>();
            this.dealercards = new List<int>();
            ResetDeck();
        }

        //Method to check if the deck has atleast a minimum number of cards left
        private void CheckDeck()
        {
            if (deck.Count < 7)
                ResetDeck();
        }

        //Method to reset the deck to 52 cards
        private void ResetDeck()
        {
            this.deck.Clear();

            for (int j = 0; j < 4; j++)
            {
                Parallel.For(1, 14, i =>
                {
                    if (i > 10)
                        this.deck.Add(10);
                    else
                        this.deck.Add(i);
                });
            }
        }

        //Method to deal cards for a single simulation of a game
        private void DealCards()
        {
            int card;
            Random rnd = new Random();

            for (int i = 0; i < 2; i++)
            {
                card = this.deck[rnd.Next(this.deck.Count)];
                this.playercards.Add(card);
                this.deck.Remove(card);

                card = this.deck[rnd.Next(this.deck.Count)];
                this.dealercards.Add(card);
                this.deck.Remove(card);
            }
        }

        //Method to find the sum of a hand
        private int Sum(List<int> cards)
        {
            List<int> count = new List<int>();

            foreach (int card in cards)
            {
                if (card == 1)
                {
                    if (cards.Sum() <= 11)      //If there is an ace in the hand then it is counted as 10 if sum is low else as 1
                    {
                        count.Add(10);
                    }
                    else
                        count.Add(1);
                }
                else
                    count.Add(card);
            }

            return count.Sum();
        }

        //Method to check if the hand is a bust
        private bool IsBust(List<int> cards)
        {
            List<int> count = new List<int>();

            foreach (int card in cards)
            {
                if (card == 1)
                {
                    if (cards.Sum() <= 11)
                    {
                        count.Add(10);
                    }
                    else
                        count.Add(1);
                }
                else
                    count.Add(card);
            }
        
            if (Sum(count) > 21)
                return true;
            else
                return false;
        }

        //Method to hit and draw a card and check if this new card lead to a bust and store the result in the QTable
        private bool Hit(int[,] qTable)
        {
            int PrevSum = Sum(playercards);
            bool bust; 
            int card;
            Random rnd = new Random();

            CheckDeck();
            card = this.deck[rnd.Next(this.deck.Count)];

            this.playercards.Add(card);
            this.deck.Remove(card);

            bust = IsBust(this.playercards);

            if (bust)
            {
                qTable[((dealercards[0] -1 ) * 20) + PrevSum, 1]++;     //If hit lead to bust then only increment the attempts counter for a particular hand
            }
            else
            {
                qTable[((dealercards[0] - 1) * 20) + PrevSum, 0]++;     //if not bust then increment both attempted and survived counters for a particular hand
                qTable[((dealercards[0] - 1) * 20) + PrevSum, 1]++;
            }

            return bust;
        }

        //Method to check if we won the game by staying
        private void Stay(int[,] qTable)
        {
            bool BlackJack = true;

            int PrevSum = Sum(playercards);

            if (Sum(playercards) != 21)
            {
                BlackJack = false;

                while (!IsBust(dealercards) && Sum(dealercards) <= PrevSum)
                {
                    DealerHit();                                                    //Dealer hits until it is either bust or it beats the player
                }
            }

            if (!BlackJack && !IsBust(dealercards) && Sum(dealercards) > PrevSum)
            {
                qTable[((dealercards[0] - 1) * 20) + PrevSum, 3]++;                 //If the player lost by staying, only increment attempted counter
                played++;
                loss++;
            }
            else
            {
                qTable[((dealercards[0] - 1) * 20) + PrevSum, 2]++;                 //If the player won by staying, increment both attempted and won counters
                qTable[((dealercards[0] - 1) * 20) + PrevSum, 3]++;
                played++;
                win++;
            }
        }

        //Method to simulate the dealer hitting on a hand
        private void DealerHit()
        {
            CheckDeck();    

            int card;
            Random rnd = new Random();

            card = this.deck[rnd.Next(this.deck.Count)];                            //Draw random card from cards left in the deck

            this.dealercards.Add(card);                                             //Add drawn card to dealer's hand
            this.deck.Remove(card);                                                 //Remove the drawn card from deck

        }

        //Method to train the QTable with the outcome of both staying and hitting for each hand
        public void Train(int[,] qTable)
        {
            if (this.deck.Count < 7)
                ResetDeck();

            playercards.Clear();                                                    //Clear cards from previous game
            dealercards.Clear();

            DealCards();                                                            //Deal cards for a new game

            while (!IsBust(playercards))
            {
                Stay(qTable);                                                       //Learn if we win by staying
                Hit(qTable);                                                        //Learn if we survive by hitting
            }
        }

        //Method to reset counters
        public void prepareToPlay()
        {
            this.played = 0;
            this.loss = 0;
            this.win = 0;
        }

        //Method for playing the game with the rules learned and stored in QTable
        public void Play(int[,] qTable)
        {
            if (this.deck.Count < 7)
                ResetDeck();

            playercards.Clear();
            dealercards.Clear();

            DealCards();

            while (!IsBust(playercards))                                            //For a hand the player decides whether to hit or stay according to the previous experience stored in QTable
            {
                if (qTable[((dealercards[0] - 1) * 20) + Sum(playercards), 1] == 0 || (Convert.ToDouble(qTable[((dealercards[0] - 1) * 20) + Sum(playercards), 0]) / Convert.ToDouble(qTable[((dealercards[0] - 1) * 20) + Sum(playercards), 1])) >= (Convert.ToDouble(qTable[((dealercards[0] - 1) * 20) + Sum(playercards), 2]) / Convert.ToDouble(qTable[((dealercards[0] - 1) * 20) + Sum(playercards), 3])))
                {
                    Hit(qTable);

                    //PrintGame();

                    if(IsBust(playercards))                                         //If player got bust by hitting, increment attempted and loss counters
                    {
                        played++;
                        loss++;
                    }
                }
                else
                {
                    Stay(qTable);                                                   //After hitting a few times or in the first attempt itself the player may decide to stay according to the previous experience
                    //PrintGame();
                    break;
                }
            }
        }

        //Method to print a game played if desired
        private void PrintGame()
        {
            Console.Clear();

            string cards = "";
            Console.WriteLine("The dealer has");
            foreach (int card in dealercards)
                cards = cards + "\t" + card;
            Console.WriteLine(cards);

            cards = "";
            Console.WriteLine("The player has");
            foreach (int card in playercards)
                cards = cards + "\t" + card;
            Console.WriteLine(cards);
        }
    }
}
