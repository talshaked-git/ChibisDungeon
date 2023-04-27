using System;
using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class Account
{
    [FirestoreDocumentId]
    public string UID { get; set; }

    [FirestoreProperty]
    public List<DocumentReference> PlayerRefs { get; set; }

    public Account()
    {
        PlayerRefs = new List<DocumentReference>();
    }

    public Account(string UID) : this()
    {
        this.UID = UID;
    }

    public void AddPlayerRef(DocumentReference playerRef)
    {
        PlayerRefs.Add(playerRef);
        FireBaseManager.instance.UpdateAccountData(this);

    }

    public DocumentReference GetAccountRef()
    {
        return FireBaseManager.instance.firebaseFirestoreManager.GetAccountRef(this);
    }

    public void RemovePlayerRef(DocumentReference playerRef)
    {
        PlayerRefs.Remove(playerRef);
        FireBaseManager.instance.UpdateAccountData(this);
    }
}
