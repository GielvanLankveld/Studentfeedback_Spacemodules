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
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace StudentFeedback_SpaceModules
{
    public partial class Form1 : Form
    {
        /*
         * Closing
         * Empathy
         * Find in DB
         * Inquire
         * Polite
         */
        private Dictionary<Tuple<String,String,String,String>, Record> records;
        private Dictionary<Tuple<String, String, String, String>, Distribution> distributionsDictionary = 
            new Dictionary<Tuple<String, String, String, String>, Distribution>();
        //private Tuple<String, String, String, String> selectedRecordKey;
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

            chart1.Series.FindByName("Leerling score").MarkerStyle = MarkerStyle.Circle;
            chart2.Series.FindByName("Leerling score").MarkerStyle = MarkerStyle.Circle;
            chart3.Series.FindByName("Leerling score").MarkerStyle = MarkerStyle.Circle;
            chart4.Series.FindByName("Leerling score").MarkerStyle = MarkerStyle.Circle;
            chart5.Series.FindByName("Leerling score").MarkerStyle = MarkerStyle.Circle;

            chart1.Series.FindByName("Leerling score").MarkerColor = leerlingColor;
            chart2.Series.FindByName("Leerling score").MarkerColor = leerlingColor;
            chart3.Series.FindByName("Leerling score").MarkerColor = leerlingColor;
            chart4.Series.FindByName("Leerling score").MarkerColor = leerlingColor;
            chart5.Series.FindByName("Leerling score").MarkerColor = leerlingColor;

            int size = 3;
            chart1.Series.FindByName("Leerling score").MarkerSize = size;
            chart2.Series.FindByName("Leerling score").MarkerSize = size;
            chart3.Series.FindByName("Leerling score").MarkerSize = size;
            chart4.Series.FindByName("Leerling score").MarkerSize = size;
            chart5.Series.FindByName("Leerling score").MarkerSize = size;

            

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

            //Extract the column labels from the datafile
            string[] labels = lines[0].Split(stringSeparators, StringSplitOptions.None);
            Dictionary<string, int> labelList = new Dictionary<string, int>();

            for (var i = 0; i < labels.Length; i++)
            {
                labelList.Add(labels[i], i);
            }

            //Parse the file
            for (var i = 2; i < lines.Length; i++)
            {
                /*
                 * Closing
                 * Empathy
                 * Find in DB
                 * Inquire
                 * Polite
                 */
                string[] items = lines[i].Split(stringSeparators, StringSplitOptions.None);
                Record tempRec = new Record();
                if (int.TryParse(items[labelList["Closing"]], out tempRec.Score1) != true) tempRec.Score1 = 0;
                if (int.TryParse(items[labelList["Empathy"]], out tempRec.Score2) != true) tempRec.Score2 = 0;
                if (int.TryParse(items[labelList["Find in DB"]], out tempRec.Score3) != true) tempRec.Score3 = 0;
                if (int.TryParse(items[labelList["Inquire"]], out tempRec.Score4) != true) tempRec.Score4 = 0;
                if (int.TryParse(items[labelList["Polite"]], out tempRec.Score5) != true) tempRec.Score5 = 0;
                tempRec.email = items[labelList["StudentID"]];

                Tuple<String, String, String, String> key = new Tuple<String, String, String, String>(items[labelList["ROC"]], items[labelList["Groep"]], items[labelList["Sessie"]], items[labelList["Naam"]].ToString());
                if (!inputList.ContainsKey(key) && key.Item1 != "" && key.Item2 != "" && key.Item3 != "" && key.Item4 != "")
                {
                    inputList.Add(key,tempRec);
                }
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
                Tuple<String, String, String, String> key = new Tuple<String, String, String, String>("Closing",item.Item1,item.Item2,item.Item3);
                if (!distributionsDictionary.TryGetValue(key, out tempDist))
                {
                    tempDist = new Distribution();
                }
                tempDist = UpdateDistribution(tempDist, records[item].Score1);
                distributionsDictionary.Remove(key);
                distributionsDictionary.Add(key, tempDist);

                // Update empathy
                key = new Tuple<String, String, String, String>("Empathy", item.Item1, item.Item2, item.Item3);
                if (!distributionsDictionary.TryGetValue(key, out tempDist))
                {
                    tempDist = new Distribution();
                }
                tempDist = UpdateDistribution(tempDist, records[item].Score2);
                distributionsDictionary.Remove(key);
                distributionsDictionary.Add(key, tempDist);

                // Update find in db
                tempDist = new Distribution();
                key = new Tuple<String, String, String, String>("Find in DB", item.Item1, item.Item2, item.Item3);
                if (!distributionsDictionary.TryGetValue(key, out tempDist))
                {
                    tempDist = new Distribution();
                }
                tempDist = UpdateDistribution(tempDist, records[item].Score3);
                distributionsDictionary.Remove(key);
                distributionsDictionary.Add(key, tempDist);

                // Update inquire
                tempDist = new Distribution();
                key = new Tuple<String, String, String, String>("Inquire", item.Item1, item.Item2, item.Item3);
                if (!distributionsDictionary.TryGetValue(key, out tempDist))
                {
                    tempDist = new Distribution();
                }
                tempDist = UpdateDistribution(tempDist, records[item].Score4);
                distributionsDictionary.Remove(key);
                distributionsDictionary.Add(key, tempDist);

                // Update polite
                tempDist = new Distribution();
                key = new Tuple<String, String, String, String>("Polite", item.Item1, item.Item2, item.Item3);
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

                        advice = "Opzoeken: ";

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

                        advice = "Doorvragen: ";

                        switch (performance)
                        {
                            case 1:
                                {
                                    //Low performance
                                    advice += "op het gebied van doorvragen doe je het nu onder het gemiddelde van je klas. Waarschijnlijk kun je je communicatie op dit onderdeel nog verbeteren. Probeer telkens te kijken of je alles weet wat je nodig hebt om de klant te helpen. Kun je het niet onthouden? Schrijf dan dingen op.";
                                }; break;
                            case 2:
                                {
                                    //Average/low performance
                                    advice += "op het gebied van doorvragen doe je het nu net iets onder het gemiddelde van je klas. Hou dit vol.";
                                }; break;
                            case 3:
                                {
                                    //Average/high performance
                                    advice += "op het gebied van doorvragen doe je het nu gelijk aan of beter dan het gemiddelde van je klas. Dit is heel netjes.";
                                }; break;
                            case 4:
                                {
                                    //High performance
                                    advice += "op het gebied van doorvragen ben je nu een van de besten van de klas. Uitstekend!";
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

            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;

            //Fill the variables
            foreach (Tuple<String, String, String, String> item in records.Keys)
            {
                if (!comboBoxROC.Items.Contains(item.Item1))
                    comboBoxROC.Items.Add(item.Item1);
            }

            if (comboBoxROC.Items.Count > 0)
            {
                comboBoxROC.SelectedIndex = 0;

                foreach (Tuple<String, String, String, String> item in records.Keys)
                {
                    if (comboBoxROC.SelectedItem.ToString() == item.Item1) 
                        if (!comboBoxGroep.Items.Contains(item.Item2))
                            comboBoxGroep.Items.Add(item.Item2);
                }

                if (comboBoxGroep.Items.Count > 0)
                {
                    comboBoxGroep.SelectedIndex = 0;

                    foreach (Tuple<String, String, String, String> item in records.Keys)
                    {
                        if (comboBoxROC.SelectedItem.ToString() == item.Item1 && comboBoxGroep.SelectedItem.ToString() == item.Item2)
                            if (!comboBoxLes.Items.Contains(item.Item3))
                                comboBoxLes.Items.Add(item.Item3);
                    }

                    if (comboBoxLes.Items.Count > 0)
                    {
                        comboBoxLes.SelectedIndex = 0;

                        foreach (Tuple<String, String, String, String> item in records.Keys)
                        {
                            if (comboBoxROC.SelectedItem.ToString() == item.Item1 && comboBoxGroep.SelectedItem.ToString() == item.Item2 && comboBoxLes.SelectedItem.ToString() == item.Item3)
                                if (!comboBoxStudent.Items.Contains(item.Item4))
                                    comboBoxStudent.Items.Add(item.Item4);
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
                //Pick student to output
                Tuple<String, String, String, String> studentKey = new Tuple<String, String, String, String>(
                    comboBoxROC.SelectedItem.ToString(),
                    comboBoxGroep.SelectedItem.ToString(),
                    comboBoxLes.SelectedItem.ToString(),
                    comboBoxStudent.SelectedItem.ToString());

                //Request output
                GenerateOutputToGraphic(studentKey);

                //Report completed
                MessageBox.Show("Output is gemaakt");
            }
            else
            {
                MessageBox.Show("Output kon niet worden gemaakt omdat er geen student geselecteerd is.");
            }
        }

        private void GenerateOutputToGraphic(Tuple<String, String, String, String> key)
        {
            String studentName = key.Item4;
            Record scores;
            if (records.TryGetValue(key, out scores))
            {
                string scoresHeader = "Les\t\tAfsluiten\t\tInvoelen\t\tOpzoeken\tDoorvragen\tVriendelijkheid";
                string scoreString = key.Item3.ToString() + "\t\t" + scores.Score1 + "\t\t" + scores.Score2
                    + "\t\t" + scores.Score3 + "\t\t" + scores.Score4 + "\t\t" + scores.Score5;

                String advice = richTextBox1.Text;

                //Generate output for student
                Bitmap closingBmp = new Bitmap(chart1.Width, chart1.Height);
                Bitmap empathyBmp = new Bitmap(chart1.Width, chart1.Height);
                Bitmap findindbBmp = new Bitmap(chart1.Width, chart1.Height);
                Bitmap inquiregBmp = new Bitmap(chart1.Width, chart1.Height);
                Bitmap politeBmp = new Bitmap(chart1.Width, chart1.Height);

                //Put the chart in a bitmap
                chart1.DrawToBitmap(closingBmp, new Rectangle(0, 0, chart1.Width, chart1.Height));
                chart2.DrawToBitmap(empathyBmp, new Rectangle(0, 0, chart1.Width, chart1.Height));
                chart3.DrawToBitmap(findindbBmp, new Rectangle(0, 0, chart1.Width, chart1.Height));
                chart4.DrawToBitmap(inquiregBmp, new Rectangle(0, 0, chart1.Width, chart1.Height));
                chart5.DrawToBitmap(politeBmp, new Rectangle(0, 0, chart1.Width, chart1.Height));

                //Bitmap into which the final output will go
                Bitmap compoundChart = new Bitmap(closingBmp.Width * 3, closingBmp.Height * 5);

                //Set spacings between elements of the output
                int spacing0 = 32;
                int spacing1 = spacing0 + 30;                           //Starting y of graphs
                int spacing2 = spacing1 + (2 * closingBmp.Height) + 12;   //Starting y of tables
                int spacing3 = spacing2 + 18;                           //Distance between table title and labels
                int spacing4 = spacing3 + 20;                           //Distance between labels and values in table
                int spacing5 = spacing4 + 12 + 20;                      //Starting y of advice title
                int spacing6 = spacing5 + 20;                           //Starting y of advice text

                using (Graphics g = Graphics.FromImage(compoundChart))
                {
                    //Make the entire background white
                    using (SolidBrush sBrush = new SolidBrush(Color.FromArgb(255, 255, 255)))
                    {
                        g.FillRectangle(sBrush, 0, 0, compoundChart.Width, compoundChart.Height);
                    }

                    //Write the student name
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    Brush brush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
                    g.DrawString(studentName, this.Font, brush, new RectangleF((compoundChart.Width / 2) - 50, 12, compoundChart.Width - 40, 12));

                    //Draw the graphs
                    g.DrawString("De onderstaande grafieken geven een overzicht van hoe jij het deed ten opzichte van de rest van de klas. "
                        + "De rode lijn ben jij en de donkerblauwe lijn is het gemiddelde van de klas. "
                        + "Alle scores die binnen de lichtblauwe lijnen vallen zitten nog binnen het gemiddelde van de klas.",
                        this.Font, brush, new RectangleF(20, spacing0, compoundChart.Width - 40, 30));
                    g.DrawImage(closingBmp, 0, spacing1);
                    g.DrawImage(empathyBmp, closingBmp.Width, spacing1);
                    g.DrawImage(findindbBmp, closingBmp.Width * 2, spacing1);
                    g.DrawImage(inquiregBmp, 0, closingBmp.Height + spacing1 + 20);
                    g.DrawImage(politeBmp, closingBmp.Width, closingBmp.Height + spacing1 + 20);

                    //Draw the scores table
                    g.DrawString("Jouw scores", this.Font, brush, new RectangleF(20, spacing2, compoundChart.Width - 40, 12));
                    g.DrawString(scoresHeader, this.Font, brush, new RectangleF(20, spacing3, compoundChart.Width - 40, 12));
                    g.DrawString(scoreString, this.Font, brush, new RectangleF(20, spacing4, compoundChart.Width - 40, 12));

                    //Draw the advice
                    g.DrawString("Jouw advies", this.Font, brush, new RectangleF(20, spacing5, compoundChart.Width - 40, 12));
                    g.DrawString(advice, this.Font, brush, new Rectangle(20, spacing6, compoundChart.Width - 40, 400));
                }
                
                //Output the feedback
                System.IO.Directory.CreateDirectory(defaultOutputDirectory + "\\" + key.Item1 + "_" + key.Item2 + "_" + key.Item3);
                compoundChart.Save(defaultOutputDirectory + "\\" + key.Item1 + "_" + key.Item2 + "_" + key.Item3 + "\\" + records[key].email + "_" + studentName + ".bmp");
            }
        }

        private void fillAdvice(Tuple<String, String, String, String> recordToUse)
        {
            //Opening
            String advice = "Beste leerling. In dit overzicht krijg je uitleg over je prestaties in de Space Modules game. "
                            + "De Space Modules game is bedoeld om je beter te maken in communicatie en "
                            + "je krijgt nu per communicatie onderdeel feedback en advies. "
                            + "Dit advies gaat over een geselecteerde les dus kijk in de grafieken op de plek "
                            + "die bij de les hoort als je wilt zien hoe je het ten opzichte van de groep doet.@@";
            
            Tuple<String, String, String, String> recKey = recordToUse;

            Tuple<String, String, String, String> closingKey = new Tuple<String, String, String, String>("Closing", recordToUse.Item1, recordToUse.Item2, recordToUse.Item3);
            Tuple<String, String, String, String> empathyKey = new Tuple<String, String, String, String>("Empathy", recordToUse.Item1, recordToUse.Item2, recordToUse.Item3);
            Tuple<String, String, String, String> findindbKey = new Tuple<String, String, String, String>("Find in DB", recordToUse.Item1, recordToUse.Item2, recordToUse.Item3);
            Tuple<String, String, String, String> inquireKey = new Tuple<String, String, String, String>("Inquire", recordToUse.Item1, recordToUse.Item2, recordToUse.Item3);
            Tuple<String, String, String, String> politeKey = new Tuple<String, String, String, String>("Polite", recordToUse.Item1, recordToUse.Item2, recordToUse.Item3);

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
                    dataGridView2.Rows.Add(record.Item1,record.Item2,record.Item3,records[record].Score1, records[record].Score2, records[record].Score3, records[record].Score4, records[record].Score5);
                }
            }
        }

        private void fillCharts(Tuple<String, String, String, String> recordToUse)
        {
            //Set keys
            Tuple<String, String, String, String> closingKey = new Tuple<String, String, String, String>("Closing", recordToUse.Item1, recordToUse.Item2, recordToUse.Item3);
            Tuple<String, String, String, String> empathyKey = new Tuple<String, String, String, String>("Empathy", recordToUse.Item1, recordToUse.Item2, recordToUse.Item3);
            Tuple<String, String, String, String> findindbKey = new Tuple<String, String, String, String>("Find in DB", recordToUse.Item1, recordToUse.Item2, recordToUse.Item3);
            Tuple<String, String, String, String> inquireKey = new Tuple<String, String, String, String>("Inquire", recordToUse.Item1, recordToUse.Item2, recordToUse.Item3);
            Tuple<String, String, String, String> politeKey = new Tuple<String, String, String, String>("Polite", recordToUse.Item1, recordToUse.Item2, recordToUse.Item3);

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
                {
                    closingSDL.Points.AddXY(key.Item4, distributionsDictionary[key].mean - distributionsDictionary[key].standardDeviation);
                    closingMean.Points.AddXY(key.Item4, distributionsDictionary[key].standardDeviation);
                    closingSDH.Points.AddXY(key.Item4, distributionsDictionary[key].standardDeviation);
                }
            }

            //Empathy
            tempDict.Clear();
            foreach (Tuple<String, String, String, String> key in distributionsDictionary.Keys)
            {
                if (key.Item1 == empathyKey.Item1 &&
                    key.Item2 == empathyKey.Item2 &&
                    key.Item3 == empathyKey.Item3)
                {
                    empathySDL.Points.AddXY(key.Item4, distributionsDictionary[key].mean - distributionsDictionary[key].standardDeviation);
                    empathyMean.Points.AddXY(key.Item4, distributionsDictionary[key].standardDeviation);
                    empathySDH.Points.AddXY(key.Item4, distributionsDictionary[key].standardDeviation);
                }
            }

            //Find in db
            tempDict.Clear();
            foreach (Tuple<String, String, String, String> key in distributionsDictionary.Keys)
            {
                if (key.Item1 == findindbKey.Item1 &&
                    key.Item2 == findindbKey.Item2 &&
                    key.Item3 == findindbKey.Item3)
                {
                    findindbSDL.Points.AddXY(key.Item4, distributionsDictionary[key].mean - distributionsDictionary[key].standardDeviation);
                    findindbMean.Points.AddXY(key.Item4, distributionsDictionary[key].standardDeviation);
                    findindbSDH.Points.AddXY(key.Item4, distributionsDictionary[key].standardDeviation);
                }
            }
            
            //Inquire
            tempDict.Clear();
            foreach (Tuple<String, String, String, String> key in distributionsDictionary.Keys)
            {
                if (key.Item1 == inquireKey.Item1 &&
                    key.Item2 == inquireKey.Item2 &&
                    key.Item3 == inquireKey.Item3)
                {
                    inquireSDL.Points.AddXY(key.Item4, distributionsDictionary[key].mean - distributionsDictionary[key].standardDeviation);
                    inquireMean.Points.AddXY(key.Item4, distributionsDictionary[key].standardDeviation);
                    inquireSDH.Points.AddXY(key.Item4, distributionsDictionary[key].standardDeviation);
                }
            }

            //Polite
            tempDict.Clear();
            foreach (Tuple<String, String, String, String> key in distributionsDictionary.Keys)
            {
                if (key.Item1 == politeKey.Item1 &&
                    key.Item2 == politeKey.Item2 &&
                    key.Item3 == politeKey.Item3)
                {
                    politeSDL.Points.AddXY(key.Item4, distributionsDictionary[key].mean - distributionsDictionary[key].standardDeviation);
                    politeMean.Points.AddXY(key.Item4, distributionsDictionary[key].standardDeviation);
                    politeSDH.Points.AddXY(key.Item4, distributionsDictionary[key].standardDeviation);
                }
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
            
            foreach (Tuple<String, String, String, String> key in records.Keys)
            {
                if (key.Item1 == recordToUse.Item1 &&
                    key.Item2 == recordToUse.Item2 &&
                    key.Item4 == recordToUse.Item4)
                {
                    closingPlayer.Points.AddXY(key.Item3, records[key].Score1);
                    empathyPlayer.Points.AddXY(key.Item3, records[key].Score2);
                    findindbPlayer.Points.AddXY(key.Item3, records[key].Score3);
                    inquirePlayer.Points.AddXY(key.Item3, records[key].Score4);
                    politePlayer.Points.AddXY(key.Item3, records[key].Score5);
                }
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
                if (comboBoxROC.SelectedItem.ToString() == item.Item1)
                    if (!comboBoxGroep.Items.Contains(item.Item2))
                        comboBoxGroep.Items.Add(item.Item2);
            }

            if (comboBoxGroep.Items.Count > 0)
            {
                comboBoxGroep.SelectedIndex = 0;

                foreach (Tuple<String, String, String, String> item in records.Keys)
                {
                    if (comboBoxROC.SelectedItem.ToString() == item.Item1 && comboBoxGroep.SelectedItem.ToString() == item.Item2)
                        if (!comboBoxLes.Items.Contains(item.Item3))
                            comboBoxLes.Items.Add(item.Item3);
                }

                if (comboBoxLes.Items.Count > 0)
                {
                    comboBoxLes.SelectedIndex = 0;

                    foreach (Tuple<String, String, String, String> item in records.Keys)
                    {
                        if (comboBoxROC.SelectedItem.ToString() == item.Item1 && comboBoxGroep.SelectedItem.ToString() == item.Item2 && comboBoxLes.SelectedItem.ToString() == item.Item3)
                            if (!comboBoxStudent.Items.Contains(item.Item4))
                                comboBoxStudent.Items.Add(item.Item4);
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
                if (comboBoxROC.SelectedItem.ToString() == item.Item1 && comboBoxGroep.SelectedItem.ToString() == item.Item2)
                    if (!comboBoxLes.Items.Contains(item.Item3))
                        comboBoxLes.Items.Add(item.Item3);
            }

            if (comboBoxLes.Items.Count > 0)
            {
                comboBoxLes.SelectedIndex = 0;

                foreach (Tuple<String, String, String, String> item in records.Keys)
                {
                    if (comboBoxROC.SelectedItem.ToString() == item.Item1 && comboBoxGroep.SelectedItem.ToString() == item.Item2 && comboBoxLes.SelectedItem.ToString() == item.Item3)
                        if (!comboBoxStudent.Items.Contains(item.Item4))
                            comboBoxStudent.Items.Add(item.Item4);
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

            if (comboBoxStudent.Items.Count > 0)
            {
                //Select student to use
                Tuple<String, String, String, String> studentKey = new Tuple<String, String, String, String>(
                        comboBoxROC.SelectedItem.ToString(),
                        comboBoxGroep.SelectedItem.ToString(),
                        comboBoxLes.SelectedItem.ToString(),
                        comboBoxStudent.SelectedItem.ToString());

                UpdateFeedback(studentKey);
            }
        }

        private void comboBoxStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Select student to use
            Tuple<String, String, String, String> studentKey = new Tuple<String, String, String, String>(
                    comboBoxROC.SelectedItem.ToString(),
                    comboBoxGroep.SelectedItem.ToString(),
                    comboBoxLes.SelectedItem.ToString(),
                    comboBoxStudent.SelectedItem.ToString());

            UpdateFeedback(studentKey);
        }

        private void UpdateFeedback(Tuple<String, String, String, String> targetStudent)
        {
            //Een student is geselecteerd, vul alle grafieken, tabellen en genereer advies
            if (comboBoxStudent.Items.Count > 0)
            {
                fillCharts(targetStudent);
                fillTable(targetStudent);
                fillAdvice(targetStudent);
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
            if (comboBoxStudent.Items.Count > 0)
            {
                Tuple<String, String, String, String> studentKey;
                for (int student = 0; student < comboBoxStudent.Items.Count;student++)
                {

                    //Pick student to output
                    studentKey = new Tuple<String, String, String, String>(
                    comboBoxROC.SelectedItem.ToString(),
                    comboBoxGroep.SelectedItem.ToString(),
                    comboBoxLes.SelectedItem.ToString(),
                    comboBoxStudent.Items[student].ToString());

                    //Visualize selected student data
                    UpdateFeedback(studentKey);

                    //Request output
                    GenerateOutputToGraphic(studentKey);
                }

                //Reset visualizations to originally selected student
                Tuple<String, String, String, String> studentKey2 = new Tuple<String, String, String, String>(
                        
                        comboBoxROC.SelectedItem.ToString(),
                        comboBoxGroep.SelectedItem.ToString(),
                        comboBoxLes.SelectedItem.ToString(),
                        comboBoxStudent.SelectedItem.ToString());

                UpdateFeedback(studentKey2);

                //Report completed
                MessageBox.Show("Output is gemaakt");
            }
            else
            {
                MessageBox.Show("Output kon niet worden gemaakt omdat er geen student geselecteerd is.");
            }
        }

        private void emailFeedback(String directory, String file)
        {
            //This function will try to email a target file to the email address included in its filename (email_studentname)

            //Split file details
            string[] stringSeparators = new string[] { "_" };
            string[] fileName = file.Split(stringSeparators, StringSplitOptions.None);
            String targetEmail = fileName[0];
            String targetStudent = fileName[1];

            //Split directory details
            string[] directoryName = directory.Split(stringSeparators, StringSplitOptions.None);
            String targetROC = directoryName[0];
            String targetGroup = directoryName[1];
            String targetLesson = directoryName[2];

            var fromAddress = new MailAddress("splonderzoek@gmail.com", "SPL onderzoek");
            var toAddress = new MailAddress(targetEmail, targetStudent);
            const string fromPassword = "spl123Onderzoek";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                //Set up mail
                Subject = "Space Modules feedback les " + targetLesson,
                Body = "Beste student,\n\nIn deze email vind je jouw persoonlijke feedback voor les " + targetLesson +
                    ". Bekijk het plaatje in de attachments goed om te zien hoe jij het gedaan hebt ten opzichte van de groep en " +
                    "lees het advies om te zien wat je kunt verbeteren en welke tips je daarvoor krijgt.\n\n"+
                    "Vriendelijke groeten,\nSPL onderzoek",
            })
            {
                // Create the file attachment for this e-mail message.
                Attachment data = new System.Net.Mail.Attachment(defaultOutputDirectory + "\\" + directory + "\\" + file + ".bmp");

                // Add time stamp information for the file.
                ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(file);
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                disposition.ReadDate = System.IO.File.GetLastAccessTime(file);

                // Add the file attachment to this e-mail message.
                message.Attachments.Add(data);

                smtp.Send(message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Genereer target student/les
            //Pick student to output
            Tuple<String, String, String, String> studentKey = new Tuple<String, String, String, String>(
                comboBoxROC.SelectedItem.ToString(),
                comboBoxGroep.SelectedItem.ToString(),
                comboBoxLes.SelectedItem.ToString(),
                comboBoxStudent.SelectedItem.ToString());

            String file = records[studentKey].email + "_" + comboBoxStudent.SelectedItem.ToString();

            String directory = comboBoxROC.SelectedItem.ToString() + "_" +
                comboBoxGroep.SelectedItem.ToString() + "_" +
                comboBoxLes.SelectedItem.ToString();
            
            if (File.Exists(defaultOutputDirectory+"\\" + directory +"\\"+file+".bmp"))
            {
                emailFeedback(directory, file);

                //Report sent
                MessageBox.Show("Email verstuurd");
            }
            else
            {
                //Report error
                MessageBox.Show("Mail kon niet worden verstuurd omdat bestand niet bestaat");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int verstuurd = 0;
            int nietVerstuurd = 0;

            //Email feedback to all students in selected group/lesson combo
            foreach (string student in comboBoxStudent.Items)
            {
                //Genereer target student/les
                //Pick student to output
                Tuple<String, String, String, String> studentKey = new Tuple<String, String, String, String>(
                    comboBoxROC.SelectedItem.ToString(),
                    comboBoxGroep.SelectedItem.ToString(),
                    comboBoxLes.SelectedItem.ToString(),
                    student);

                String directory = comboBoxROC.SelectedItem.ToString() + "_" +
                    comboBoxGroep.SelectedItem.ToString() + "_" +
                    comboBoxLes.SelectedItem.ToString();

                //If there is a record for the student, send an email
                if (records.ContainsKey(studentKey))
                {
                    String file = records[studentKey].email + "_" + student;

                    if (File.Exists(defaultOutputDirectory + "\\" + directory + "\\" + file + ".bmp"))
                    {
                        emailFeedback(directory, file);

                        //Report sent
                        verstuurd++;
                    }
                    else
                    {
                        //Report error
                        nietVerstuurd++;
                    }
                }
                else
                {
                    //Report error
                    nietVerstuurd++;
                }
            }

            MessageBox.Show(verstuurd + " email(s) verstuurd, " + nietVerstuurd + " email(s) niet verstuurd");
        }
    }
}