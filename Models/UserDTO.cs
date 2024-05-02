namespace IndexCardBackendApi.Models;

public class UserDTO
{
    public required long Id {get;set;}

    public required String Username{get;set;}

    public required List<CardDeck> Decks{get;set;}



}