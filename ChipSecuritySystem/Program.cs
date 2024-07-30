using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ChipSecuritySystem
{

    class Program
    {
         static void Main(string[] args)
         {
            List<ColorChip> Chips = new List<ColorChip>();
            Console.Clear();

            Console.WriteLine("1. Generat Random chips.");
            Console.WriteLine("2. Use Predefined chips: [Blue, Yellow] [Red, Green] [Yellow, Red] [Orange, Purple].");

            // Validate input
            int option;
            while (!int.TryParse(Console.ReadLine(), out option) || (option != 1 && option != 2))
            {
                Console.WriteLine("Invalid input. Please enter 1 or 2.");
            }

            switch (option)
            {
                case 1:
                    // Generate random chips
                    Console.WriteLine("Generating random chips...");
                    Chips = GenerateRandomChips(4);
                    break;
                case 2:
                    // Generate Sample Chips
                    Chips.Add(new ColorChip(Color.Blue, Color.Yellow));
                    Chips.Add(new ColorChip(Color.Red, Color.Green));
                    Chips.Add(new ColorChip(Color.Yellow, Color.Red));
                    Chips.Add(new ColorChip(Color.Orange, Color.Purple));
                    break;
            }

            Console.WriteLine("Available Chips:");

            // Display chips
            foreach (var chip in Chips)
            {
                Console.WriteLine("[" + chip + "]");
            }
            Console.WriteLine();
            //Console.ReadLine();
            // find chip from Blue to Green
            List<ColorChip> chipchain = FindChipsChannel(Chips, Color.Blue, Color.Green, null);
            //Console.WriteLine(chipchain.Count());
            //Console.WriteLine();
            if (chipchain != null && chipchain.Count() > 0 && chipchain.Last().EndColor == Color.Green)
            {
                Console.WriteLine("found connecting channel:");
                Console.WriteLine(Color.Blue);
                foreach (var chip in chipchain)
                {
                    Console.WriteLine("[" + chip + "]");
                }
                Console.WriteLine(Color.Green);
            }
            else
            {
                Console.WriteLine("Unable to find a connecting channel starting marker being blue and the ending marker being green.");
            }
            Console.WriteLine();
            Console.ReadLine();
         }


        static List<ColorChip> GenerateRandomChips(int count)
        {
            Random random = new Random();
            List<ColorChip> chips = new List<ColorChip>();

            //for (int i = 0; i < count; i++)
            //{
            //    Color startColor = (Color)random.Next(Enum.GetValues(typeof(Color)).Length);
            //    Color endColor = (Color)random.Next(Enum.GetValues(typeof(Color)).Length);
            //    ColorChip chip = new ColorChip(startColor, endColor);
            //    //if(startColor != endColor && !chips.Contains(chip))
            //    chips.Add(chip);
            //}
            int i = 0;
            while(i < count)
            {
                Color startColor = (Color)random.Next(Enum.GetValues(typeof(Color)).Length);
                Color endColor = (Color)random.Next(Enum.GetValues(typeof(Color)).Length);
                ColorChip chip = new ColorChip(startColor, endColor);
                if (!startColor.Equals(endColor) && !containsColorChip(chips, chip))
                {
                    chips.Add(chip);
                    i++;
                }
            }

            return chips;
        }

        static List<ColorChip> FindChipsChannel(List<ColorChip> chips, Color startColor, Color endColor, List<ColorChip> solutionChips)
        {
            List<ColorChip> tempChip = new List<ColorChip>();
            foreach (ColorChip chip in chips)
            {
                tempChip.Add(chip);
            }

            if(solutionChips == null)
                solutionChips = new List<ColorChip>();

            foreach (var chip in chips.Where(chip => chip.StartColor == startColor))
            {
                tempChip.Remove(chip);
                solutionChips.Add(chip);
                if (chip.EndColor == endColor)
                {
                    break;
                }
                else
                {
                    FindChipsChannel(tempChip, chip.EndColor, Color.Green, solutionChips);
                }
            }
            
            return solutionChips;
        }
        

        private static bool containsColorChip(List<ColorChip> Chips, ColorChip colorChip)
        {
            //duplicate check
            foreach (var ch in Chips)
            {
                bool startColorSame = colorChip.StartColor.Equals(ch.StartColor);
                bool endColorSame = colorChip.EndColor.Equals(ch.EndColor);

                if (startColorSame && endColorSame)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
