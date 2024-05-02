using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CardDeck
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id{set;get;}

    // Foreign key property
    public long UserId { get; set; }

    public List<Card> Cards{get;set;} = new List<Card>();

    public String nameOfDeck{get;set;}

    public int length{get;set;}

    public CardDeck(){
        nameOfDeck = "";
        length = 0;
    }

    public Card? FindCard(long id){
        for(int i = 0;i<length;i++){
            if(Cards[i].getId() == id){
                return Cards[i];
            }
        }
        return null;
    }

    public Boolean AddCard(Card card){
        if(!isFull()){
            Cards[length++] = card;
            return true;
        }
        return false;
    }

    public Boolean isFull(){
        return length == 50;
    }

    public Boolean deleteCard(long id){
        bool removed = false;
        for(int i = 0;i<length;i++){
            Cards.ForEach(e =>{
                if(e.Id == id){
                    Cards.Remove(e);
                    removed = true;
                }
            });
        }
        return removed;
    }

   
}