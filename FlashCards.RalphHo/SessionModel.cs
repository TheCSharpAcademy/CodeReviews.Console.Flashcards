using System;
using Microsoft.Identity.Client;
public class SessionModel
{

    public int Id {get;set;}
    public string Date {get;set;}
    public int StackId {get; set;}
    public List<FlashCardModel> FlashCards {get;set;}
    public  string Score{get;set;}

}