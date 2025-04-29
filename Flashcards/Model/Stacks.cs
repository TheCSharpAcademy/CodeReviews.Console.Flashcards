namespace Flashcards.Model
{
    public class Stacks
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public Stacks() { }

        public Stacks(int id, string name)
        {
            Id = id; 
            Name = name;
        }

        public override string ToString()
        {
            return $"{Id}: {Name}";
        }
    }
}
