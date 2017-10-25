using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;


namespace BowlingGame
{
	public class Game
	{
		private Dictionary<int, Tuple<int, int>> frames = new Dictionary<int, Tuple<int, int>>();
		private int currentFrame;
		public int CurrentFrame
		{
			get { return currentFrame; }
			set
			{
				if (value > MaxFrames)
					throw new Exception($"Only {MaxFrames} Frames!");
				currentFrame = value;
			}
		}

		private int MaxFrames = 10;

		private int stepInFrame;
		public int StepInFrame
		{
		 get { return stepInFrame; }
			set
			{
				if (value >= 2)
					stepInFrame = 0;
				else
					stepInFrame = 1;
			}
		}

		public void Roll(int pins)
		{
			
			if (StepInFrame == 1)
			{
				var previousPins = frames[currentFrame].Item1;
				if (CurrentFrame != 9 && pins + previousPins > 10)
					throw new Exception("Only 10 Pins!");
				else
				{
					MaxFrames += 1;
				}
				frames[currentFrame] = Tuple.Create(previousPins, pins);
				CurrentFrame += 1;
			}
			else
			{
				if (CurrentFrame == 10)
				{
					if ((frames[9].Item1 + frames[9].Item2) % 10 != 0)
						throw new Exception();
				}

				frames[currentFrame] = Tuple.Create(pins, 0);

				if (pins == 10)
				{
					if (CurrentFrame != 9)
					{
						CurrentFrame += 1;
						StepInFrame += 1;
					}

				}
			}
			StepInFrame += 1;
		}

		public int GetScore()
		{
			int fromSpares = 0;
			int prevSum = 0;
			
			for (var i = CurrentFrame-1; i >= 0; i--)
			{
				
				var frame = frames[i];
				var currentSum = frame.Item1 + frame.Item2;

				if (frame.Item1 == 10)
				{
					frames[i] = Tuple.Create(10, prevSum);
					currentSum = 10 + prevSum;
				}
				else if (currentSum == 10)
				{
					var toAdd = frames[i + 1].Item1;
					if (CurrentFrame == 10)
						toAdd = 0;
					frames[i] = Tuple.Create(frame.Item1, frame.Item2 + toAdd);
				}
				prevSum = currentSum;
			}
			return frames.Values.Sum(x => x.Item1 + x.Item2);
		}
	}


	[TestFixture]
	public class Game_should : ReportingTest<Game_should>
	{
		// ReSharper disable once UnusedMember.Global
		public static string Names = "12 Zhukova Starcev"; // Ivanov Petrov

		public Game game;
		[SetUp]
		public void SetUp()
		{
			game = new Game();
		}

		[Test]
		public void HaveZeroScore_BeforeAnyRolls()
		{
			game.GetScore()
				.Should().Be(0);
		}

		[Test]
		public void GetScore_When_HitOnePin()
		{
			game.Roll(1);
			game.GetScore().Should().Be(1);
		}

		[Test]
		public void GetScore_When_TwentyRollsWithOneHit()
		{
			RollInCycle(20);
			game.GetScore().Should().Be(20);
		}

		[Test]
		public void ThrowException_WhenMoreThanTenFrames()
		{
			Assert.Throws<Exception>(() => RollInCycle(21));
		}

		[Test]
		public void SkipFrame_IfStrike()
		{
			game.CurrentFrame.Should().Be(0);
			game.Roll(10);
			game.CurrentFrame.Should().Be(1);
			game.StepInFrame.Should().Be(0);
		}


		private void RollInCycle(int num)
		{
			for (int i = 0; i < num; i++)
			{
				game.Roll(1);
			}
		}
		 
		[Test]
		public void ThrowException_When_MoreThanTenPins()
		{
			game.Roll(1);
			Assert.Throws<Exception>(() => game.Roll(10));
		}

		[Test]
		public void GetScores_When_PreviousStrike()
		{
			game.Roll(10);
			game.Roll(1);
			game.Roll(2);
			game.GetScore().Should().Be(16);
		}

		[Test]
		public void GetScore_When_TwoStrikes_InRow()
		{
			game.Roll(10);
			game.Roll(10);
			game.Roll(3);
			game.Roll(2);
			game.GetScore().Should().Be(45);

		}

		[Test]
		public void GetScore_When_Spare()
		{
			game.Roll(5);
			game.Roll(5);
			game.Roll(2);
			game.GetScore().Should().Be(14);
		}

		[Test]
		public void GetScore_When_TwoSpareInRow()
		{
			game.Roll(5);
			game.Roll(5);
			game.Roll(6);
			game.Roll(4);
			game.Roll(2);
			game.GetScore().Should().Be(30);
		}

		[Test]
		public void GetScore_When_StrikeAndSpareInRow()
		{
			game.Roll(10);
			game.Roll(6);
			game.Roll(4);
			game.Roll(2);
			game.GetScore().Should().Be(34);
		}

		[Test]
		public void GetScore_When_ThreeStrikesOnLastFrame()
		{
			RollInCycle(18);
			game.Roll(10);
			game.Roll(10);
			game.Roll(10);
			game.GetScore().Should().Be(48);
		}
		[Test]
		public void GetScore_When_SpareOnLastFrame()
		{
			RollInCycle(18);
			game.Roll(5);
			game.Roll(5);
			game.Roll(2);
			game.GetScore().Should().Be(30);
		}
		[Test]
		public void NoThirdTry_IfNoSpareStrike()
		{
			RollInCycle(18);
			game.Roll(5);
			game.Roll(2);
			Assert.Throws<Exception>(() => game.Roll(1));
		}

		[Test]
		public void TwelveStrikes()
		{
			for (var i=0;i<12;i++)
				game.Roll(10);
			game.GetScore().Should().Be(660);
		}


	}
}
