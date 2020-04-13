using System;

namespace BlackJack_QLearning
{

    //Programmed by Nimit Johri | nimit.johri@temple.edu

    /// <summary>
    /// The main() loads any previously learned QTable and trains for the given number of games first
    /// Then the games are played based on what was learned while training or other previous games
    /// Once the QTable is learned, the training part can be skipped for each run
    /// </summary>


    //Class containing the controlling main function
    class Program
    {
        static void Main(string[] args)
        {
            QTable qTable = new QTable();

            //Read the previouly learned QTable
            qTable.Table = qTable.ReadQTable(qTable.Table);

            //Train the QTable by exploring the outcome of both hit and stay for each hand
            Game game = new Game();
            for (int i = 0; i < 1000000; i++)
            {
                game.Train(qTable.Table);
            }

            //Reset the counters to play according the the learned QTable 
            game.prepareToPlay();

            //Play according to the learned rules in QTable
            for (int i = 0; i < 1000000; i++)
            {
                game.Play(qTable.Table);
            }

            //Update the QTable with the learnings from this session, implementing reinforced learning
            qTable.WriteQTable(qTable.Table);

            Console.WriteLine("Played:\t" +game.played+ "\tWin:\t" +game.win+ "\tloss:\t" + game.loss + "\tWin Percentage:\t" +(Convert.ToDouble(game.win)/Convert.ToDouble(game.played))*100);

            Console.ReadLine();
        }

        //Method to print the QTable if desired
        private static void Print(int[,] table)
        {
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    Console.Write(table[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }
    }
}
