using System;
namespace TheatricalPlayersRefactoringKata
{
	public class CalculatedResultForPerformance
	{
        private string _name;
        private int _audience;
        private decimal _amount;

        public string Name { get => _name; set => _name = value; }
        public int Audience { get => _audience; set => _audience = value; }
        public decimal Amount { get => _amount; set => _amount = value; }

        public CalculatedResultForPerformance(string name, int audience, decimal amount)
        {
            this._name = name;
            this._audience = audience;
            this._amount = amount;
        }
    }
}

