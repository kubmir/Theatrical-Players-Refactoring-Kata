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
            var result = string.Format("Statement for {0}\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach (var perf in invoice.Performances)
            {
                var play = plays[perf.PlayID];
                var total_seats_in_perf = GetTotalAmountOfSeats(play, perf);

                volumeCredits += CalculateVolumeCreditsAddition(play, perf.Audience);

                // print line for this order
                result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(total_seats_in_perf / 100), perf.Audience);
                totalAmount += total_seats_in_perf;
            }
            result += string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += string.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }

        private int GetTotalAmountOfSeats(Play play, Performance perf)
        {
            return play.Type switch
            {
                "tragedy" => CalculateAmount(perf.Audience, 40000, 30, 1000),
                "comedy" => CalculateAmount(perf.Audience, 30000 + 300 * perf.Audience, 20, 500, 10000),
                _ => throw new Exception("unknown type: " + play.Type),
            };
        }

        private int CalculateVolumeCreditsAddition(Play play, int audience)
        {
            // add volume credits
            var result = Math.Max(audience - 30, 0);
            // add extra credit for every ten comedy attendees
            if ("comedy" == play.Type) result += (int)Math.Floor((decimal)audience / 5);

            return result;
        }

        private int CalculateAmount(int audience, int initial_amount, int threshold,
                                    int multiplication = 1, int addition = 0)
        {
            int result = initial_amount;
            if (audience > threshold)
            {
                result += addition + multiplication * (audience - threshold);
            }
            return result;
        }
    }
}
