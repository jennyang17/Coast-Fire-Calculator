using System;
using System.Collections.Generic;
using System.Text;

namespace Coast_Fire_Calculator
{
    public class Data
    {
        public List<CoastFireNumYear> CalculatedYearValues { get; private set; }

        public int CurrentAge { get; set; }
        public double RetirementAge { get; set; }
        public double AnnualSpending { get; set; }
        public double StartingInvestment { get; set; }
        public double MonthlyContribution { get; set; }
        public double InvestmentGrowthRate { get; set; }
        public double InflationRate { get; set; }
        public double SafeWithdrawalRate { get; set; }
        public int FireNumber { get; private set; }
        public int YearsLeftUntilCoastFire { get; private set; }

        public CoastFireNumYear CoastFireYear { get; private set; }


        public Data(int currentAge, 
            int retirementAge,
             int annualSpending,
             int currentInvestment,
             int monthlyContribution,
             int investmentGrowthRate,
             int inflationRate,
             int safeWithdrawalRate)
        {
            CalculatedYearValues = new List<CoastFireNumYear>();
            CurrentAge = currentAge;
            RetirementAge = retirementAge;
            AnnualSpending = annualSpending;
            StartingInvestment = currentInvestment;
            MonthlyContribution = monthlyContribution;
            InvestmentGrowthRate = investmentGrowthRate;
            InflationRate = inflationRate;
            SafeWithdrawalRate = safeWithdrawalRate;

            CoastFireYear = new CoastFireNumYear()
            {
                Age = CurrentAge,
                Networth = StartingInvestment,
                CoastFireNum = CoastFireNumberCalc(CurrentAge)
            };
            CalculatedYearValues.Add(CoastFireYear);

            FireNumber = FireNumberCalc();

            double rollingInvestment = StartingInvestment;
            for(int nextAge = currentAge + 1; nextAge <= retirementAge; nextAge++)
            {
                CoastFireNumYear currentValue = new CoastFireNumYear();
                currentValue.Networth = (rollingInvestment + (MonthlyContribution * 12)) * TotalGrowthRateCalc();
                currentValue.CoastFireNum = CoastFireNumberCalc(nextAge);
                currentValue.Age = nextAge;
                CalculatedYearValues.Add(currentValue);
                rollingInvestment = currentValue.Networth;
            }

            int coastFireAge = 0;

            foreach (var item in CalculatedYearValues)
            {
                if(item.Networth > item.CoastFireNum)
                {
                    coastFireAge = item.Age;
                    break;
                }
            }
            YearsLeftUntilCoastFire = coastFireAge - currentAge;
            
        }

        private int CoastFireNumberCalc(int age)
        {
            double x = TotalGrowthRateCalc();
            double y = RetirementAge - age;
            double coastFireNum = AnnualSpending / ((SafeWithdrawalRate / 100) * Math.Pow(x, y));
            return (int)coastFireNum;
        }
        
        private int FireNumberCalc()
        {
            double fireNum = AnnualSpending / (SafeWithdrawalRate / 100);
            return (int)fireNum;
        }

        private double TotalGrowthRateCalc()
        {
            return 1 + ((InvestmentGrowthRate - InflationRate) / 100);
        }
    }
    
}
