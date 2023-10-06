using System.Linq;
using System.Collections.Generic;


string input = Console.ReadLine().Trim();
var (tankers, oilAmount) = ParseInput(input);

List<List<int>> efficientDeliveries = FindEfficientDelivery(tankers, oilAmount);

if (efficientDeliveries.Count == 0)
{
	int minTankerCapacity = tankers.Min();
	int oilToAdd = minTankerCapacity - oilAmount % minTankerCapacity;
	Console.WriteLine(oilToAdd);
} else
{
    Console.WriteLine("[" + string.Join("][", efficientDeliveries.Select(delivery => string.Join(",", delivery))) + "]");
}


static (List<int>, int) ParseInput(string input)
{
	int leftParenthesisIndex = input.IndexOf('(');
	int rightParenthesisIndex = input.IndexOf(')'); // this parses the input between the parenthesis

	string tankerString = input.Substring(leftParenthesisIndex + 1, rightParenthesisIndex - leftParenthesisIndex - 1);
	string oilAmountString = input.Substring(rightParenthesisIndex + 2).Trim();
	List<int> tankers = tankerString.Split(',').Select(int.Parse).ToList();

	int oilAmount = int.Parse(oilAmountString);
	return (tankers, oilAmount);
}

static List<List<int>> FindEfficientDelivery(List<int> tankers, int oilAmount)
{
	List<List<int>> efficientDeliveries	= new List<List<int>>(); 
	GenerateCombinations(tankers, oilAmount, 0, new List<int>(), efficientDeliveries); // return all possible combinations
	return efficientDeliveries;
}

static void GenerateCombinations(List<int> tankers, int remainingOil, int index, List<int> currentCombinations, List<List<int>> efficientDeliveries)
{
	// input takes in the list of possible tankers, the oilAmount, the index from 0 to ... , current
	// combination and list of list of efficient deliveries
	if (index == tankers.Count)
	{
		if(remainingOil == 0)
		{
			efficientDeliveries.Add(new List<int>(currentCombinations));
		}
		return;
	} // if the index reaches the last tanker and there's no more oil: return

	int tankerCapacity = tankers[index]; // tankers[0] = 2
	int maxTankers = remainingOil / tankerCapacity; // for (2, 5), 12, maxtankers = 6

	for (int i = 0; i <= maxTankers; i++)
	{
		currentCombinations.Add(i); //add(0) = 0 (take the (0 * 2 + x * 5) into account
		GenerateCombinations(tankers, remainingOil - i * tankerCapacity, index + 1, currentCombinations, efficientDeliveries);
		currentCombinations.RemoveAt(currentCombinations.Count - 1);
	}
}