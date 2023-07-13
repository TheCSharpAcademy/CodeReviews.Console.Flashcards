<h1>Flashcards</h1>
<br>
This application is a part of the www.csharpacademy.com roadmap. <br>
Goal of the website is to become a (better) C# and .Net developper. Learning happens trough building projects yourself. <br>
They give the assingments, each time a bit more difficult, and provide links to the right documentation and we the students give our best to delivere a working application.<br>
When we are done they give our code a manual review in wich we recieve feedback.<br>
It's a great place to learn so go check it out. <br>
<br>
<h2>Use:</h2>
-This app allows the user to create stacks wich contains flashcards.<br>
-The user can create/modify/delete both stacks and flashcards.<br>
-There is also a study function were users can view the cards to study them, when they know the cards well enough the can test there knowledge troughout a quiz. <br>
-These test results are stored in the database and can be viewed afterwards.<br>
-There's also a show report function wich gives an overview of the number of tests and average score per month for a given year.<br>
<br>
<h2>Requirements</h2>
-For this application we used SQL for the first time, previous assignements were done with SQLite.<br>
-We need to create more than one table for the first time and link these to eachother.<br>
-When a stack is deleted, all linked records in the flashcard and study table also needed to removed along with it<br>
-Stacknames needs to be unique.<br>
-We need to use DTO's to create models that contain data from multiple tables.<br>
-Table data needed to be shown with the ConsoleTableBuilder package.<br>
-Study results needed to be stored in the database and displayed by a seperate function.br>
-As an extra challenge, you can make a report function by using a pivot statement.<br>
<br>
<h2>Lessons learned: </h2>
-Since this was our first app with SQL I needed to get familiar with using SQL Server Management Studio.<br>
-Although the SQL code is simular to SQLite code there're diffrences I needed to learn.<br>
-During this project I also learned alot of new things in SQL like using UNIQUE, how to autoincrement id's, CASCADE, PIVOT,.. So my knowledge of SQL has significently improved.<br>
-Doing the extra challenge, PIVOTING, took some time but I managed to get it working.<br>
-I also had to learn what DTO's were and how to apply them.<br>
-And maybe the biggest lesson I learned: Plan ahead before starting to code, I dived right into coding the first bit to realise after writing a couple of methods that my approach wasn't the right one.<br>
When I realised this, I drawed out the project on paper so I had a 'guidline' on where I was going and how to achieve it.<br>
<br>
<h2>Resources used:</h2>
-The assignment page on www.thecsharpacademy.com ofcourse where links were provided on how to setup sql management studio and info about DTO's and Pivoting.<br>
-Microsoft documentation.<br>
-Various google links on how to write the needed sql statements.<br>
-The Discord channel of thecsharpacademy.


