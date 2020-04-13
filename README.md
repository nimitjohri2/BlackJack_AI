# BlackJack_AI
Repository for the self learning, reinforced Blackjack AI

This repository contains an AI project which implements Q-Learning from the reinforced learning umbrella. The project was developed in C#
and truly uses the power of object orientation. 

The project has three major classes 
  
  QTalble class which learns and saves the QTbale which the AI learns while playing games
  
  Game class which contains the entire logic of playing a game of Blackjack, which our AI doesn't know anything about.
  
  Program class which is the controlling class and dictates how many games are to palyed for training and then tessting.
  
The AI in this project doesn't knows anything about the game of blackjack and starts by making random moves at first. For a few initial games, the AI would even hit at 21 or stay at 9. Gradually it learns how to play the game and what moves to make by reward for good moves and punishment for poor ones.

After 10K iterations of the game, the AI was able to win approximately 44% of the games and was making the correct moves 90% of the times.

Future scope for this project is to give it a personality where we can train a number of different types Blackjack players, like risky, conservative, All-In or casual players.  

