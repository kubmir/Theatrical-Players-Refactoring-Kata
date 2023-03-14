using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var performanceResults = new List<CalculatedResultForPerformance>();

            foreach(var perf in invoice.Performances)
            {
                var play = plays[perf.PlayID];

                var thisAmount = CalculateAmountForCurrentPerformance(perf, play);

                performanceResults.Add(new CalculatedResultForPerformance(play.Name, perf.Audience, Convert.ToDecimal(thisAmount / 100)));

                volumeCredits += CalculateVolumeCredits(perf, play);
                totalAmount += thisAmount;
            }

            return PrintResults(invoice.Customer, performanceResults, totalAmount, volumeCredits);
        }

        private int CalculateAmountForCurrentPerformance(Performance perf, Play play)
        {
            switch (play.Type)
            {
                case "tragedy":
                    return CalculateTragedyTotalAmount(perf);
                case "comedy":
                    return CalculateComedyTotalAmount(perf);
                default:
                    throw new Exception("unknown type: " + play.Type);
            }
        }

        private string PrintResults(string customer, List<CalculatedResultForPerformance> performanceResults, int totalAmount, int volumeCredits)
        {
            CultureInfo cultureInfo = new CultureInfo("en-US");
            var result = string.Format("Statement for {0}\n", customer);

            performanceResults.ForEach(performanceResult =>
            {
                result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", performanceResult.Name, performanceResult.Amount, performanceResult.Audience);
            });

            result += string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += string.Format("You earned {0} credits\n", volumeCredits);

            return result;
        }

        private int CalculateVolumeCredits(Performance performance, Play play)
        {
            var volumeCredits = 0;

            // add volume credits
            volumeCredits += Math.Max(performance.Audience - 30, 0);
            // add extra credit for every ten comedy attendees
            if ("comedy" == play.Type) volumeCredits += (int)Math.Floor((decimal)performance.Audience / 5);

            return volumeCredits;
        }

        private int CalculateComedyTotalAmount(Performance perf)
        {
            int thisAmount = 30000;
            if (perf.Audience > 20)
            {
                thisAmount += 10000 + 500 * (perf.Audience - 20);
            }
            thisAmount += 300 * perf.Audience;
            return thisAmount;
        }

        private int CalculateTragedyTotalAmount(Performance performance)
        {
            var thisAmount = 40000;

            if (performance.Audience > 30)
            {
                thisAmount += 1000 * (performance.Audience - 30);
            }

            return thisAmount;
        }
    }
}
