using System;
using System.Collections.Generic;

namespace Monke
{
	class MainClass
	{

		public static void Main (string[] args)
		{
			Monke monke = new Monke();
			monke.print ();
		}
	}

	class Monke
	{
		private static int seed = Guid.NewGuid ().GetHashCode ();
		private Random rand = new Random(seed);

		private int[,] rows = new int[3, 2]{{00, 01}, {00, 01}, {00, 31}};
		private Dictionary<int, string> mapping = new Dictionary<int, string> ()
		{
			{00, "|| "},
			{01, " ||"},

			{10, "||%"},
			{11, "%||"},

			{20, "||)"},
			{21, "(||"},

			{30, "||&"},
			{31, "&||"},
		};
		public int score = 0;

		private List<int> leftSide = new List<int>(){00, 10, 20};
		private List<int> rightSide = new List<int>(){01, 21};

		public Monke ()
		{
			// The init.
		}

		public void print()
		{
			string display = "";
			for (int i = 0; i < rows.GetLength(0); i++) 
			{
				display += mapping [rows [i, 0]] + mapping [rows [i, 1]];
				if (i < 2) {
					display += "\n";
				}

			}
			// Setting the cursor back where is started allows
			// overwritting the previous print
			Console.SetCursorPosition(0, 0);
			Console.Write("{0}", display);
			Console.Write("  Score: {0}", score);
			WaitForInput ();
		}

		private void WaitForInput ()
		{
			// Needed to get arrow key inputs
			ConsoleKeyInfo kb = Console.ReadKey();
			if (kb.Key == ConsoleKey.LeftArrow)
				ProcessInput (0);
			if (kb.Key == ConsoleKey.RightArrow)
				ProcessInput (1);
		}

		private void ProcessInput (int dir){
			// Holds a copy of the original data.
			int[,] oldRows = new int[3, 2]; 
			Array.Copy (rows, oldRows, rows.Length);

			for (int i = 0; i < rows.GetLength(0); i ++) {
				if (i == 0){
					// Randomly add an element. Make sure 2 bees aren't together.
					// Left
					int leftChoice = rand.Next (leftSide.Count);
					rows [i, 0] = leftSide[leftChoice];

					// A bee was added on this row. 
					// Removing it from the right side options.
					if (leftSide[leftChoice] != 10){
						rightSide.Add (11);
					}

					// Right
					int rightChoice = rand.Next (rightSide.Count);
					rows [i, 1] = rightSide[rightChoice];
					if (rightSide.Contains (11)){
						rightSide.RemoveAt (rightSide.Count - 1);
					}

				}else if (i == 1) {
					// Copy the previous row downward.
					rows [i, 0] = oldRows[i - 1, 0];
					rows [i, 1] = oldRows[i - 1, 1];
				} else if (i == 2){
					// Move the monkey to the selected side and 
					// deal with anything it collides with.
					if (dir == 0) {
						// Bees
						if (oldRows [i - 1, 0] == 10) {
							EndGame ();
						// Bananas
						} else if (oldRows [i - 1, 0] == 20){
							score += 1;
						}
						rows [i, 0] = 30;
						rows [i, 1] = oldRows[i - 1, 1];
					} else {
						// Bees
						if (oldRows [i - 1, 1] == 11) {
							EndGame();
						// Bananas
						} else if (oldRows [i - 1, 1] == 21){
							score += 1;
						}
						rows [i, 0] = oldRows[i - 1, 0];
						rows [i, 1] = 31;
					}
				}
			}
			print ();
		}

		private void EndGame()
		{
			Console.WriteLine("\n\nGame Over");
			System.Environment.Exit(1);
		}

	}
}
