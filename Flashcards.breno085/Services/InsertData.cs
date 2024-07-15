using flashcards.Models;
using flashcards.Repositories;

namespace flashcards.Services
{
    public class InsertData
    {
        //inserting some data in the flashcards table for tests

        public void InsertFlashcardsData()
        {
            UserRepository userRepository = new UserRepository();

            userRepository.InsertFlashcardsDataForTests(Flashcards());
        }
        public List<Flashcards> Flashcards()
        {
            var flashcards = new List<Flashcards>
            {
            CreateFlashcard("The ethereal melodies of the band created a haunting atmosphere", "As melodias etéreas da banda criaram uma atmosfera assombrosa.", 1),
            CreateFlashcard("The nebula's colors were a kaleidoscope of breathtaking beauty", "As cores da nebulosa eram um caleidoscópio de uma beleza de tirar o fôlego", 1),
            CreateFlashcard("In the vast expanse of space, they encountered a temporal anomaly", "Na vasta extensão do espaço, eles encontraram uma anomalia temporal", 1),
            CreateFlashcard("Through the wormhole, they discovered a new dimension", "Através do buraco de minhoca, eles descobriram uma nova dimensão", 1),
            CreateFlashcard("His transcendence from reality was both thrilling and terrifying", "Sua transcendência da realidade foi tanto emocionante quanto aterrorizante", 1),
            CreateFlashcard("Buongiorno", "Bom dia", 2),
            CreateFlashcard("Grazie", "Obrigado(a)", 2),
            CreateFlashcard("Dove è il bagno?", "Onde fica o banheiro?", 2),
            CreateFlashcard("Quanto costa?", "Quanto custa?", 2),
            CreateFlashcard("Mi chiamo...", "Meu nome é...", 2),
            CreateFlashcard("Hola", "Olá", 3),
            CreateFlashcard("Gracias", "Obrigado(a)", 3),
            CreateFlashcard("¿Dónde está el baño?", "Onde fica o banheiro?", 3),
            CreateFlashcard("¿Cuánto cuesta?", "Quanto custa?", 3),
            CreateFlashcard("Me llamo...", "Meu nome é...", 3)
            };

            return flashcards;
        }

        private Flashcards CreateFlashcard(string front, string back, int stackId)
        {
            return new Flashcards { Front = front, Back = back, StackId = stackId };
        }
    }
}