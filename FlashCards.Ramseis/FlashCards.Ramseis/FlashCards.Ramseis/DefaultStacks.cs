namespace FlashCards.Ramseis
{
    internal class DefaultStacks
    {
        public static void Populate()
        {
            IO.SqlAddStack("Addition");
            int ID = IO.SqlGetStackID("Addition");
            List<string> questions = new List<string>
            {
                "1 + 1 =","2 + 2 =","3 + 3 =","4 + 4 =","5 + 5 =","6 + 6 =","7 + 7 =","8 + 8 =","9 + 9 =","10 + 10 =",
                "11 + 8 =","12 + 6 =","13 + 4 =","14 + 2 =","15 + 3 =","16 + 5 =","17 + 7 =","18 + 9 =","19 + 7 =","20 + 2 ="
            };
            List<string> answers = new List<string>
            {
                "2","4","6","8","10","12","14","16","18","20","19","18","17","16","18","21","24","27","26","22"
            };
            for (int i = 0; i < questions.Count; i++)
            {
                IO.SqlAddCard(ID, questions[i], answers[i]);
            }

            IO.SqlAddStack("Multiplication");
            ID = IO.SqlGetStackID("Multiplication");
            questions = new List<string>
            {
                "1 x 1 =","2 x 2 =","3 x 3 =","4 x 4 =","5 x 5 =","6 x 6 =","7 x 7 =","8 x 8 =","9 x 9 =","10 x 10 =",
                "11 x 8 =","12 x 6 =","13 x 4 =","14 x 2 =","15 x 3 =","16 x 5 =","17 x 7 =","18 x 9 =","19 x 7 =","20 x 2 ="
            };
            answers = new List<string>
            {
                "1","4","9","16","25","36","49","64","81","100","88","72","52","28","45","80","119","162","133","40"
            };
            for (int i = 0; i < questions.Count; i++)
            {
                IO.SqlAddCard(ID, questions[i], answers[i]);
            }

            IO.SqlAddStack("US State Capitals");
            ID = IO.SqlGetStackID("US State Capitals");
            questions = new List<string>
            {
                "Alabama","Alaska","Arizona","Arkansas","California","Colorado","Connecticut","Delaware","Florida","Georgia",
                "Hawaii","Idaho","Illinois","Indiana","Iowa","Kansas","Kentucky","Louisiana","Maine","Maryland","Massachusetts",
                "Michigan","Minnesota","Mississippi","Missouri","Montana","Nebraska","Nevada","New Hampshire","New Jersey",
                "New Mexico","New York","North Carolina","North Dakota","Ohio","Oklahoma","Oregon","Pennsylvania","Rhode Island",
                "South Carolina","South Dakota","Tennessee","Texas","Utah","Vermont","Virginia","Washington","West Virginia",
                "Wisconsin","Wyoming"
            };
            answers = new List<string>
            {
                "Montgomery","Juneau","Phoenix","Little Rock","Sacramento","Denver","Hartford","Dover","Tallahassee","Atlanta",
                "Honolulu","Boise","Springield","Indianapolis","Des Moines","Topeka","Frankfort","Baton Rouge","Augusta",
                "Annapolis","Boston","Lansing","Saint Paul","Jackson","Jefferson City","Helena","Lincoln","Carson City",
                "Concord","Trenton","Santa Fe","Albany","Raleigh","Bismarck","Columbus","Oklahoma City","Salem","Harrisburg",
                "Providence","Columbia","Pierre","Nashville","Austin","Salt Lake City","Montpelier","Richmond","Olympia",
                "Charleston","Madison","Cheyenne"
            };
            for (int i = 0; i < questions.Count; i++)
            {
                IO.SqlAddCard(ID, questions[i], answers[i]);
            }
        }
    }
}
