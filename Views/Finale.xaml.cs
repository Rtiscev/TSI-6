using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace TSI_6.Views
{
    public partial class Finale : UserControl
    {
        Dictionary<string, int> impacts { get; set; }
        public Dictionary<string, Dictionary<string, List<int>>> results { get; set; }
        Dictionary<string, List<int>> outofTwenty = new();
        public string HardwareVTxt { get; set; }
        public string SoftwareVTxt { get; set; }
        public string NetworkVTxt { get; set; }
        public string PersonalVTxt { get; set; }
        public string LocationVTxt { get; set; }

        public string HardwareTTxt { get; set; }
        public string SoftwareTTxt { get; set; }
        public string NetworkTTxt { get; set; }
        public string PersonalTTxt { get; set; }
        public string LocationTTxt { get; set; }

        public string HardwareITxt { get; set; }
        public string SoftwareITxt { get; set; }
        public string NetworkITxt { get; set; }
        public string PersonalITxt { get; set; }
        public string LocationITxt { get; set; }


        public string HardwarePTxt { get; set; }
        public string SoftwarePTxt { get; set; }
        public string NetworkPTxt { get; set; }
        public string PersonalPTxt { get; set; }
        public string LocationPTxt { get; set; }

        public string HardwarePRTxt { get; set; }
        public string SoftwarePRTxt { get; set; }
        public string NetworkPRTxt { get; set; }
        public string PersonalPRTxt { get; set; }
        public string LocationPRTxt { get; set; }

        public string HardwareRTxt { get; set; }
        public string SoftwareRTxt { get; set; }
        public string NetworkRTxt { get; set; }
        public string PersonalRTxt { get; set; }
        public string LocationRTxt { get; set; }

        public string VulnerTotal { get; set; }
        public string ThreatsTotal { get; set; }
        public string RisksTotal { get; set; }
        public string RiskValueTotal { get; set; }
        Dictionary<int, string> ffff = new();

        public Finale()
        {
            InitializeComponent();
            DataContext = this;
        }

        public Finale(ref Dictionary<string, Dictionary<string, List<int>>> results, ref Dictionary<string, int> Impacts)
        {
            InitializeComponent();

            ffff.Add(1, "очень низкий");
            ffff.Add(2, "низкий");
            ffff.Add(3, "средний");
            ffff.Add(4, "высокий");
            ffff.Add(5, "очень высокий");

            this.impacts = Impacts;
            this.results = results;
            string tail = "/20";

            // vulnerabilities
            HardwareVTxt = results["Вопросы оценки уязвимостей"]["Аппаратные средства"].Sum().ToString() + tail;
            SoftwareVTxt = results["Вопросы оценки уязвимостей"]["Программное обеспечение"].Sum().ToString() + tail;
            NetworkVTxt = results["Вопросы оценки уязвимостей"]["Сеть и коммуникация"].Sum().ToString() + tail;
            PersonalVTxt = results["Вопросы оценки уязвимостей"]["Персонал"].Sum().ToString() + tail;
            LocationVTxt = results["Вопросы оценки уязвимостей"]["Место функционирования организации"].Sum().ToString() + tail;

            // threats
            HardwareTTxt = results["Вопросы оценки угроз"]["Аппаратные средства"].Sum().ToString() + tail;
            SoftwareTTxt = results["Вопросы оценки угроз"]["Программное обеспечение"].Sum().ToString() + tail;
            NetworkTTxt = results["Вопросы оценки угроз"]["Сеть и коммуникация"].Sum().ToString() + tail;
            PersonalTTxt = results["Вопросы оценки угроз"]["Персонал"].Sum().ToString() + tail;
            LocationTTxt = results["Вопросы оценки угроз"]["Место функционирования организации"].Sum().ToString() + tail;


            HardwareITxt = Impacts["Аппаратные средства"].ToString();
            SoftwareITxt = Impacts["Программное обеспечение"].ToString();
            NetworkITxt = Impacts["Сеть и коммуникация"].ToString();
            PersonalITxt = Impacts["Персонал"].ToString();
            LocationITxt = Impacts["Место функционирования организации"].ToString();

            Dictionary<int, double> dict = new();
            dict.Add(1, 0.07);
            dict.Add(2, 0.13);
            dict.Add(3, 0.2);
            dict.Add(4, 0.27);
            dict.Add(5, 0.33);

            HardwarePTxt = dict[Impacts["Аппаратные средства"]].ToString();
            SoftwarePTxt = dict[Impacts["Программное обеспечение"]].ToString();
            NetworkPTxt = dict[Impacts["Сеть и коммуникация"]].ToString();
            PersonalPTxt = dict[Impacts["Персонал"]].ToString();
            LocationPTxt = dict[Impacts["Место функционирования организации"]].ToString();

            HardwarePRTxt = getRiskPoints(HardwareVTxt, HardwareTTxt, HardwarePTxt);
            SoftwarePRTxt = getRiskPoints(HardwareVTxt, SoftwareTTxt, SoftwarePTxt);
            NetworkPRTxt = getRiskPoints(NetworkVTxt, NetworkTTxt, NetworkPTxt);
            PersonalPRTxt = getRiskPoints(PersonalVTxt, PersonalTTxt, PersonalPTxt);
            LocationPRTxt = getRiskPoints(LocationVTxt, LocationTTxt, LocationPTxt);

            HardwareRTxt = getRiskValue(HardwarePRTxt, HardwarePTxt);
            SoftwareRTxt = getRiskValue(SoftwarePRTxt, SoftwarePTxt);
            NetworkRTxt = getRiskValue(NetworkPRTxt, NetworkPTxt);
            PersonalRTxt = getRiskValue(PersonalPRTxt, PersonalPTxt);
            LocationRTxt = getRiskValue(LocationPRTxt, LocationPTxt);

            string hundred = "/100";
            VulnerTotal = getSum(HardwareVTxt, SoftwareVTxt, NetworkVTxt, PersonalVTxt, LocationVTxt, false) + hundred;
            ThreatsTotal = getSum(HardwareTTxt, SoftwareTTxt, NetworkTTxt, PersonalTTxt, LocationTTxt, false) + hundred;
            RisksTotal = getSum(HardwarePRTxt, SoftwarePRTxt, NetworkPRTxt, PersonalPRTxt, LocationPRTxt, true) + hundred;
            RiskValueTotal = uniqueSum(HardwareRTxt, SoftwareRTxt, NetworkRTxt, PersonalRTxt, LocationRTxt);

            DataContext = this;
        }

        private string uniqueSum(string hardwareRTxt, string softwareRTxt, string networkRTxt, string personalRTxt, string locationRTxt)
        {
            int sum1 = findKey(hardwareRTxt);
            int sum2 = findKey(softwareRTxt);
            int sum3 = findKey(networkRTxt);
            int sum4 = findKey(personalRTxt);
            int sum5 = findKey(locationRTxt);
            double total = (sum1 + sum2 + sum3 + sum4 + sum5) / 5.00;
            total = Math.Round(total);
            return ffff[((int)total)];
        }

        private int findKey(string str)
        {
            int i = 1;
            foreach (var item in ffff.Values)
            {
                if (item == str)
                    break;
                else
                    i++;
            }
            return i;
        }

        private string getSum(string HardwareVTxt1, string SoftwareVTxt1, string NetworkVTxt1, string PersonalVTxt1, string LocationVTxt1, bool choice)
        {
            if (!choice)
            {
                HardwareVTxt1 = trimTwenty(HardwareVTxt1);
                SoftwareVTxt1 = trimTwenty(SoftwareVTxt1);
                NetworkVTxt1 = trimTwenty(NetworkVTxt1);
                PersonalVTxt1 = trimTwenty(PersonalVTxt1);
                LocationVTxt1 = trimTwenty(LocationVTxt1);
            }

            int sum = Convert.ToInt32(HardwareVTxt1) + Convert.ToInt32(SoftwareVTxt1) + Convert.ToInt32(NetworkVTxt1) + Convert.ToInt32(PersonalVTxt1) + Convert.ToInt32(LocationVTxt1);
            return sum.ToString();
        }

        private string trimTwenty(string str)
        {
            int indexOFTrim = str.IndexOf('/');
            str = str.Substring(0, indexOFTrim);
            return str;
        }
        private string getRiskPoints(string score_vulnerab, string score_threats, string influence)
        {
            score_vulnerab = trimTwenty(score_vulnerab);
            score_threats = trimTwenty(score_threats);


            int score_sum = Convert.ToInt32(score_vulnerab) + Convert.ToInt32(score_threats);
            int percentage_of_score = (score_sum * 100) / 40;
            double inf = Convert.ToDouble(influence) * 100;
            int risk_score = (int)Math.Round((percentage_of_score * inf) / 100);
            return risk_score.ToString();
        }

        private string getRiskValue(string RS, string inf)
        {
            double risk_score = Convert.ToInt32(RS);
            double influence = Convert.ToDouble(inf) * 100;
            //All categories of risks
            double very_low = influence / 5.00;
            double low = influence / 5.00 * 2;
            double middle = influence / 5.00 * 3;
            double high = influence / 5.00 * 4;
            double very_high = influence;

            int index_risk_score = 0;


            if (0 <= risk_score && risk_score < very_low)
            {
                // Very_Low risk
                index_risk_score = 1;
            }
            if (very_low <= risk_score && risk_score < low)
            {
                // Low risk
                index_risk_score = 2;
            }
            if (low <= risk_score && risk_score < middle)
            {
                // Middle risk
                index_risk_score = 3;
            }
            if (middle <= risk_score && risk_score < high)
            {
                // High risk
                index_risk_score = 4;
            }
            if (high <= risk_score && risk_score <= very_high)
            {
                index_risk_score = 5;
            }
            return ffff[index_risk_score];

        }
    }
}
