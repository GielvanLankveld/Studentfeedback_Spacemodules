/**
 * Copyright (C) 2016 Open University (http://www.ou.nl/)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace StudentFeedback_SpaceModules
{
    public partial class Form1 : Form
    {
        private Dictionary<Tuple<String,String,String,String>, Record> records;
        private Dictionary<String, Distribution> distributionsDictionary = new Dictionary<String, Distribution>();
        private Tuple<String, String, String, String> selectedRecordKey;

        public Form1()
        {
            InitializeComponent();
        }

        private String openCSVFileDialog()
        {
            //Prep the dialog
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = System.Configuration.ConfigurationManager.AppSettings["defaultDirectory"]; //For debugging
            openFileDialog1.Filter = "csv files (*.csv)|*.csv";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

            }

            return openFileDialog1.FileName;
        }

        private String openJsonFileDialog()
        {
            //Prep the dialog
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "C:\\Users\\Worker\\Documents\\Git_repositories\\StudentFeedback_SpaceModules"; //For debugging
            openFileDialog1.Filter = "json files (*.json)|*.json";
            openFileDialog1.RestoreDirectory = true;

            //Get the records from the csv
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            { }

            return openFileDialog1.FileName;
        }
        
        private void LoadFromCSV(String fileName)
        {
            //Prep the list
            Dictionary<Tuple<String, String, String, String>, Record> inputList = new Dictionary<Tuple<String, String, String, String>, Record>();

            //Read the file
            string[] lines;
            if (fileName == "")
            {
                lines = System.IO.File.ReadAllLines("C:\\Users\\Worker\\Documents\\Git_repositories\\StudentFeedback_SpaceModules\\Records.csv");
            }
            else
            {
                lines = System.IO.File.ReadAllLines(fileName);
            }

            string[] stringSeparators = new string[] { "," };

            //Parse the file
            for (var i = 2; i < lines.Length; i++)
            {
                string[] items = lines[i].Split(stringSeparators, StringSplitOptions.None);
                Record tempRec = new Record();
                if (int.TryParse(items[4], out tempRec.Score1) != true) tempRec.Score1 = 0;
                if (int.TryParse(items[5], out tempRec.Score2) != true) tempRec.Score2 = 0;
                if (int.TryParse(items[6], out tempRec.Score3) != true) tempRec.Score3 = 0;
                if (int.TryParse(items[7], out tempRec.Score4) != true) tempRec.Score4 = 0;
                if (int.TryParse(items[8], out tempRec.Score5) != true) tempRec.Score5 = 0;

                Tuple<String, String, String, String> key = new Tuple<String, String, String, String>(items[0], items[1], items[2], items[3].ToString());
                inputList.Add(key,tempRec);
            }

            //Output the individual scores lists and the distributions
            records = inputList;
            GenerateDistributionsFromRecords();
        }
                
        private void GenerateDistributionsFromRecords()
        {
            //Create a distribution for each unique group (CEFIP)
            Distribution tempDist;
            foreach (Tuple<String, String, String, String> item in records.Keys)
            {
                //Update closing
                String key = "closing_" + item.Item2 + "_" + item.Item3 + "_" + item.Item4;
                if (!distributionsDictionary.TryGetValue(key, out tempDist))
                {
                    tempDist = new Distribution();
                }
                tempDist = UpdateDistribution(tempDist, records[item].Score1);
                distributionsDictionary.Remove(key);
                distributionsDictionary.Add(key, tempDist);

                // Update empathy
                key = "empathy_" + item.Item2 + "_" + item.Item3 + "_" + item.Item4;
                if (!distributionsDictionary.TryGetValue(key, out tempDist))
                {
                    tempDist = new Distribution();
                }
                tempDist = UpdateDistribution(tempDist, records[item].Score2);
                distributionsDictionary.Remove(key);
                distributionsDictionary.Add(key, tempDist);

                // Update find in db
                tempDist = new Distribution();
                key = "findindb_" + item.Item2 + "_" + item.Item3 + "_" + item.Item4;
                if (!distributionsDictionary.TryGetValue(key, out tempDist))
                {
                    tempDist = new Distribution();
                }
                tempDist = UpdateDistribution(tempDist, records[item].Score3);
                distributionsDictionary.Remove(key);
                distributionsDictionary.Add(key, tempDist);

                // Update inquire
                tempDist = new Distribution();
                key = "inquire_" + item.Item2 + "_" + item.Item3 + "_" + item.Item4;
                if (!distributionsDictionary.TryGetValue(key, out tempDist))
                {
                    tempDist = new Distribution();
                }
                tempDist = UpdateDistribution(tempDist, records[item].Score4);
                distributionsDictionary.Remove(key);
                distributionsDictionary.Add(key, tempDist);

                // Update polite
                tempDist = new Distribution();
                key = "polite_" + item.Item2 + "_" + item.Item3 + "_" + item.Item4;
                if (!distributionsDictionary.TryGetValue(key, out tempDist))
                {
                    tempDist = new Distribution();
                }
                tempDist = UpdateDistribution(tempDist, records[item].Score5);
                distributionsDictionary.Remove(key);
                distributionsDictionary.Add(key, tempDist);
            }
        }

        private String generateTextAdvice(Distribution distribution, Record studentPerformance, int targetSkill)
        {
            //Generate advice for student based on his performance and the group performance
            String advice = "";
            int performance;

            switch (targetSkill)
            {
                case 1:
                    {
                        //Closing
                        performance = studentPerformance.Score1;

                        advice = "Afsluiten: ";

                        switch (performance)
                        {
                            case 1:
                                {
                                    //Low performance
                                    advice += "op het gebied van gesprekken afsluiten doe je het onder het gemiddelde van je klas. Waarschijnlijk kun je je communicatie op dit onderdeel nog verbeteren. Probeer vriendelijk te blijven en te kijken of je de klant verder nog kunt helpen.";
                                }; break;
                            case 2:
                                {
                                    //Average/low performance
                                    advice += "op het gebied van gesprekken afsluiten doe je het net zo goed als het gemiddelde van je klas. Hou dit vol.";
                                }; break;
                            case 3:
                                {
                                    //Average/high performance
                                    advice += "op het gebied van gesprekken afsluiten doe je het iets beter dan het gemiddelde van je klas. Dit is heel netjes.";
                                }; break;
                            case 4:
                                {
                                    //High performance
                                    advice += "op het gebied van gesprekken afsluiten ben je een van de besten van de klas. Uitstekend!";
                                }; break;
                        }
                    }; break;
                case 2:
                    {
                        //Empathy
                        performance = studentPerformance.Score2;

                        advice = "Invoelen: ";

                        switch (performance)
                        {
                            case 1:
                                {
                                    //Low performance
                                    advice += "op het gebied van invoelen doe je het onder het gemiddelde van je klas. Waarschijnlijk kun je je communicatie op dit onderdeel nog verbeteren. Probeer vriendelijk te blijven en begrip te tonen als een klant emotioneel wordt.";
                                }; break;
                            case 2:
                                {
                                    //Average/low performance
                                    advice += "op het gebied van invoelen doe je het net zo goed als het gemiddelde van je klas. Hou dit vol.";
                                }; break;
                            case 3:
                                {
                                    //Average/high performance
                                    advice += "op het gebied van invoelen doe je het iets beter dan het gemiddelde van je klas. Dit is heel netjes.";
                                }; break;
                            case 4:
                                {
                                    //High performance
                                    advice += "op het gebied van invoelen ben je een van de besten van de klas. Uitstekend!";
                                }; break;
                        }
                    }; break;
                case 3:
                    {
                        //Find in DB
                        performance = studentPerformance.Score3;

                        advice = "Informatie opzoeken: ";

                        switch (performance)
                        {
                            case 1:
                                {
                                    //Low performance
                                    advice += "op het gebied van informatie opzoeken doe je het onder het gemiddelde van je klas. Waarschijnlijk kun je je communicatie op dit onderdeel nog verbeteren. Probeer te zoeken in de modules en let goed op: sommige problemen zijn nog niet bekend! Kun je niet onthouden welk onderdeel het was? Schrijf dan dingen op. Vergeet ook niet om aan de klant te melden dat je de informatie gaat opzoeken.";
                                }; break;
                            case 2:
                                {
                                    //Average/low performance
                                    advice += "op het gebied van informatie opzoeken doe je het net zo goed als het gemiddelde van je klas. Hou dit vol.";
                                }; break;
                            case 3:
                                {
                                    //Average/high performance
                                    advice += "op het gebied van informatie opzoeken doe je het iets beter dan het gemiddelde van je klas. Dit is heel netjes.";
                                }; break;
                            case 4:
                                {
                                    //High performance
                                    advice += "op het gebied van informatie opzoeken ben je een van de besten van de klas. Uitstekend!";
                                }; break;
                        }
                    }; break;
                case 4:
                    {
                        //Inquire
                        performance = studentPerformance.Score4;

                        advice = "Vragen stellen: ";

                        switch (performance)
                        {
                            case 1:
                                {
                                    //Low performance
                                    advice += "op het gebied van vragen stellen doe je het onder het gemiddelde van je klas. Waarschijnlijk kun je je communicatie op dit onderdeel nog verbeteren. Probeer telkens te kijken of je alles weet wat je nodig hebt om de klant te helpen. Kun je het niet onthouden? Schrijf dan dingen op.";
                                }; break;
                            case 2:
                                {
                                    //Average/low performance
                                    advice += "op het gebied van vragen stellen doe je het net zo goed als het gemiddelde van je klas. Hou dit vol.";
                                }; break;
                            case 3:
                                {
                                    //Average/high performance
                                    advice += "op het gebied van vragen stellen doe je het iets beter dan het gemiddelde van je klas. Dit is heel netjes.";
                                }; break;
                            case 4:
                                {
                                    //High performance
                                    advice += "op het gebied van vragen stellen ben je een van de besten van de klas. Uitstekend!";
                                }; break;
                        }
                    }; break;
                case 5:
                    {
                        //Polite
                        performance = 4;

                        advice = "Vriendelijkheid: ";

                        switch (performance)
                        {
                            case 1:
                                {
                                    //Low performance
                                    advice += "op het gebied van vriendelijkheid doe je het onder het gemiddelde van je klas. Probeer dingen zo netjes mogelijk te zeggen en blijf rustig en beleefd, ook als de klant boos wordt. Vergeet ook niet aan de klant te melden als je tijd nodig hebt om informatie op te zoeken.";
                                }; break;
                            case 2:
                                {
                                    //Average/low performance
                                    advice += "op het gebied van vriendelijkheid doe je het net zo goed als het gemiddelde van je klas. Hou dit vol.";
                                }; break;
                            case 3:
                                {
                                    //Average/high performance
                                    advice += "op het gebied van vriendelijkheid doe je het iets beter dan het gemiddelde van je klas. Dit is heel netjes.";
                                }; break;
                            case 4:
                                {
                                    //High performance
                                    advice += "op het gebied van vriendelijkheid ben je een van de besten van de klas. Uitstekend!";
                                }; break;
                        }
                    }; break;
            }

            //Output personalized advice
            return advice;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Select a file and load records form csv
            LoadFromCSV(openCSVFileDialog());

            //Prep the comboboxes
            comboBoxROC.Enabled = true;
            comboBoxROC.Items.Clear();

            comboBoxGroep.Enabled = true;
            comboBoxGroep.Items.Clear();

            comboBoxLes.Enabled = true;
            comboBoxLes.Items.Clear();

            comboBoxStudent.Enabled = true;
            comboBoxStudent.Items.Clear();
            
            //Fill the variables
            foreach (Tuple<String, String, String, String> item in records.Keys)
            {
                if (!comboBoxROC.Items.Contains(item.Item2))
                    comboBoxROC.Items.Add(item.Item2);
            }

            if (comboBoxROC.Items.Count > 0)
            {
                comboBoxROC.SelectedIndex = 0;

                foreach (Tuple<String, String, String, String> item in records.Keys)
                {
                    if (comboBoxROC.SelectedItem.ToString() == item.Item2) 
                        if (!comboBoxGroep.Items.Contains(item.Item3))
                            comboBoxGroep.Items.Add(item.Item3);
                }

                if (comboBoxGroep.Items.Count > 0)
                {
                    comboBoxGroep.SelectedIndex = 0;

                    foreach (Tuple<String, String, String, String> item in records.Keys)
                    {
                        if (comboBoxROC.SelectedItem.ToString() == item.Item2 && comboBoxGroep.SelectedItem.ToString() == item.Item3)
                            if (!comboBoxLes.Items.Contains(item.Item4))
                                comboBoxLes.Items.Add(item.Item4);
                    }

                    if (comboBoxLes.Items.Count > 0)
                    {
                        comboBoxLes.SelectedIndex = 0;

                        foreach (Tuple<String, String, String, String> item in records.Keys)
                        {
                            if (comboBoxROC.SelectedItem.ToString() == item.Item2 && comboBoxGroep.SelectedItem.ToString() == item.Item3 && comboBoxLes.SelectedItem.ToString() == item.Item4)
                                if (!comboBoxStudent.Items.Contains(item.Item1))
                                    comboBoxStudent.Items.Add(item.Item1);
                        }

                        if (comboBoxStudent.Items.Count > 0)
                            comboBoxStudent.SelectedIndex = 0;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Generate output for student
            chart1.SaveImage("test.jpeg", ChartImageFormat.Jpeg);
            Image image = Image.FromFile("test.jpeg");
        }

        private void fillAdvice(Tuple<String, String, String, String> recordToUse)
        {
            //Opening
            String advice = "Beste leerling. In dit overzicht krijg je uitleg over je prestaties in de Space Modules game. "
                            + "De Space Modules game is bedoeld om je beter te maken in communicatie en "
                            + "je krijgt nu per communicatie onderdeel feedback en advies.@@";
            
            Tuple<String, String, String, String> recKey = recordToUse;
            String distKey = recKey.Item2+"_" + recKey.Item3+"_" + recKey.Item4;
            advice += generateTextAdvice(distributionsDictionary["closing_"+distKey],records[recKey],1) + "@"; //Closing
            advice += generateTextAdvice(distributionsDictionary["empathy_"+distKey],records[recKey],2) + "@"; //Empathy
            advice += generateTextAdvice(distributionsDictionary["findindb_" + distKey], records[recKey], 3) + "@"; //Find in DB
            advice += generateTextAdvice(distributionsDictionary["inquire_" + distKey], records[recKey], 4) + "@"; //Inquiry
            advice += generateTextAdvice(distributionsDictionary["polite_" + distKey], records[recKey], 5) + "@@"; //Politeness
            
            //Closing
            advice += "Probeer deze adviezen te gebruiken om zo goed mogelijk te worden in communicatie. "
                        +"Blijf goed je best doen, zelfs als je al uitstekende scores haalt. "
                        +"Door te blijven oefenen kun je beter worden.";

            //Add newline characters
            advice = advice.Replace("@", System.Environment.NewLine);

            richTextBox1.Clear();
            richTextBox1.Text = advice;
        }

        private void fillTable(Tuple<String, String, String, String> recordToUse)
        {
            dataGridView2.Rows.Clear();
            foreach (Tuple<String, String, String, String> record in records.Keys)
            {
                if (record.Item1 == recordToUse.Item1 && record.Item2 == recordToUse.Item2 && record.Item3 == recordToUse.Item3 && record.Item4 == recordToUse.Item4)
                {
                    dataGridView2.Rows.Add(record.Item2,record.Item3,record.Item4,records[record].Score1, records[record].Score2, records[record].Score3, records[record].Score4, records[record].Score5);
                }
            }
        }

        private void fillCharts(Tuple<String, String, String, String> recordToUse)
        {
            //Change data in the form charts
            chart1.Series.Clear();

            for (var i = 0; i == 4; i++)
            {
                //chart1.Series.Add();
                //records[comboBox1.SelectedIndex].Score1
            }
        }

        private void comboBoxROC_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Een ROC is geselecteerd, verander de inhoud van de groep combobox
            comboBoxGroep.Items.Clear();
            comboBoxLes.Items.Clear();
            comboBoxStudent.Items.Clear();
            
            foreach (Tuple<String, String, String, String> item in records.Keys)
            {
                if (comboBoxROC.SelectedItem.ToString() == item.Item2)
                    if (!comboBoxGroep.Items.Contains(item.Item3))
                        comboBoxGroep.Items.Add(item.Item3);
            }

            if (comboBoxGroep.Items.Count > 0)
            {
                comboBoxGroep.SelectedIndex = 0;

                foreach (Tuple<String, String, String, String> item in records.Keys)
                {
                    if (comboBoxROC.SelectedItem.ToString() == item.Item2 && comboBoxGroep.SelectedItem.ToString() == item.Item3)
                        if (!comboBoxLes.Items.Contains(item.Item4))
                            comboBoxLes.Items.Add(item.Item4);
                }

                if (comboBoxLes.Items.Count > 0)
                {
                    comboBoxLes.SelectedIndex = 0;

                    foreach (Tuple<String, String, String, String> item in records.Keys)
                    {
                        if (comboBoxROC.SelectedItem.ToString() == item.Item2 && comboBoxGroep.SelectedItem.ToString() == item.Item3 && comboBoxLes.SelectedItem.ToString() == item.Item4)
                            if (!comboBoxStudent.Items.Contains(item.Item1))
                                comboBoxStudent.Items.Add(item.Item1);
                    }

                    if (comboBoxStudent.Items.Count > 0)
                        comboBoxStudent.SelectedIndex = 0;
                }
            }
        }

        private void comboBoxGroep_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Een groep (binnen een ROC) is geselecteerd, verander de inhoud van de student combobox
            comboBoxLes.Items.Clear();
            comboBoxStudent.Items.Clear();
            
            foreach (Tuple<String, String, String, String> item in records.Keys)
            {
                if (comboBoxROC.SelectedItem.ToString() == item.Item2 && comboBoxGroep.SelectedItem.ToString() == item.Item3)
                    if (!comboBoxLes.Items.Contains(item.Item4))
                        comboBoxLes.Items.Add(item.Item4);
            }

            if (comboBoxLes.Items.Count > 0)
            {
                comboBoxLes.SelectedIndex = 0;

                foreach (Tuple<String, String, String, String> item in records.Keys)
                {
                    if (comboBoxROC.SelectedItem.ToString() == item.Item2 && comboBoxGroep.SelectedItem.ToString() == item.Item3 && comboBoxLes.SelectedItem.ToString() == item.Item4)
                        if (!comboBoxStudent.Items.Contains(item.Item1))
                            comboBoxStudent.Items.Add(item.Item1);
                }

                if (comboBoxStudent.Items.Count > 0)
                    comboBoxStudent.SelectedIndex = 0;
            }
        }

        private void comboBoxLes_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxStudent.Items.Clear();

            foreach (Tuple<String, String, String, String> item in records.Keys)
            {
                if (comboBoxROC.SelectedItem.ToString() == item.Item2 && comboBoxGroep.SelectedItem.ToString() == item.Item3 && comboBoxLes.SelectedItem.ToString() == item.Item4)
                    if (!comboBoxStudent.Items.Contains(item.Item1))
                        comboBoxStudent.Items.Add(item.Item1);
            }

            if (comboBoxStudent.Items.Count > 0)
                comboBoxStudent.SelectedIndex = 0;
        }

        private void comboBoxStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Een student is geselecteerd, vul alle grafieken, tabellen en genereer advies

            selectedRecordKey = new Tuple<String, String, String, String>(
                comboBoxStudent.SelectedItem.ToString(),
                comboBoxROC.SelectedItem.ToString(),
                comboBoxGroep.SelectedItem.ToString(),
                comboBoxLes.SelectedItem.ToString()
                );

            //fillCharts(selectedRecordKey);
            fillTable(selectedRecordKey);
            fillAdvice(selectedRecordKey);
        }

        private Distribution UpdateDistribution(Distribution distribution, Double newScore)
        {
            // New N, code guarantees N of 1 or higher
            int tmpNL = distribution.n;

            if (tmpNL <= 0)
            {
                tmpNL = 1;

                // First max and min
                distribution.max = newScore;
                distribution.min = newScore;
            }
            else
            {
                tmpNL++;

                // New max
                if (newScore > distribution.max)
                    distribution.max = newScore;

                // New min
                if (newScore < distribution.min)

                    distribution.min = newScore;
            }
            distribution.n = tmpNL;
            Double tmpN = (double)tmpNL;

            // New sum
            distribution.sum = newScore + distribution.sum;

            // New mean, variance, & stdDev >> based on:
            // http://www.johndcook.com/blog/standard_deviation/
            Double oldMean = distribution.mean;
            Double newMean;
            Double oldS = distribution.variance;
            Double newS;

            if (tmpN == 1)
            {
                newMean = newScore;
                newS = 0D;
            }
            else
            {
                // New means formula suitable for big data
                // Previous code guarantees tmpN > 0
                newMean = oldMean + (newScore - oldMean) / tmpN;
                newS = oldS + (newScore - oldMean) * (newScore - newMean);
            }
            distribution.mean = newMean;
            distribution.variance = ((tmpN > 1) ? (newS / (tmpN - 1)) : 0D);
            distribution.standardDeviation = (Math.Sqrt(distribution.variance));

            // New skewness & kurtosis >> based on:
            // http://www.johndcook.com/blog/skewness_kurtosis/
            double delta, delta_n, delta_n2, term1;

            delta = newScore - newMean;
            delta_n = delta / tmpN;
            delta_n2 = delta_n * delta_n;
            term1 = delta * delta_n * tmpN;
            distribution.helper3 = ((term1 * delta_n2 * (tmpN * tmpN - 3 * tmpN + 3))
                            + (6 * delta_n2 * distribution.helper1) - (4 * delta_n * distribution.helper2));
            distribution.helper2 = ((term1 * delta_n * (tmpN - 2)) - (3 * tmpN

                    * delta_n * distribution.helper1));
            distribution.helper1 = term1;

            distribution.skewness = (Math.Sqrt((double)tmpN)

                    * distribution.helper2 / Math.Pow(distribution.helper1, 1.5));
            distribution.kurtosis = (((double)tmpN) * distribution.helper3
                    / (distribution.helper1 * distribution.helper1) - 3.0);

            //Checking the normality assumption

            int SMALL_SAMPLE_MAX = 29;
            Double SMALL_SAMPLE_THRESHOLD = 1.96;
            Double LARGE_SAMPLE_THRESHOLD = 2.58;

            Boolean normal = true;

            Double skew = distribution.skewness;
            Double kurt = distribution.kurtosis;
            Double mean = distribution.mean;
            Double stdDev = distribution.standardDeviation;
            int n = distribution.n;

            // This check can only be performed on distributions with a standard
            // deviation that is not zero
            if (stdDev != 0)
            {
                // Calculate the z-scores of skewness and kurtosis
                Double zSkew = (skew - mean) / stdDev;
                Double zKurt = (kurt - mean) / stdDev;

                // Assumption checks are based on the size of n (see explanation
                // above)
                if (n < SMALL_SAMPLE_MAX)
                {
                    if ((zSkew >= SMALL_SAMPLE_THRESHOLD)
                            && (zSkew <= -SMALL_SAMPLE_THRESHOLD))
                        normal = false;
                }
                else
                {
                    if ((zSkew >= LARGE_SAMPLE_THRESHOLD)
                            && (zSkew <= -LARGE_SAMPLE_THRESHOLD))
                        normal = false;
                }
            }
            else
            {
                // On a distribution with too few samples the assumptions cannot be
                // checked.
                // On these small distributions the normality is set to false.

                normal = false;
            }

            distribution.normal = normal;

            return distribution;
        }
    }
}