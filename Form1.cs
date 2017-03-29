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
        private Dictionary<Tuple<String, String, String, String>, Distribution> distributionsDictionary = 
            new Dictionary<Tuple<String, String, String, String>, Distribution>();
        private Tuple<String, String, String, String> selectedRecordKey;
        private String defaultOutputDirectory = System.Configuration.ConfigurationManager.AppSettings["defaultOutputDirectory"];
        private String defaultInputDirectory = System.Configuration.ConfigurationManager.AppSettings["defaultInputDirectory"];

        public Form1()
        {
            InitializeComponent();

            //Set the chart area
            chart1.ChartAreas[0].AxisX.Minimum = 1;
            chart2.ChartAreas[0].AxisX.Minimum = 1;
            chart3.ChartAreas[0].AxisX.Minimum = 1;
            chart4.ChartAreas[0].AxisX.Minimum = 1;
            chart5.ChartAreas[0].AxisX.Minimum = 1;

            Color MeanColor = Color.FromArgb(10, 0, 0, 255);
            Color MeanBorderColor = Color.FromArgb(255, 0, 0, 255);
            Color sdhColor = Color.FromArgb(10, 0, 0, 255);
            Color sdhBorderColor = Color.FromArgb(255, 153, 153, 255);
            Color sdlColor = Color.FromArgb(0, 0, 0, 255);
            Color sdlBorderColor = Color.FromArgb(255, 153, 153, 255);
            Color leerlingColor = Color.FromArgb(255, 255, 0, 0);
            Color leerlingBorderColor = Color.FromArgb(255, 255, 0, 0);

            //Set chart colors
            chart1.Series.FindByName("Groepsgemiddelde").Color = MeanColor;
            chart2.Series.FindByName("Groepsgemiddelde").Color = MeanColor;
            chart3.Series.FindByName("Groepsgemiddelde").Color = MeanColor;
            chart4.Series.FindByName("Groepsgemiddelde").Color = MeanColor;
            chart5.Series.FindByName("Groepsgemiddelde").Color = MeanColor;
            chart1.Series.FindByName("Groepsgemiddelde").BorderColor = MeanBorderColor;
            chart2.Series.FindByName("Groepsgemiddelde").BorderColor = MeanBorderColor;
            chart3.Series.FindByName("Groepsgemiddelde").BorderColor = MeanBorderColor;
            chart4.Series.FindByName("Groepsgemiddelde").BorderColor = MeanBorderColor;
            chart5.Series.FindByName("Groepsgemiddelde").BorderColor = MeanBorderColor;

            chart1.Series.FindByName("Groeps deviatie hoog").Color = sdhColor;
            chart2.Series.FindByName("Groeps deviatie hoog").Color = sdhColor;
            chart3.Series.FindByName("Groeps deviatie hoog").Color = sdhColor;
            chart4.Series.FindByName("Groeps deviatie hoog").Color = sdhColor;
            chart5.Series.FindByName("Groeps deviatie hoog").Color = sdhColor;
            chart1.Series.FindByName("Groeps deviatie hoog").BorderColor = sdhBorderColor;
            chart2.Series.FindByName("Groeps deviatie hoog").BorderColor = sdhBorderColor;
            chart3.Series.FindByName("Groeps deviatie hoog").BorderColor = sdhBorderColor;
            chart4.Series.FindByName("Groeps deviatie hoog").BorderColor = sdhBorderColor;
            chart5.Series.FindByName("Groeps deviatie hoog").BorderColor = sdhBorderColor;

            chart1.Series.FindByName("Groeps deviatie laag").Color = sdlColor;
            chart2.Series.FindByName("Groeps deviatie laag").Color = sdlColor;
            chart3.Series.FindByName("Groeps deviatie laag").Color = sdlColor;
            chart4.Series.FindByName("Groeps deviatie laag").Color = sdlColor;
            chart5.Series.FindByName("Groeps deviatie laag").Color = sdlColor;
            chart1.Series.FindByName("Groeps deviatie laag").BorderColor = sdlBorderColor;
            chart2.Series.FindByName("Groeps deviatie laag").BorderColor = sdlBorderColor;
            chart3.Series.FindByName("Groeps deviatie laag").BorderColor = sdlBorderColor;
            chart4.Series.FindByName("Groeps deviatie laag").BorderColor = sdlBorderColor;
            chart5.Series.FindByName("Groeps deviatie laag").BorderColor = sdlBorderColor;

            chart1.Series.FindByName("Leerling score").Color = leerlingColor;
            chart2.Series.FindByName("Leerling score").Color = leerlingColor;
            chart3.Series.FindByName("Leerling score").Color = leerlingColor;
            chart4.Series.FindByName("Leerling score").Color = leerlingColor;
            chart5.Series.FindByName("Leerling score").Color = leerlingColor;
            chart1.Series.FindByName("Leerling score").BorderColor = leerlingBorderColor;
            chart2.Series.FindByName("Leerling score").BorderColor = leerlingBorderColor;
            chart3.Series.FindByName("Leerling score").BorderColor = leerlingBorderColor;
            chart4.Series.FindByName("Leerling score").BorderColor = leerlingBorderColor;
            chart5.Series.FindByName("Leerling score").BorderColor = leerlingBorderColor;

        }

        private String openCSVFileDialog()
        {
            //Prep the dialog
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = defaultInputDirectory; //For debugging
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
            openFileDialog1.InitialDirectory = defaultInputDirectory; //For debugging
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
                Tuple<String, String, String, String> key = new Tuple<String, String, String, String>("closing",item.Item2,item.Item3,item.Item4);
                if (!distributionsDictionary.TryGetValue(key, out tempDist))
                {
                    tempDist = new Distribution();
                }
                tempDist = UpdateDistribution(tempDist, records[item].Score1);
                distributionsDictionary.Remove(key);
                distributionsDictionary.Add(key, tempDist);

                // Update empathy
                key = new Tuple<String, String, String, String>("empathy", item.Item2, item.Item3, item.Item4);
                if (!distributionsDictionary.TryGetValue(key, out tempDist))
                {
                    tempDist = new Distribution();
                }
                tempDist = UpdateDistribution(tempDist, records[item].Score2);
                distributionsDictionary.Remove(key);
                distributionsDictionary.Add(key, tempDist);

                // Update find in db
                tempDist = new Distribution();
                key = new Tuple<String, String, String, String>("findindb", item.Item2, item.Item3, item.Item4);
                if (!distributionsDictionary.TryGetValue(key, out tempDist))
                {
                    tempDist = new Distribution();
                }
                tempDist = UpdateDistribution(tempDist, records[item].Score3);
                distributionsDictionary.Remove(key);
                distributionsDictionary.Add(key, tempDist);

                // Update inquire
                tempDist = new Distribution();
                key = new Tuple<String, String, String, String>("inquire", item.Item2, item.Item3, item.Item4);
                if (!distributionsDictionary.TryGetValue(key, out tempDist))
                {
                    tempDist = new Distribution();
                }
                tempDist = UpdateDistribution(tempDist, records[item].Score4);
                distributionsDictionary.Remove(key);
                distributionsDictionary.Add(key, tempDist);

                // Update polite
                tempDist = new Distribution();
                key = new Tuple<String, String, String, String>("polite", item.Item2, item.Item3, item.Item4);
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
            //Advies is nu niet relatief aan groepsscores

            //Generate advice for student based on his performance and the group performance
            String advice = "";
            int performance;

            switch (targetSkill)
            {
                case 1:
                    {
                        //Slecht resultaat
                        performance = 1;

                        if (studentPerformance.Score1 >= (distribution.mean - distribution.standardDeviation))
                        {
                            //Onder gemiddeld resultaat
                            performance = 2;

                            if (studentPerformance.Score1 >= distribution.mean)
                            {
                                //Goed resultaat
                                performance = 3;

                                if (studentPerformance.Score1 >= (distribution.mean + distribution.standardDeviation))
                                {
                                    //Best mogelijke resultaat
                                    performance = 4;
                                }
                            }
                        }

                        advice = "Afsluiten: ";

                        switch (performance)
                        {
                            case 1:
                                {
                                    //Low performance
                                    advice += "op het gebied van gesprekken afsluiten doe je het nu onder het gemiddelde van je klas. Waarschijnlijk kun je je communicatie op dit onderdeel nog verbeteren. Probeer vriendelijk te blijven en te kijken of je de klant verder nog kunt helpen.";
                                }; break;
                            case 2:
                                {
                                    //Average/low performance
                                    advice += "op het gebied van gesprekken afsluiten doe je het nu net iets onder het gemiddelde van je klas. Hou dit vol.";
                                }; break;
                            case 3:
                                {
                                    //Average/high performance
                                    advice += "op het gebied van gesprekken afsluiten doe je het nu gelijk aan of beter dan het gemiddelde van je klas. Dit is heel netjes.";
                                }; break;
                            case 4:
                                {
                                    //High performance
                                    advice += "op het gebied van gesprekken afsluiten ben je nu een van de besten van de klas. Uitstekend!";
                                }; break;
                        }
                    }; break;
                case 2:
                    {
                        //Empathy
                        //Slecht resultaat
                        performance = 1;

                        if (studentPerformance.Score2 >= (distribution.mean - distribution.standardDeviation))
                        {
                            //Onder gemiddeld resultaat
                            performance = 2;

                            if (studentPerformance.Score2 >= distribution.mean)
                            {
                                //Goed resultaat
                                performance = 3;

                                if (studentPerformance.Score2 >= (distribution.mean + distribution.standardDeviation))
                                {
                                    //Best mogelijke resultaat
                                    performance = 4;
                                }
                            }
                        }

                        advice = "Invoelen: ";

                        switch (performance)
                        {
                            case 1:
                                {
                                    //Low performance
                                    advice += "op het gebied van invoelen doe je het nu onder het gemiddelde van je klas. Waarschijnlijk kun je je communicatie op dit onderdeel nog verbeteren. Probeer vriendelijk te blijven en begrip te tonen als een klant emotioneel wordt.";
                                }; break;
                            case 2:
                                {
                                    //Average/low performance
                                    advice += "op het gebied van invoelen doe je het nu net iets onder het gemiddelde van je klas. Hou dit vol.";
                                }; break;
                            case 3:
                                {
                                    //Average/high performance
                                    advice += "op het gebied van invoelen doe je het nu gelijk aan of beter dan het gemiddelde van je klas. Dit is heel netjes.";
                                }; break;
                            case 4:
                                {
                                    //High performance
                                    advice += "op het gebied van invoelen ben je nu een van de besten van de klas. Uitstekend!";
                                }; break;
                        }
                    }; break;
                case 3:
                    {
                        //Find in DB
                        //Slecht resultaat
                        performance = 1;

                        if (studentPerformance.Score3 >= (distribution.mean - distribution.standardDeviation))
                        {
                            //Onder gemiddeld resultaat
                            performance = 2;

                            if (studentPerformance.Score3 >= distribution.mean)
                            {
                                //Goed resultaat
                                performance = 3;

                                if (studentPerformance.Score3 >= (distribution.mean + distribution.standardDeviation))
                                {
                                    //Best mogelijke resultaat
                                    performance = 4;
                                }
                            }
                        }

                        advice = "Informatie opzoeken: ";

                        switch (performance)
                        {
                            case 1:
                                {
                                    //Low performance
                                    advice += "op het gebied van informatie opzoeken doe je het nu onder het gemiddelde van je klas. Waarschijnlijk kun je je communicatie op dit onderdeel nog verbeteren. Probeer te zoeken in de modules en let goed op: sommige problemen zijn nog niet bekend! Kun je niet onthouden welk onderdeel het was? Schrijf dan dingen op. Vergeet ook niet om aan de klant te melden dat je de informatie gaat opzoeken.";
                                }; break;
                            case 2:
                                {
                                    //Average/low performance
                                    advice += "op het gebied van informatie opzoeken doe je het nu iets onder als het gemiddelde van je klas. Hou dit vol.";
                                }; break;
                            case 3:
                                {
                                    //Average/high performance
                                    advice += "op het gebied van informatie opzoeken doe je het nu gelijk aan of beter dan het gemiddelde van je klas. Dit is heel netjes.";
                                }; break;
                            case 4:
                                {
                                    //High performance
                                    advice += "op het gebied van informatie opzoeken ben je nu een van de besten van de klas. Uitstekend!";
                                }; break;
                        }
                    }; break;
                case 4:
                    {
                        //Inquire
                        //Slecht resultaat
                        performance = 1;

                        if (studentPerformance.Score4 >= (distribution.mean - distribution.standardDeviation))
                        {
                            //Onder gemiddeld resultaat
                            performance = 2;

                            if (studentPerformance.Score4 >= distribution.mean)
                            {
                                //Goed resultaat
                                performance = 3;

                                if (studentPerformance.Score4 >= (distribution.mean + distribution.standardDeviation))
                                {
                                    //Best mogelijke resultaat
                                    performance = 4;
                                }
                            }
                        }

                        advice = "Vragen stellen: ";

                        switch (performance)
                        {
                            case 1:
                                {
                                    //Low performance
                                    advice += "op het gebied van vragen stellen doe je het nu onder het gemiddelde van je klas. Waarschijnlijk kun je je communicatie op dit onderdeel nog verbeteren. Probeer telkens te kijken of je alles weet wat je nodig hebt om de klant te helpen. Kun je het niet onthouden? Schrijf dan dingen op.";
                                }; break;
                            case 2:
                                {
                                    //Average/low performance
                                    advice += "op het gebied van vragen stellen doe je het nu net iets onder het gemiddelde van je klas. Hou dit vol.";
                                }; break;
                            case 3:
                                {
                                    //Average/high performance
                                    advice += "op het gebied van vragen stellen doe je het nu gelijk aan of beter dan het gemiddelde van je klas. Dit is heel netjes.";
                                }; break;
                            case 4:
                                {
                                    //High performance
                                    advice += "op het gebied van vragen stellen ben je nu een van de besten van de klas. Uitstekend!";
                                }; break;
                        }
                    }; break;
                case 5:
                    {
                        //Polite
                        //Slecht resultaat
                        performance = 1;

                        if (studentPerformance.Score5 >= (distribution.mean - distribution.standardDeviation))
                        {
                            //Onder gemiddeld resultaat
                            performance = 2;

                            if (studentPerformance.Score5 >= distribution.mean)
                            {
                                //Goed resultaat
                                performance = 3;

                                if (studentPerformance.Score5 >= (distribution.mean + distribution.standardDeviation))
                                {
                                    //Best mogelijke resultaat
                                    performance = 4;
                                }
                            }
                        }

                        advice = "Vriendelijkheid: ";

                        switch (performance)
                        {
                            case 1:
                                {
                                    //Low performance
                                    advice += "op het gebied van vriendelijkheid doe je het nu onder het gemiddelde van je klas. Probeer dingen zo netjes mogelijk te zeggen en blijf rustig en beleefd, ook als de klant boos wordt. Vergeet ook niet aan de klant te melden als je tijd nodig hebt om informatie op te zoeken.";
                                }; break;
                            case 2:
                                {
                                    //Average/low performance
                                    advice += "op het gebied van vriendelijkheid doe je het nu net iets onder het gemiddelde van je klas. Hou dit vol.";
                                }; break;
                            case 3:
                                {
                                    //Average/high performance
                                    advice += "op het gebied van vriendelijkheid doe je het nu gelijk aan of beter dan het gemiddelde van je klas. Dit is heel netjes.";
                                }; break;
                            case 4:
                                {
                                    //High performance
                                    advice += "op het gebied van vriendelijkheid ben je nu een van de besten van de klas. Uitstekend!";
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
            if (comboBoxStudent.Items.Count > 0)
            {
                String studentName = comboBoxStudent.SelectedItem.ToString();

                //Generate output for student
                Bitmap closingBmp = new Bitmap(chart1.Width, chart1.Height);
                Bitmap empathyBmp = new Bitmap(chart1.Width, chart1.Height);
                Bitmap findindbBmp = new Bitmap(chart1.Width, chart1.Height);
                Bitmap inquiregBmp = new Bitmap(chart1.Width, chart1.Height);
                Bitmap politeBmp = new Bitmap(chart1.Width, chart1.Height);
                
                //Put the chart in a bitmap
                chart1.DrawToBitmap(closingBmp, new Rectangle(0,0,chart1.Width,chart1.Height));
                chart2.DrawToBitmap(empathyBmp, new Rectangle(0, 0, chart1.Width, chart1.Height));
                chart3.DrawToBitmap(findindbBmp, new Rectangle(0, 0, chart1.Width, chart1.Height));
                chart4.DrawToBitmap(inquiregBmp, new Rectangle(0, 0, chart1.Width, chart1.Height));
                chart5.DrawToBitmap(politeBmp, new Rectangle(0, 0, chart1.Width, chart1.Height));

                //Bitmap into which the final output will go
                Bitmap compoundChart = new Bitmap(closingBmp.Width*3,closingBmp.Height*2);

                using (Graphics g = Graphics.FromImage(compoundChart))
                {
                    g.DrawImage(closingBmp, 0, 0);
                    g.DrawImage(empathyBmp, closingBmp.Width, 0);
                    g.DrawImage(findindbBmp, closingBmp.Width*2, 0);
                    g.DrawImage(inquiregBmp, 0, closingBmp.Height);
                    g.DrawImage(politeBmp, closingBmp.Width, closingBmp.Height);
                }


                //Output the feedback
                compoundChart.Save(defaultOutputDirectory + "\\" + studentName + ".bmp");
            }
        }

        private void fillAdvice(Tuple<String, String, String, String> recordToUse)
        {
            //Opening
            String advice = "Beste leerling. In dit overzicht krijg je uitleg over je prestaties in de Space Modules game. "
                            + "De Space Modules game is bedoeld om je beter te maken in communicatie en "
                            + "je krijgt nu per communicatie onderdeel feedback en advies. "
                            + "Dit advies gaat over de laatste keer dat je speelde dus kijk in de grafieken telkens naar het meest rechtse punt.@@";
            
            Tuple<String, String, String, String> recKey = recordToUse;

            Tuple<String, String, String, String> closingKey = new Tuple<String, String, String, String>("closing", recordToUse.Item2, recordToUse.Item3, recordToUse.Item4);
            Tuple<String, String, String, String> empathyKey = new Tuple<String, String, String, String>("empathy", recordToUse.Item2, recordToUse.Item3, recordToUse.Item4);
            Tuple<String, String, String, String> findindbKey = new Tuple<String, String, String, String>("findindb", recordToUse.Item2, recordToUse.Item3, recordToUse.Item4);
            Tuple<String, String, String, String> inquireKey = new Tuple<String, String, String, String>("inquire", recordToUse.Item2, recordToUse.Item3, recordToUse.Item4);
            Tuple<String, String, String, String> politeKey = new Tuple<String, String, String, String>("polite", recordToUse.Item2, recordToUse.Item3, recordToUse.Item4);

            //Check to see if there are data for this student in this lesson
            Record tempRec = new Record();
            if (records.TryGetValue(recKey, out tempRec))
                advice += generateTextAdvice(distributionsDictionary[closingKey], tempRec, 1) + "@"; //Closing
            else
                advice += "Er zijn voor jou geen scores over afsluiten bekend voor deze les.@";

            tempRec = new Record();
            if (records.TryGetValue(recKey, out tempRec))
                advice += generateTextAdvice(distributionsDictionary[empathyKey], tempRec, 2) + "@"; //Empathy
            else
                advice += "Er zijn voor jou geen scores over invoelen bekend voor deze les.@";

            tempRec = new Record();
            if (records.TryGetValue(recKey, out tempRec))
                advice += generateTextAdvice(distributionsDictionary[findindbKey], tempRec, 3) + "@"; //Find in DB
            else
                advice += "Er zijn voor jou geen scores over opzoeken bekend voor deze les.@";

            tempRec = new Record();
            if (records.TryGetValue(recKey, out tempRec))
                advice += generateTextAdvice(distributionsDictionary[inquireKey], tempRec, 4) + "@"; //Inquiry
            else
                advice += "Er zijn voor jou geen scores over vragen stellen bekend voor deze les.@";

            tempRec = new Record();
            if (records.TryGetValue(recKey, out tempRec))
                advice += generateTextAdvice(distributionsDictionary[politeKey], tempRec, 5) + "@@"; //Politeness
            else
                advice += "Er zijn voor jou geen scores over vriendelijkheid bekend voor deze les.@@";
            
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
            //Set keys
            Tuple<String, String, String, String> closingKey = new Tuple<String, String, String, String>("closing",recordToUse.Item2, recordToUse.Item3, recordToUse.Item4);
            Tuple<String, String, String, String> empathyKey = new Tuple<String, String, String, String>("empathy", recordToUse.Item2, recordToUse.Item3, recordToUse.Item4);
            Tuple<String, String, String, String> findindbKey = new Tuple<String, String, String, String>("findindb", recordToUse.Item2, recordToUse.Item3, recordToUse.Item4);
            Tuple<String, String, String, String> inquireKey = new Tuple<String, String, String, String>("inquire", recordToUse.Item2, recordToUse.Item3, recordToUse.Item4);
            Tuple<String, String, String, String> politeKey = new Tuple<String, String, String, String>("polite", recordToUse.Item2, recordToUse.Item3, recordToUse.Item4);

            //Get mean series from charts
            Series closingMean = chart1.Series.FindByName("Groepsgemiddelde");
            Series empathyMean = chart2.Series.FindByName("Groepsgemiddelde");
            Series findindbMean = chart3.Series.FindByName("Groepsgemiddelde");
            Series inquireMean = chart4.Series.FindByName("Groepsgemiddelde");
            Series politeMean = chart5.Series.FindByName("Groepsgemiddelde");

            //Clear points in series
            closingMean.Points.Clear();
            empathyMean.Points.Clear();
            findindbMean.Points.Clear();
            inquireMean.Points.Clear();
            politeMean.Points.Clear();

            //Get standard deviation series from charts
            Series closingSDH = chart1.Series.FindByName("Groeps deviatie hoog");
            Series empathySDH = chart2.Series.FindByName("Groeps deviatie hoog");
            Series findindbSDH = chart3.Series.FindByName("Groeps deviatie hoog");
            Series inquireSDH = chart4.Series.FindByName("Groeps deviatie hoog");
            Series politeSDH = chart5.Series.FindByName("Groeps deviatie hoog");

            //Clear points in series
            closingSDH.Points.Clear();
            empathySDH.Points.Clear();
            findindbSDH.Points.Clear();
            inquireSDH.Points.Clear();
            politeSDH.Points.Clear();

            //Get standard deviation series from charts
            Series closingSDL = chart1.Series.FindByName("Groeps deviatie laag");
            Series empathySDL = chart2.Series.FindByName("Groeps deviatie laag");
            Series findindbSDL = chart3.Series.FindByName("Groeps deviatie laag");
            Series inquireSDL = chart4.Series.FindByName("Groeps deviatie laag");
            Series politeSDL = chart5.Series.FindByName("Groeps deviatie laag");

            //Clear points in series
            closingSDL.Points.Clear();
            empathySDL.Points.Clear();
            findindbSDL.Points.Clear();
            inquireSDL.Points.Clear();
            politeSDL.Points.Clear();

            //Add group values to charts
            //Closing
            Dictionary<Tuple<String, String, String, String>, Distribution> tempDict = new Dictionary<Tuple<String, String, String, String>, Distribution>();
            foreach (Tuple<String, String, String, String> key in distributionsDictionary.Keys)
            {
                if (key.Item1 == closingKey.Item1 && 
                    key.Item2 == closingKey.Item2 &&
                    key.Item3 == closingKey.Item3)
                    tempDict.Add(key, distributionsDictionary[key]);
                    
            }
            for (int i = 1; i <= tempDict.Count; i++)
            {
                Tuple<String, String, String, String> tempKey = new Tuple<String, String, String, String>(
                    closingKey.Item1, closingKey.Item2, closingKey.Item3, i.ToString());

                closingSDL.Points.AddXY(i, tempDict[tempKey].mean - tempDict[tempKey].standardDeviation);
                closingMean.Points.AddXY(i, tempDict[tempKey].standardDeviation);
                closingSDH.Points.AddXY(i, tempDict[tempKey].standardDeviation);

                //This code works with linecharts but not with stacked-areacharts
                //closingMean.Points.AddXY(i,tempDict[tempKey].mean);
                //closingSDH.Points.AddXY(i,tempDict[tempKey].mean + tempDict[tempKey].standardDeviation);
                //closingSDL.Points.AddXY(i,tempDict[tempKey].mean - tempDict[tempKey].standardDeviation);
            }

            //Empathy
            tempDict.Clear();
            foreach (Tuple<String, String, String, String> key in distributionsDictionary.Keys)
            {
                if (key.Item1 == empathyKey.Item1 &&
                    key.Item2 == empathyKey.Item2 &&
                    key.Item3 == empathyKey.Item3)
                    tempDict.Add(key, distributionsDictionary[key]);

            }
            for (int i = 1; i <= tempDict.Count; i++)
            {
                Tuple<String, String, String, String> tempKey = new Tuple<String, String, String, String>(
                    empathyKey.Item1, empathyKey.Item2, empathyKey.Item3, i.ToString());

                empathySDL.Points.AddXY(i, tempDict[tempKey].mean - tempDict[tempKey].standardDeviation);
                empathyMean.Points.AddXY(i, tempDict[tempKey].standardDeviation);
                empathySDH.Points.AddXY(i, tempDict[tempKey].standardDeviation);

                //empathyMean.Points.AddXY(i,tempDict[tempKey].mean);
                //empathySDH.Points.AddXY(i,tempDict[tempKey].mean + tempDict[tempKey].standardDeviation);
                //empathySDL.Points.AddXY(i,tempDict[tempKey].mean - tempDict[tempKey].standardDeviation);
            }

            //Find in db
            tempDict.Clear();
            foreach (Tuple<String, String, String, String> key in distributionsDictionary.Keys)
            {
                if (key.Item1 == findindbKey.Item1 &&
                    key.Item2 == findindbKey.Item2 &&
                    key.Item3 == findindbKey.Item3)
                    tempDict.Add(key, distributionsDictionary[key]);

            }
            for (int i = 1; i <= tempDict.Count; i++)
            {
                Tuple<String, String, String, String> tempKey = new Tuple<String, String, String, String>(
                    findindbKey.Item1, findindbKey.Item2, findindbKey.Item3, i.ToString());

                findindbSDL.Points.AddXY(i, tempDict[tempKey].mean - tempDict[tempKey].standardDeviation);
                findindbMean.Points.AddXY(i, tempDict[tempKey].standardDeviation);
                findindbSDH.Points.AddXY(i, tempDict[tempKey].standardDeviation);

                //findindbMean.Points.AddXY(i,tempDict[tempKey].mean);
                //findindbSDH.Points.AddXY(i,tempDict[tempKey].mean + tempDict[tempKey].standardDeviation);
                //findindbSDL.Points.AddXY(i,tempDict[tempKey].mean - tempDict[tempKey].standardDeviation);
            }
            
            //Inquire
            tempDict.Clear();
            foreach (Tuple<String, String, String, String> key in distributionsDictionary.Keys)
            {
                if (key.Item1 == inquireKey.Item1 &&
                    key.Item2 == inquireKey.Item2 &&
                    key.Item3 == inquireKey.Item3)
                    tempDict.Add(key, distributionsDictionary[key]);

            }
            for (int i = 1; i <= tempDict.Count; i++)
            {
                Tuple<String, String, String, String> tempKey = new Tuple<String, String, String, String>(
                    inquireKey.Item1, inquireKey.Item2, inquireKey.Item3, i.ToString());

                inquireSDL.Points.AddXY(i, tempDict[tempKey].mean - tempDict[tempKey].standardDeviation);
                inquireMean.Points.AddXY(i, tempDict[tempKey].standardDeviation);
                inquireSDH.Points.AddXY(i, tempDict[tempKey].standardDeviation);

                //inquireMean.Points.AddXY(i,tempDict[tempKey].mean);
                //inquireSDH.Points.AddXY(i,tempDict[tempKey].mean + tempDict[tempKey].standardDeviation);
                //inquireSDL.Points.AddXY(i,tempDict[tempKey].mean - tempDict[tempKey].standardDeviation);
            }

            //Polite
            tempDict.Clear();
            foreach (Tuple<String, String, String, String> key in distributionsDictionary.Keys)
            {
                if (key.Item1 == politeKey.Item1 &&
                    key.Item2 == politeKey.Item2 &&
                    key.Item3 == politeKey.Item3)
                    tempDict.Add(key, distributionsDictionary[key]);

            }
            for (int i = 1; i <= tempDict.Count; i++)
            {
                Tuple<String, String, String, String> tempKey = new Tuple<String, String, String, String>(
                    politeKey.Item1, politeKey.Item2, politeKey.Item3, i.ToString());

                politeSDL.Points.AddXY(i, tempDict[tempKey].mean - tempDict[tempKey].standardDeviation);
                politeMean.Points.AddXY(i, tempDict[tempKey].standardDeviation);
                politeSDH.Points.AddXY(i, tempDict[tempKey].standardDeviation);

                //politeMean.Points.AddXY(i,tempDict[tempKey].mean);
                //politeSDH.Points.AddXY(i,tempDict[tempKey].mean + tempDict[tempKey].standardDeviation);
                //politeSDL.Points.AddXY(i,tempDict[tempKey].mean - tempDict[tempKey].standardDeviation);
            }

            //Adding player scores charts

            //Get standard deviation series from charts
            Series closingPlayer = chart1.Series.FindByName("Leerling score");
            Series empathyPlayer = chart2.Series.FindByName("Leerling score");
            Series findindbPlayer = chart3.Series.FindByName("Leerling score");
            Series inquirePlayer = chart4.Series.FindByName("Leerling score");
            Series politePlayer = chart5.Series.FindByName("Leerling score");

            //Clear points in series
            closingPlayer.Points.Clear();
            empathyPlayer.Points.Clear();
            findindbPlayer.Points.Clear();
            inquirePlayer.Points.Clear();
            politePlayer.Points.Clear();

            Dictionary<Tuple<String, String, String, String>, Record> tempRecs = new Dictionary<Tuple<String, String, String, String>, Record>();
            foreach (Tuple<String, String, String, String> key in records.Keys)
            {
                if (key.Item1 == recordToUse.Item1 &&
                    key.Item2 == recordToUse.Item2 &&
                    key.Item3 == recordToUse.Item3)
                {
                    tempRecs.Add(key,records[key]);
                }
            }
            for (int i = 1; i <= tempRecs.Count; i++)
            {
                Tuple<String, String, String, String> tempKey = new Tuple<String, String, String, String>(
                    recordToUse.Item1, recordToUse.Item2, recordToUse.Item3, i.ToString());

                closingPlayer.Points.AddXY(i,tempRecs[tempKey].Score1);
                empathyPlayer.Points.AddXY(i,tempRecs[tempKey].Score2);
                findindbPlayer.Points.AddXY(i,tempRecs[tempKey].Score3);
                inquirePlayer.Points.AddXY(i,tempRecs[tempKey].Score4);
                politePlayer.Points.AddXY(i,tempRecs[tempKey].Score5);
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
            //comboBoxStudent.Items.Clear();

            //foreach (Tuple<String, String, String, String> item in records.Keys)
            //{
            //    if (comboBoxROC.SelectedItem.ToString() == item.Item2 && comboBoxGroep.SelectedItem.ToString() == item.Item3 && comboBoxLes.SelectedItem.ToString() == item.Item4)
            //        if (!comboBoxStudent.Items.Contains(item.Item1))
            //            comboBoxStudent.Items.Add(item.Item1);
            //}

            //if (comboBoxStudent.Items.Count > 0)
            //    comboBoxStudent.SelectedIndex = 0;

            UpdateFeedback();
        }

        private void comboBoxStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFeedback();
        }

        private void UpdateFeedback()
        {
            //Een student is geselecteerd, vul alle grafieken, tabellen en genereer advies

            if (comboBoxStudent.Items.Count > 0)
            {
                selectedRecordKey = new Tuple<String, String, String, String>(
                    comboBoxStudent.SelectedItem.ToString(),
                    comboBoxROC.SelectedItem.ToString(),
                    comboBoxGroep.SelectedItem.ToString(),
                    comboBoxLes.SelectedItem.ToString());

                fillCharts(selectedRecordKey);
                fillTable(selectedRecordKey);
                fillAdvice(selectedRecordKey);
            }

            
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

        private void button3_Click(object sender, EventArgs e)
        {
            //Itereer door alle studenten heen voor een gegeven ROC, groep en les
        }
    }
}