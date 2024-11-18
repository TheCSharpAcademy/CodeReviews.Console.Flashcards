namespace FlashcardGame.Models
{
    internal class Stack
    {
        public int stack_id { get; set; }
        public string stack_name { get; set; }

        public Stack(int stack_id, string stack_name)
        {
            this.stack_id = stack_id;
            this.stack_name = stack_name;
        }

        public Stack()
        {

        }
    }
}
