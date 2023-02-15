using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coast_Fire_Calculator
{
    public partial class CoastFireCalculator : Form
    {
        public ISeries[] Series { get; set; } =
        {
            new LineSeries<double>
            {
                Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                Fill = null
            }
        };


        private Data _myData;

        public CoastFireCalculator()
        {
            InitializeComponent();

            lbCurrentInvestmentGrowthRate.Text = trackbInvestmentGrowth.Value.ToString() + " %";
            lbCurrentInflationRate.Text = trackbInflationRate.Value.ToString() + " %";
            lbCurrentSafeWithdrawalRate.Text = trackbSafeWithdrawalRate.Value.ToString() + " %";
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

            _myData = new Data(
                Convert.ToInt32(tbCurrentAge.Text),
                Convert.ToInt32(tbRetirementAge.Text),
                Convert.ToInt32(tbAnnualSpending.Text),
                Convert.ToInt32(tbCurrentInvestment.Text),
                Convert.ToInt32(tbMonthlyContribution.Text),
                trackbInvestmentGrowth.Value,
                trackbInflationRate.Value,
                trackbSafeWithdrawalRate.Value);


            lbCFNoCurrentAge.Text = "£ " + _myData.CoastFireYear.CoastFireNum.ToString();
            lbCoastFireYear.Text = $"You're {_myData.YearsLeftUntilCoastFire} years from Coast FIRE!";

            List<ISeries> series = new List<ISeries>();
            series.Add(new LineSeries<CoastFireNumYear>()
            {
                Name = "NetWorth : ",
                GeometrySize = 4,
                Values = _myData.CalculatedYearValues,
                Mapping = (coastFireNumYear, point) =>
                {
                    point.PrimaryValue = (double)coastFireNumYear.Networth;
                    point.SecondaryValue = (int)coastFireNumYear.Age;
                },
                Fill = null
            });
            series.Add(new LineSeries<CoastFireNumYear>()
            {
                Name = "Coast Fire Number : ",
                GeometrySize = 4,
                Values = _myData.CalculatedYearValues,
                Mapping = (coastFireNumYear, point) =>
                {
                    point.PrimaryValue = (double)coastFireNumYear.CoastFireNum;
                    point.SecondaryValue = (int)coastFireNumYear.Age;
                },
                Fill = null
            });


            cartesianChart1.Series = series;
            cartesianChart1.Refresh();
        }

        private void trackbInvestmentGrowth_Scroll(object sender, EventArgs e)
        {
            lbCurrentInvestmentGrowthRate.Text = trackbInvestmentGrowth.Value.ToString() + " %";
        }

        private void trackbInflationRate_Scroll(object sender, EventArgs e)
        {
            lbCurrentInflationRate.Text = trackbInflationRate.Value.ToString() + " %";
        }

        private void trackbSafeWithdrawalRate_Scroll(object sender, EventArgs e)
        {
            lbCurrentSafeWithdrawalRate.Text = trackbSafeWithdrawalRate.Value.ToString() + " %";
        }
    }
}
