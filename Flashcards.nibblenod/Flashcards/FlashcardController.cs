using Dapper;
using Flashcards.DTOs;
using Flashcards.Models;
using Microsoft.Data.SqlClient;
using static Flashcards.Enums;

namespace Flashcards
{
    internal class FlashcardController
    {
        DatabaseManager dbManager = new();

        internal List<FlashcardDto> GiveStackFlashcards(string name)
        {
            //Retrieve the ID of the Stack Name

            string id = GetIDOfStack(name);

            string command = $"SELECT * FROM Flashcards WHERE StackID = {id}";

            using (var connection = new SqlConnection(dbManager.connectionStringWithDB))
            {
                var results = connection.Query<Flashcard>(command);

                List<FlashcardDto> flashcardDTOs = new();

                int dtoID = 1;
                //Convert into DTO
                foreach (var result in results)
                {
                    flashcardDTOs.Add(DtoMapper.ToFlashcardDto(result, dtoID));
                    dtoID++;
                }

                return flashcardDTOs;
            }
        }

        internal void EditFlashcard(int id, List<FlashcardDto> flashcards, EditType editType, string updateValue)
        {
            int originalID = DtoMapper.FlashcardIDMap[id];

            using (var connection = new SqlConnection(dbManager.connectionStringWithDB))
            {
                string command = editType switch
                {
                    EditType.Front => $"UPDATE Flashcards SET Front = '{updateValue}' WHERE ID = '{originalID}'",
                    EditType.Back => $"UPDATE Flashcards SET Back = '{updateValue}' WHERE ID = '{originalID}'",
                };

                connection.Execute(command);
            }

        }

        internal void DeleteFlashcard(int id, List<FlashcardDto> flashcards)
        {
            int originalID = DtoMapper.FlashcardIDMap[id];

            using (var connection = new SqlConnection(dbManager.connectionStringWithDB))
            {
                var command = $"DELETE FROM Flashcards WHERE ID = '{originalID}'";
                connection.Execute(command);
            }

        }

        internal void CreateFlashcard(string front, string back, string stackName)
        {
            using (var connection = new SqlConnection(dbManager.connectionStringWithDB))
            {
                string command = $"SELECT ID FROM Stacks where NAME = '{stackName}'";
                int stackID = connection.ExecuteScalar<int>(command);
                command = $"INSERT INTO Flashcards VALUES ('{front}', '{back}', '{stackID}')";
                connection.Execute(command);
            }
        }


        private string GetIDOfStack(string name)
        {

            using (var connection = new SqlConnection(dbManager.connectionStringWithDB))
            {
                string command = @$"SELECT ID from Stacks where Name = '{name}'";
                string id = connection.ExecuteScalar<string>(command);

                return id;
            }
        }


    }
}
