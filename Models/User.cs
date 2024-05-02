public class User{

     public long Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    // Navigation property for decks
    public List<CardDeck> Decks { get; set; } = new List<CardDeck>(); // Use a list instead of an array

    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public bool AddDeck(string name)
    {
        if (Decks.Count == 10)
        {
            return false;
        }
        else
        {
            var newDeck = new CardDeck {nameOfDeck = name };
            Decks.Add(newDeck);
            return true;
        }
    }


}