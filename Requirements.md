# Introduction

After the first two projects, you should be somewhat comfortable with how things work in C#. It's time to make things slightly more complex. You'll be using SQL Server for the first time. We could continue using SQLite, as it does everything needed for most small applications, but SQL Server is heavily used in the industry and the sooner we get familiar with it, the better.

This time the database will be a little more complex as well. We will have two tables linked by a foreign key. And for the first time we will be working with DTOs (Data Transfer Objects), which will help us use the same object in different ways.

Time to get started!

<h2>Requirements</h2>

<p>This is an application where the users will create Stacks of Flashcards.</p>

<p>You'll need two different tables for stacks and flashcards. The tables should be linked by a foreign key.</p>

<p>Stacks should have an unique name.</p>

<p>Every flashcard needs to be part of a stack. If a stack is deleted, the same should happen with the flashcard.</p>

<p>You should use DTOs to show the flashcards to the user without the Id of the stack it belongs to.</p>

<p>When showing a stack to the user, the flashcard Ids should always start with 1 without gaps between them. If you have 10 cards and number 5 is deleted, the table should show Ids from 1 to 9.</p>

<p>After creating the flashcards functionalities, create a "Study Session" area, where the users will study the stacks. All study sessions should be stored, with date and score.</p>

<p>The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted.</p>

<p>The project should contain a call to the study table so the users can see all their study sessions. This table receives insert calls upon each study session, but there shouldn't be update and delete calls to it.</p>

<h2>Using SQL Server</h2>

<p>Creating linked tables with SQL.</p>

<p>Using DTOs to create different versions of classes.</p>

<p>Pivoting Tables in SQL</p>

<h2>Tips</h2>

<p>Before starting to code, try creating tables and running a few CRUD queries in SQL Server to get familiar with SQL Server Studio.</p>

<p>For management of stacks, let the user choose the stack by name.</p>

<p>Think of the "stacks" and "study" areas almost as separate applications. The study area is merely using data from the stacks area.</p>

<h2>Installing SQL Server</h2>

<p>This is the first project in the academy where we’ll be using SQL Server. You can work with SQL Server using Visual Studio.. But I recommend you start using Microsoft SQL Server Management Studio. For this app, don’t use SQL Server EXPRESS, but only LOCAL DB. Here’s a tutorial on how to install the studio. And here’s a tutorial on how to connect to your localdb.</p>

<h2>Challenge</h2>

<p>If you want to expand on this project, here’s an idea. Try to create a report system where you can see the number of sessions per month per stack. And another one with the average score per month per stack. This is not an easy challenge if you’re just getting started with databases, but it will teach you all the power of SQL and the possibilities it gives you to ask interesting questions from your tables.</p>

<h2>Review Repository</h2>

<p><https://github.com/TheCSharpAcademy/CodeReviews.Console.Flashcards></p>
