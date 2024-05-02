using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Card{

     [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id{get;set;}

    public long DeckID{get;set;}

     public required CardDeck parent { get; set; } // Navigation property

    public required String CardName { set; get; }

    public required String CardInfo{set;get;}

    
    public String getName(){
        return CardName;
    }

    public long getId(){
        return Id;
    }

    public String getInfo(){
        return CardInfo;
    }

    public void setName(String name){
        CardName = name;
    }

    public void setInfo(String info){
        CardInfo = info;
    }



    


}