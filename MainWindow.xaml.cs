using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using TSI_6.Views;

namespace TSI_6
{
    public partial class MainWindow : Window
    {
        private string[] firstQFiles, secondQFiles;
        Dictionary<string, int> impacts = new();
        Dictionary<string, Dictionary<string, List<int>>> formResults = new();
        List<int> tempList = new();
        string result;
        int incremented = 0;
        startPage startPage = new startPage();
        QA topic = new QA();

        public MainWindow()
        {
            result = getActualDirectory();
            // get folders paths
            string firstQFolder = System.IO.Path.Combine(result, @"Questions\Вопросы оценки угроз\");
            string secondQFolder = System.IO.Path.Combine(result, @"Questions\Вопросы оценки уязвимостей\");
            // get files from those folders
            firstQFiles = Directory.GetFiles(firstQFolder);
            secondQFiles = Directory.GetFiles(secondQFolder);

            InitializeComponent();
            mainContainer.Content = startPage;
            startPage.proceed += goToQuestions;
        }
        public void goToQuestions(object sender, MyEventArgs e)
        {
            // save impact IDs
            impacts = e.MyData;
            mainContainer.Content = topic;

            // basic setup 
            nextCategory("Вопросы оценки угроз", firstQFiles, 1);
        }
        private void nextCategory(string name, string[] filesList, int fdNumber)
        {
            topic.topicName = topic.tName.Text = name;
            topic.progressBar.Value = 1;
            topic.folderNumber = fdNumber;
            topic.Files = filesList;
            topic.iterationFolder = 0;
            topic.iterationQuestion = 0;

            createNewQuestion();
        }
        private void createNewQuestion()
        {
            Category cat = new();
            cat.number = topic.iterationQuestion + 1;
            cat.category = System.IO.Path.GetFileNameWithoutExtension(topic.Files[topic.iterationFolder]);
            using (StreamReader reader = new StreamReader(topic.Files[topic.iterationFolder]))
            {
                // Read the file line by line
                string line;
                int i = 1;
                while ((line = reader.ReadLine()) != null)
                {
                    if (i++ == cat.number)
                    {
                        int index = line.IndexOf('.');
                        cat.question = line.Substring(index + 1, line.Length - index - 1);
                        break;
                    }
                }
            }

            topic.Category.Content = cat;

            cat.next += nextQuestion;
        }
        private void nextQuestion(object sender, RadioEventArgs e)
        {
            string tempName = topic.topicName;
            if (!formResults.ContainsKey(topic.topicName))
            {
                formResults.Add(topic.topicName, new Dictionary<string, List<int>>());
            }
            // pushing the "yes,no,idk" to the list in form of INT
            tempList.Add(getAnswerResult(e.selectedItem));
            //Random rand = new Random();
            //tempList.Add(rand.Next(4));

            // update the progress bar
            topic.progressBar.Value++;

            // checks to proceed with either new category or new question
            if (topic.iterationFolder <= 4)
            {
                // if it hasn't reached the end of questions INC
                if (topic.iterationQuestion < 4)
                {
                    topic.iterationQuestion++;
                }
                else if (topic.iterationFolder == 4 && topic.iterationQuestion == 4 && topic.folderNumber == 2)
                {
                    List<int> megaTemp = new();
                    foreach (var item in tempList)
                        megaTemp.Add(item);

                    formResults[tempName].Add(System.IO.Path.GetFileNameWithoutExtension(topic.Files[4]), megaTemp);
                    finishiHim();
                }
                // if it has reached the end of questions
                // reset questions and INC folder
                else
                {
                    topic.iterationQuestion = 0;
                    // if hasn't reached end of folders INC
                    if (topic.iterationFolder < 4)
                    {
                        topic.iterationFolder++;
                    }
                    // if in the first Topic and reached the end 
                    else if (topic.iterationFolder >= 4 && topic.folderNumber == 1)
                    {
                        incremented++;
                        nextCategory("Вопросы оценки уязвимостей", secondQFiles, 2);
                    }
                    else if (topic.iterationFolder >= 4 && topic.folderNumber == 2)
                    {
                        incremented++;
                    }
                    // push the list to the dic
                    if (tempList != null)
                    {
                        int index = topic.iterationFolder - 1;
                        if ((topic.iterationFolder == 0 && incremented == 1) || (topic.iterationFolder == 4 && topic.folderNumber == 2 && incremented == 1))
                            index = 4;
                        List<int> megaTemp = new();
                        foreach (var item in tempList)
                            megaTemp.Add(item);

                        formResults[tempName].Add(System.IO.Path.GetFileNameWithoutExtension(topic.Files[index]), megaTemp);
                        if (incremented == 1)
                            incremented = 0;
                        tempList.Clear();
                    }
                }
                createNewQuestion();
            }
        }
        private void finishiHim()
        {
            Finale leFInale = new(ref formResults, ref impacts);
            mainContainer.Content = leFInale;
        }
        private string getActualDirectory()
        {
            string path = System.IO.Path.GetDirectoryName(Directory.GetCurrentDirectory());
            for (int i = 0; i < 2; i++)
                path = System.IO.Path.GetDirectoryName(path);
            return path;
        }
        private int getAnswerResult(string answer)
        {
            if (answer == "Да") return 4;
            else if (answer == "Не знаю") return 2;
            else if (answer == "Нет") return 0;
            else return 0;
        }
    }
}
