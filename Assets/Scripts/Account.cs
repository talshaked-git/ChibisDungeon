

public class Account
{
    private string UID { get; set; }

    public Player[] players { get; set; }



    public Account(string _UID)
    {
        UID = _UID;
        players = new Player[3];
    }

}
