namespace Flashcards.Model {
    internal class Stack {

        public Stack( string id, string name ) {
            Id = id;
            Name = name;
        }

        public String Name { get; set; }
        public String Id { get; set; }
    }
}
