using System;
using System.IO;

namespace BlackJack_QLearning
{

    //Programmed by Nimit Johri | nimit.johri@temple.edu

    /// <summary>
    /// In the game of blackJack, there are 10 possible card values for the dealer's upcard and 21 possible sum values of the player's cards.
    /// The QTable consists of 211 rows for every combination of dealer's upcard with player's cards
    /// The QTable contains 4 colums for Hits survived | Hits attempted | Stay won | Stay attempted respectively
    /// The QTable stores the statistics for every hand combination with hits attempted to hits survived and stay attempted to stay won
    /// </summary>


    //Class containing the table to be learned
    class QTable
    {
        public int[,] Table = new int[211, 4];

        //Method to write the QTable to a file
        public void WriteQTable(int[,] Table)
        {
            using (var sw = new StreamWriter("QTable.txt"))
            {
                for (int i = 0; i < 211; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        sw.Write(Table[i, j] + " ");
                    }
                    sw.Write("\n");
                }
                

                sw.Flush();
                sw.Close();
            }
        }

        //Method to read the QTable from a file
        public int[,] ReadQTable(int[,] Table)
        {
            if (!File.Exists("QTable.txt"))
                return Table;

            String input = File.ReadAllText(@"QTable.txt");
            input = input.Substring(0, input.Length - 1);
            int i = 0, j = 0;
            foreach (var row in input.Split('\n'))
            {
                j = 0;
                foreach (var col in row.Trim().Split(' '))
                {
                    Table[i, j] = int.Parse(col.Trim());
                    j++;
                }
                i++;
            }
            return Table;
        }
    }
}
